using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPersonaje : MonoBehaviour
{
    public float speed = 6f;
    private Rigidbody rb;
    private Vector3 movement;
    private Animator animator; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        // Obtener la entrada del teclado
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Crear un vector de movimiento con base en la entrada del teclado
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Actualizar el parámetro isMoving en el Animator
        animator.SetBool("isMoving", movement.magnitude > 0.1f); // Considerar un pequeño umbral
    }

    void FixedUpdate()
    {
        // Aplicar el movimiento al Rigidbody, multiplicado por la velocidad
        MovePlayer();
    }

    void MovePlayer()
    {
        // Mover el Rigidbody con base en el vector de movimiento
        Vector3 move = movement * speed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + move);
    }
}