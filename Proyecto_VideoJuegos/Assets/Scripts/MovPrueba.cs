using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovPrueba : MonoBehaviour
{
    public float velocidad = 6f;
    public float fuerzaSalto = 5f;
    public Transform puntoTeletransporte; // Punto al que se teletransporta el jugador

    private Rigidbody rb;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movimiento básico con A, W, S, D
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;
        rb.MovePosition(transform.position + mover * velocidad * Time.deltaTime);

        // Saltar si está en el suelo
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Validar si el jugador está tocando el suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }

        // Si el jugador toca la lava, se teletransporta
        if (collision.gameObject.CompareTag("Lava"))
        {
            transform.position = puntoTeletransporte.position;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Si deja de tocar el suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }
}
