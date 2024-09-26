using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPersonaje : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

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
            velocity.y = -2f;
        }

        // Determinamos la velocidad de movimiento basado en el estado de correr
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Convertimos el input de movimiento en un vector 3D
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Detectar el movimiento en las direcciones: adelante, atr�s, izquierda, derecha
        bool isMovingBackwards = moveInput.y < 0;
        bool isMovingLeft = moveInput.x < -0.5f;
        bool isMovingRight = moveInput.x > 0.5f;

        // Actualizamos la animaci�n: "Speed"
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
                    animator.SetFloat("Speed", -1f); // Caminar hacia atr�s
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
                    animator.SetFloat("Speed", 1f); // Caminar hacia adelante
                }
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f); // Quieto (Idle)
        }

        // Salto
        if (jumpInput && isGrounded)
        {
            isGrounded = false;
            // Determinar la direcci�n y el impulso del salto
            Vector3 jumpDirection = Vector3.up; // Impulso inicial hacia arriba

            float jumpImpulse = 0f; // Impulso horizontal adicional para saltos de esquiva
            float currentJumpHeight = jumpHeight; // Altura del salto

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
            velocity.y = Mathf.Sqrt(currentJumpHeight * -2f * gravity);
        }

        // Si est� en el suelo, regresar el estado de salto
        if (isGrounded)
        {
            animator.SetBool("isJumping", false); // Regresar a no saltar
        }

        // Aplicamos gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
}