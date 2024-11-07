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
        // Deshabilitamos los controles cuando el script no está activo
        inputActions.personaje.Disable();
    }

    void Update()
    {
        // Comprobamos si está en el suelo
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Asegura que el personaje se mantenga en el suelo
        }

        // Determinamos la velocidad de movimiento basado en el estado de correr
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Gasta energía al correr
        if (isRunning && energiaActual > 0)
        {
            // Se gasta energía mientras corres
            energiaActual -= Time.deltaTime * 10; // Gasta 10 de energía por segundo al correr
        }
        else
        {
            // Si no está corriendo o no hay energía, no se puede correr
            isRunning = false;
        }
        if (!isRunning && energiaActual < 100)
        {
            energiaActual += Time.deltaTime * 10; // Gasta 10 de energía por segundo al correr
        }

        // Convertimos el input de movimiento en un vector 3D basado en la cámara
        Vector3 move = cameraTransform.TransformDirection(moveInput.x, 0, moveInput.y);
        move.y = 0; // Mantener el movimiento en el plano horizontal

        // Movimiento lateral y hacia adelante
        Vector3 forwardMove = moveInput.y * transform.forward * currentSpeed;
        Vector3 lateralMove = moveInput.x * transform.right * currentSpeed;

        // Solo rotar hacia adelante cuando el movimiento es hacia adelante
        if (moveInput.magnitude > 0)
        {
            // Si se mueve hacia adelante (moveInput.y > 0) o hacia atrás (no rotar al ir hacia atrás)
            if (moveInput.y > 0) // Solo rota hacia adelante
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // Mueve al personaje hacia adelante y lateralmente
        controller.Move((forwardMove + lateralMove) * Time.deltaTime); // Combina ambos movimientos

        // Actualizamos la animación "Speed"
        UpdateAnimation();

        // Manejar el salto
        HandleJump();

        // Aplicamos gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Actualizar las barras de vida y energía
        UpdateUIBars();
    }

    private void UpdateAnimation()
    {
        // Detectar el movimiento en las direcciones: adelante, atrás, izquierda, derecha
        bool isMovingBackwards = moveInput.y < 0;
        bool isMovingLeft = moveInput.x < -0.5f;
        bool isMovingRight = moveInput.x > 0.5f;

        if (moveInput.magnitude > 0) // Si el personaje se está moviendo
        {
            if (isMovingBackwards)  // Movimiento hacia atrás
            {
                if (isRunning)
                {
                    animator.SetFloat("Speed", -2f); // Correr hacia atrás
                }
                else
                {
                    if (moveInput.y < 0 && moveInput.y > -0.5)
                    {
                        animator.SetFloat("Speed", -1f); // Caminar hacia atrás
                    }
                    else
                    {
                        animator.SetFloat("Speed", -7f); // Caminar hacia atrás
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
                        animator.SetFloat("Speed", 7f); // Caminar hacia atrás
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
            // Si el personaje tiene energía, permite saltar
            if (energiaActual > 0)
            {
                isGrounded = false;

                // Determinar la dirección y el impulso del salto
                Vector3 jumpDirection = Vector3.up; // Impulso inicial hacia arriba

                float jumpImpulse = 0f; // Impulso horizontal adicional para saltos de esquiva
                float currentJumpHeight = jumpHeight; // Altura del salto

                // Detectar si el personaje está en movimiento
                bool isMovingBackwards = moveInput.y < 0;
                bool isMovingLeft = moveInput.x < -0.5f;
                bool isMovingRight = moveInput.x > 0.5f;

                if (moveInput.magnitude > 0) // Si el personaje está en movimiento
                {
                    if (isMovingBackwards) // Esquivar hacia atrás
                    {
                        jumpDirection += -transform.forward; // Añadir impulso hacia atrás
                        jumpImpulse = 5f; // La velocidad de esquiva hacia atrás
                        currentJumpHeight = jumpHeight * 0.5f; // Altura más baja para la esquiva
                        animator.SetFloat("JumpDirection", -1f); // Animación de esquiva hacia atrás
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
                    animator.SetFloat("JumpDirection", 0f); // Animación de salto Idle
                }

                // Actualizar parámetros del Animator
                animator.SetBool("isJumping", true);

                // Aplicar impulso horizontal para esquivar
                controller.Move(jumpDirection.normalized * jumpImpulse * Time.deltaTime);

                // Aplicar el impulso de salto vertical
                velocity.y = Mathf.Sqrt(currentJumpHeight * -2f * gravity); // Calcular el salto

                energiaActual -= 15; // Gasta 15 de energía por salto                
            }
            else
            {
                Debug.Log("No tienes suficiente energía para saltar.");
            }

        }

        // Si está en el suelo, regresar el estado de salto
        if (isGrounded)
        {
            animator.SetBool("isJumping", false); // Regresar a no saltar
        }
    }

    // Función que se llama cuando se recibe el input de movimiento
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Función que se llama cuando se cancela el input de movimiento
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            TakeDamage(20);
            Debug.Log("¡recibiste daño!");
        }
        else if (other.CompareTag("Heal"))
        {
            Heal(20);
            Debug.Log("¡te has curado!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Lava"))
        {
            vidaActual = 0; // Vida a 0 al tocar "Lava"
            Die();
            Debug.Log("¡Has caído en lava! Vida reducida a 0.");
        }
    }

    public void TakeDamage(int damage)
    {
        vidaActual -= damage; // Resta el daño de la salud actual

        if (vidaActual <= 0)
        {
            Die(); // Llama a la función de morir si la salud es 0 o menos
        }
    }

    public void Heal(int healAmount)
    {
        vidaActual += healAmount; // Suma la cantidad de curación

        // Asegúrate de que la salud no supere el máximo
        if (vidaActual > VidaMax)
        {
            vidaActual = VidaMax;
        }
    }

    private void Die()
    {
        Debug.Log("El personaje ha muerto.");
        // Aquí puedes añadir lógica adicional al morir, como reproducir una animación, desactivar el objeto, etc.
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

        // Actualizamos la barra de energía con fillAmount
        energyBar.fillAmount = energiaActual / energiaMax;
    }

}
