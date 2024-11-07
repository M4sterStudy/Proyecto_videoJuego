using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlPersonaje : MonoBehaviour
{
    [Header("GameObjects")]
    public CharacterController controller;
    public Animator animator;
    public Transform cameraTransform;
    public Image healthBar;
    public Image energyBar;

    [Header("Fisicas")]
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Estadisticas")]
    public int vidaActual;
    public int VidaMax = 100;
    public float energiaActual;
    public float energiaMax = 100;
    public float tasaRegeneracion = 5f;

    private Vector3 velocity;
    private bool isGrounded;

    // Variables del nuevo Input System
    private Vector2 moveInput;
    private bool jumpInput;
    private bool isRunning;

    private Player inputActions; // Clase generada por el Input System

    private void Awake()
    {
        // Inicializamos los bindings del Input System
        inputActions = new Player();
    }

    private void Start()
    {
        vidaActual = VidaMax;
        energiaActual = energiaMax;

        // Inicializar las barras
        healthBar.fillAmount = (float)vidaActual / VidaMax;
        energyBar.fillAmount = energiaActual / energiaMax;
    }

    private void OnEnable()
    {
        // Activamos los controles cuando se habilita el script
        inputActions.personaje.Enable();

        // Asignamos funciones para cuando el input ocurra
        inputActions.personaje.movimiento.performed += OnMovePerformed;
        inputActions.personaje.movimiento.canceled += OnMoveCanceled;

        inputActions.personaje.saltar.performed += ctx => jumpInput = true;
        inputActions.personaje.saltar.canceled += ctx => jumpInput = false;

        inputActions.personaje.correr.performed += ctx => isRunning = true;
        inputActions.personaje.correr.canceled += ctx => isRunning = false;
    }

    private void OnDisable()
    {
        // Deshabilitamos los controles cuando el script no est� activo
        inputActions.personaje.Disable();
    }

    void Update()
    {
        // Comprobamos si est� en el suelo
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Asegura que el personaje se mantenga en el suelo
        }

        // Determinamos la velocidad de movimiento basado en el estado de correr
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Gasta energ�a al correr
        if (isRunning && energiaActual > 0)
        {
            // Se gasta energ�a mientras corres
            energiaActual -= Time.deltaTime * 10; // Gasta 10 de energ�a por segundo al correr
        }
        else
        {
            // Si no est� corriendo o no hay energ�a, no se puede correr
            isRunning = false;
        }
        if (!isRunning && energiaActual < 100)
        {
            energiaActual += Time.deltaTime * 10; // Gasta 10 de energ�a por segundo al correr
        }

        // Convertimos el input de movimiento en un vector 3D basado en la c�mara
        Vector3 move = cameraTransform.TransformDirection(moveInput.x, 0, moveInput.y);
        move.y = 0; // Mantener el movimiento en el plano horizontal

        // Movimiento lateral y hacia adelante
        Vector3 forwardMove = moveInput.y * transform.forward * currentSpeed;
        Vector3 lateralMove = moveInput.x * transform.right * currentSpeed;

        // Solo rotar hacia adelante cuando el movimiento es hacia adelante
        if (moveInput.magnitude > 0)
        {
            // Si se mueve hacia adelante (moveInput.y > 0) o hacia atr�s (no rotar al ir hacia atr�s)
            if (moveInput.y > 0) // Solo rota hacia adelante
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // Mueve al personaje hacia adelante y lateralmente
        controller.Move((forwardMove + lateralMove) * Time.deltaTime); // Combina ambos movimientos

        // Actualizamos la animaci�n "Speed"
        UpdateAnimation();

        // Manejar el salto
        HandleJump();

        // Aplicamos gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Actualizar las barras de vida y energ�a
        UpdateUIBars();
    }

    private void UpdateAnimation()
    {
        // Detectar el movimiento en las direcciones: adelante, atr�s, izquierda, derecha
        bool isMovingBackwards = moveInput.y < 0;
        bool isMovingLeft = moveInput.x < -0.5f;
        bool isMovingRight = moveInput.x > 0.5f;

        if (moveInput.magnitude > 0) // Si el personaje se est� moviendo
        {
            if (isMovingBackwards)  // Movimiento hacia atr�s
            {
                if (isRunning)
                {
                    animator.SetFloat("Speed", -2f); // Correr hacia atr�s
                }
                else
                {
                    if (moveInput.y < 0 && moveInput.y > -0.5)
                    {
                        animator.SetFloat("Speed", -1f); // Caminar hacia atr�s
                    }
                    else
                    {
                        animator.SetFloat("Speed", -7f); // Caminar hacia atr�s
                    }

                }
            }
            else if (isMovingLeft)  // Movimiento hacia la izquierda
            {
                if (isRunning)
                {
                    animator.SetFloat("Speed", 4f); // Correr hacia la izquierda
                }
                else
                {
                    animator.SetFloat("Speed", 3f); // Caminar hacia la izquierda
                }
            }
            else if (isMovingRight)  // Movimiento hacia la derecha
            {
                if (isRunning)
                {
                    animator.SetFloat("Speed", 6f); // Correr hacia la derecha
                }
                else
                {
                    animator.SetFloat("Speed", 5f); // Caminar hacia la derecha
                }
            }
            else  // Movimiento hacia adelante
            {
                if (isRunning)
                {
                    animator.SetFloat("Speed", 2f); // Correr hacia adelante
                }
                else
                {
                    if (moveInput.y > 0 && moveInput.y < 0.5)
                    {
                        animator.SetFloat("Speed", 1f); // Caminar hacia adelante
                    }
                    else
                    {
                        animator.SetFloat("Speed", 7f); // Caminar hacia atr�s
                    }

                }
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f); // Quieto (Idle)
        }
    }

    private void HandleJump()
    {
        if (jumpInput && isGrounded)
        {
            // Si el personaje tiene energ�a, permite saltar
            if (energiaActual > 0)
            {
                isGrounded = false;

                // Determinar la direcci�n y el impulso del salto
                Vector3 jumpDirection = Vector3.up; // Impulso inicial hacia arriba

                float jumpImpulse = 0f; // Impulso horizontal adicional para saltos de esquiva
                float currentJumpHeight = jumpHeight; // Altura del salto

                // Detectar si el personaje est� en movimiento
                bool isMovingBackwards = moveInput.y < 0;
                bool isMovingLeft = moveInput.x < -0.5f;
                bool isMovingRight = moveInput.x > 0.5f;

                if (moveInput.magnitude > 0) // Si el personaje est� en movimiento
                {
                    if (isMovingBackwards) // Esquivar hacia atr�s
                    {
                        jumpDirection += -transform.forward; // A�adir impulso hacia atr�s
                        jumpImpulse = 5f; // La velocidad de esquiva hacia atr�s
                        currentJumpHeight = jumpHeight * 0.5f; // Altura m�s baja para la esquiva
                        animator.SetFloat("JumpDirection", -1f); // Animaci�n de esquiva hacia atr�s
                    }
                    else if (isMovingLeft) // Esquivar hacia la izquierda
                    {
                        jumpDirection += -transform.right;
                        jumpImpulse = 5f;
                        currentJumpHeight = jumpHeight * 0.5f;
                        animator.SetFloat("JumpDirection", 2f);
                    }
                    else if (isMovingRight) // Esquivar hacia la derecha
                    {
                        jumpDirection += transform.right;
                        jumpImpulse = 5f;
                        currentJumpHeight = jumpHeight * 0.5f;
                        animator.SetFloat("JumpDirection", -2f);
                    }
                    else // Esquivar hacia adelante
                    {
                        jumpDirection += transform.forward;
                        jumpImpulse = 5f;
                        currentJumpHeight = jumpHeight * 0.5f;
                        animator.SetFloat("JumpDirection", 1f);
                    }
                }
                else
                {
                    // Saltar desde Idle
                    animator.SetFloat("JumpDirection", 0f); // Animaci�n de salto Idle
                }

                // Actualizar par�metros del Animator
                animator.SetBool("isJumping", true);

                // Aplicar impulso horizontal para esquivar
                controller.Move(jumpDirection.normalized * jumpImpulse * Time.deltaTime);

                // Aplicar el impulso de salto vertical
                velocity.y = Mathf.Sqrt(currentJumpHeight * -2f * gravity); // Calcular el salto

                energiaActual -= 15; // Gasta 15 de energ�a por salto                
            }
            else
            {
                Debug.Log("No tienes suficiente energ�a para saltar.");
            }

        }

        // Si est� en el suelo, regresar el estado de salto
        if (isGrounded)
        {
            animator.SetBool("isJumping", false); // Regresar a no saltar
        }
    }

    // Funci�n que se llama cuando se recibe el input de movimiento
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Funci�n que se llama cuando se cancela el input de movimiento
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            TakeDamage(20);
            Debug.Log("�recibiste da�o!");
        }
        else if (other.CompareTag("Heal"))
        {
            Heal(20);
            Debug.Log("�te has curado!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Lava"))
        {
            vidaActual = 0; // Vida a 0 al tocar "Lava"
            Die();
            Debug.Log("�Has ca�do en lava! Vida reducida a 0.");
        }
    }

    public void TakeDamage(int damage)
    {
        vidaActual -= damage; // Resta el da�o de la salud actual

        if (vidaActual <= 0)
        {
            Die(); // Llama a la funci�n de morir si la salud es 0 o menos
        }
    }

    public void Heal(int healAmount)
    {
        vidaActual += healAmount; // Suma la cantidad de curaci�n

        // Aseg�rate de que la salud no supere el m�ximo
        if (vidaActual > VidaMax)
        {
            vidaActual = VidaMax;
        }
    }

    private void Die()
    {
        Debug.Log("El personaje ha muerto.");
        // Aqu� puedes a�adir l�gica adicional al morir, como reproducir una animaci�n, desactivar el objeto, etc.
        gameObject.SetActive(false); // Desactiva el objeto del jugador
    }

    public int GetCurrentHealth()
    {
        return vidaActual;
    }

    private void UpdateUIBars()
    {
        // Actualizamos la barra de vida con fillAmount
        healthBar.fillAmount = (float)vidaActual / VidaMax;

        // Actualizamos la barra de energ�a con fillAmount
        energyBar.fillAmount = energiaActual / energiaMax;
    }

}
