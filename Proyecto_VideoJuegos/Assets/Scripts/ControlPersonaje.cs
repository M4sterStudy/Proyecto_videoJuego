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

        // Determinamos la velocidad según si corre o camina
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Convertimos el input de movimiento en un vector 3D
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Actualizamos la animación
        speed = moveInput.magnitude; // 0 = Idle, 1 = Walk, 2 = Run
        animator.SetFloat("Speed", speed);

        // Salto
        if (jumpInput && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
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