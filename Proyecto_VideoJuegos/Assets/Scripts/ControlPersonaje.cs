using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ControlPersonaje : MonoBehaviour
{
    [Header("GameObjects")]
    public CharacterController controller;
    public Animator animator;
    public Transform cameraTransform;
    public Image healthBar;
    public Image energyBar;
    public TextMeshProUGUI energiaInfinitaTexto; // Referencia al TextMeshPro para la cuenta regresiva

    [Header("Fisicas")]
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector2 moveInput;
    private bool jumpInput;
    private bool isRunning;

    [Header("Estadisticas")]
    public int vidaActual;
    public int VidaMax = 100;
    public float energiaActual;
    public float energiaMax = 100;
    public float tasaRegeneracion = 5f;

    private bool energiaInfinita = false; // Nueva variable para energía infinita

    [Header("Terceros")]
    private Player inputActions; // Clase generada por el Input System
    private bool isInCofreRange = false;
    private Collider cofreCollider;

    private void Awake()
    {
        inputActions = new Player();
    }

    private void Start()
    {
        vidaActual = VidaMax;
        energiaActual = energiaMax;

        healthBar.fillAmount = (float)vidaActual / VidaMax;
        energyBar.fillAmount = energiaActual / energiaMax;

        energiaInfinitaTexto.gameObject.SetActive(false); // Esconde el texto de cuenta regresiva al inicio
    }

    private void OnEnable()
    {
        inputActions.personaje.Enable();

        inputActions.personaje.movimiento.performed += OnMovePerformed;
        inputActions.personaje.movimiento.canceled += OnMoveCanceled;

        inputActions.personaje.saltar.performed += ctx => jumpInput = true;
        inputActions.personaje.saltar.canceled += ctx => jumpInput = false;

        inputActions.personaje.correr.performed += ctx => isRunning = true;
        inputActions.personaje.correr.canceled += ctx => isRunning = false;

        inputActions.personaje.interactuar.performed += OnAccionPerformed;
    }

    private void OnDisable()
    {
        inputActions.personaje.Disable();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (isRunning && (energiaActual > 0 || energiaInfinita))
        {
            if (!energiaInfinita)
            {
                energiaActual -= Time.deltaTime * 10;
            }
        }
        else
        {
            isRunning = false;
        }
        if (!isRunning && energiaActual < 100)
        {
            energiaActual += Time.deltaTime * 10;
        }

        Vector3 move = cameraTransform.TransformDirection(moveInput.x, 0, moveInput.y);
        move.y = 0;

        Vector3 forwardMove = moveInput.y * transform.forward * currentSpeed;
        Vector3 lateralMove = moveInput.x * transform.right * currentSpeed;

        if (moveInput.magnitude > 0)
        {
            if (moveInput.y > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        controller.Move((forwardMove + lateralMove) * Time.deltaTime);

        UpdateAnimation();

        HandleJump();

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        UpdateUIBars();
    }

    private void UpdateAnimation()
    {
        bool isMovingBackwards = moveInput.y < 0;
        bool isMovingLeft = moveInput.x < -0.5f;
        bool isMovingRight = moveInput.x > 0.5f;

        if (moveInput.magnitude > 0)
        {
            if (isMovingBackwards)
            {
                animator.SetFloat("Speed", isRunning ? -2f : -1f);
            }
            else if (isMovingLeft)
            {
                animator.SetFloat("Speed", isRunning ? 4f : 3f);
            }
            else if (isMovingRight)
            {
                animator.SetFloat("Speed", isRunning ? 6f : 5f);
            }
            else
            {
                animator.SetFloat("Speed", isRunning ? 2f : 1f);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private void HandleJump()
    {
        if (jumpInput && isGrounded)
        {
            if (energiaActual > 0 || energiaInfinita)
            {
                isGrounded = false;

                Vector3 jumpDirection = Vector3.up;
                float jumpImpulse = 0f;
                float currentJumpHeight = jumpHeight;

                bool isMovingBackwards = moveInput.y < 0;
                bool isMovingLeft = moveInput.x < -0.5f;
                bool isMovingRight = moveInput.x > 0.5f;

                if (moveInput.magnitude > 0)
                {
                    if (isMovingBackwards)
                    {
                        jumpDirection += -transform.forward;
                        jumpImpulse = 5f;
                        currentJumpHeight = jumpHeight * 0.5f;
                        animator.SetFloat("JumpDirection", -1f);
                    }
                    else if (isMovingLeft)
                    {
                        jumpDirection += -transform.right;
                        jumpImpulse = 5f;
                        currentJumpHeight = jumpHeight * 0.5f;
                        animator.SetFloat("JumpDirection", 2f);
                    }
                    else if (isMovingRight)
                    {
                        jumpDirection += transform.right;
                        jumpImpulse = 5f;
                        currentJumpHeight = jumpHeight * 0.5f;
                        animator.SetFloat("JumpDirection", -2f);
                    }
                    else
                    {
                        jumpDirection += transform.forward;
                        jumpImpulse = 5f;
                        currentJumpHeight = jumpHeight * 0.5f;
                        animator.SetFloat("JumpDirection", 1f);
                    }
                }
                else
                {
                    animator.SetFloat("JumpDirection", 0f);
                }

                animator.SetBool("isJumping", true);
                controller.Move(jumpDirection.normalized * jumpImpulse * Time.deltaTime);
                velocity.y = Mathf.Sqrt(currentJumpHeight * -2f * gravity);

                // Resta energía solo si no está activa la energía infinita
                if (!energiaInfinita)
                {
                    energiaActual -= 15;
                }
            }
            else
            {
                Debug.Log("No tienes suficiente energía para saltar.");
            }
        }

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            TakeDamage(20);
            Debug.Log("¡Recibiste daño!");
        }
        else if (other.CompareTag("Heal"))
        {
            Heal(20);
            Debug.Log("¡Te has curado!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Lava"))
        {
            vidaActual = 0;
            Die();
            Debug.Log("¡Has caído en lava! Vida reducida a 0.");
        }
        else if (other.CompareTag("cofre"))
        {
            isInCofreRange = true;
            cofreCollider = other;
            Debug.Log("Cerca de un cofre. Presiona la tecla designada para abrir.");
        }
        else if (other.CompareTag("vida"))
        {
            Heal(20);
            Debug.Log("Has tocado un objeto de vida. ¡Curado 20 puntos de vida!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("energia"))
        {
            StartCoroutine(EnergiaInfinita());
            Debug.Log("Has tocado un objeto de energía. ¡Energía infinita activada por 30 segundos!");
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("cofre"))
        {
            isInCofreRange = false;
            cofreCollider = null;
            Debug.Log("Fuera del rango del cofre.");
        }
    }

    private void OnAccionPerformed(InputAction.CallbackContext context)
    {
        if (isInCofreRange && cofreCollider != null)
        {
            StartCoroutine(OpenCofre());
        }
    }

    private IEnumerator OpenCofre()
    {
        // Ejecutar animación de abrir cofre
        animator.SetBool("abrirCofre", true);
        Debug.Log("Abriendo cofre...");

        // Esperar 2 segundos para simular la animación de abrir
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("abrirCofre", false);

        // Destruir el cofre después de la animación
        if (cofreCollider != null)
        {
            Destroy(cofreCollider.gameObject); // Destruir el cofre que activó la colisión
            Debug.Log("¡Cofre abierto y destruido!");
        }

        // Salir del rango del cofre
        isInCofreRange = false;
        cofreCollider = null;
    }

    private IEnumerator EnergiaInfinita()
    {
        energiaInfinita = true;
        float duracion = 30f;

        energiaInfinitaTexto.gameObject.SetActive(true); // Muestra el texto al activar energía infinita

        while (duracion > 0)
        {
            energiaInfinitaTexto.text = "Energía infinita: " + Mathf.Ceil(duracion) + "s";
            duracion -= Time.deltaTime;
            yield return null;
        }

        energiaInfinitaTexto.gameObject.SetActive(false); // Esconde el texto cuando se acaba el tiempo
        energiaInfinita = false;
    }

    void Heal(int amount)
    {
        vidaActual = Mathf.Min(vidaActual + amount, VidaMax);
        UpdateUIBars();
    }

    public void TakeDamage(int amount)
    {
        vidaActual -= amount;
        UpdateUIBars();
        if (vidaActual <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        vidaActual = 0;
        Debug.Log("Has muerto.");
    }

    private void UpdateUIBars()
    {
        healthBar.fillAmount = (float)vidaActual / VidaMax;
        energyBar.fillAmount = energiaActual / energiaMax;
    }
}
