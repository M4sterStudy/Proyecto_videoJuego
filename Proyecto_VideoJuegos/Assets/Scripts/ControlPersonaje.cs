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

    private float speed;       

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
        // Deshabilitamos los controles cuando el script no está activo
        inputActions.personaje.Disable();
    }

    void Update()
    {
        // Comprobamos si está en el suelo
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

        // Detectar el movimiento en las direcciones: adelante, atrás, izquierda, derecha
        bool isMovingBackwards = moveInput.y < 0;
        bool isMovingLeft = moveInput.x < 0;
        bool isMovingRight = moveInput.x > 0;

        // Actualizamos la animación: "Speed"
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
                    animator.SetFloat("Speed", -1f); // Caminar hacia atrás
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
            animator.SetBool("isJumping", true);
            animator.SetFloat("JumpDirection", 0f);

            if (isMovingBackwards) // Hacia atrás
            {
                isGrounded = false;
                animator.SetBool("isJumping", true);
                animator.SetFloat("JumpDirection", -1f);
            }
            else if (isMovingLeft) // Hacia la izquierda
            {
                isGrounded = false;
                animator.SetBool("isJumping", true);
                animator.SetFloat("JumpDirection", 2f);
            }
            else if (isMovingRight) // Hacia la derecha
            {
                isGrounded = false;
                animator.SetBool("isJumping", true);
                animator.SetFloat("JumpDirection", -2f);
            }
            else if (moveInput.magnitude > 0) // Hacia adelante
            {
                isGrounded = false;
                animator.SetBool("isJumping", true);
                animator.SetFloat("JumpDirection", 1f);
            }

            // Actualizamos los parámetros
             // Indicar que está saltando

            // Aplicar la física del salto
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Si está en el suelo, regresar el estado de salto
        if (isGrounded)
        {
            animator.SetBool("isJumping", false); // Regresar a no saltar
        }

        // Aplicamos gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
}