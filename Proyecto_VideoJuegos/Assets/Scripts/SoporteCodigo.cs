using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoporteCodigo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidadCaminata = 2f;      // Velocidad al caminar
    [SerializeField] private float velocidadCorrer = 5f;        // Velocidad al correr
    [SerializeField] private float distanciaCaminata = 10f;     // Distancia para comenzar a caminar al seguir al jugador
    [SerializeField] private float distanciaMaxima = 2f;        // Distancia máxima para detenerse al jugador
    [SerializeField] private float velocidadRotacion = 120f;    // Velocidad de rotación para seguir al jugador

    private Transform jugador;                                  // Transform del jugador
    private Animator animador;                                  // Referencia al animador

    private void Start()
    {
        // Buscar el objeto del jugador por tag
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }

        animador = GetComponent<Animator>();

        if (jugador == null)
        {
            Debug.LogError("No se encontró el jugador en la escena.");
        }
        if (animador == null)
        {
            Debug.LogError("No se encontró el componente Animator en el objeto de soporte.");
        }
    }

    private void Update()
    {
        if (jugador == null) return;

        // Calcular la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        // Determinar el estado de movimiento según la distancia al jugador
        if (distanciaAlJugador > distanciaMaxima)
        {
            if (distanciaAlJugador <= distanciaCaminata)
            {
                // Si está dentro de la distancia de caminata, comienza a caminar
                SeguirJugador(velocidadCaminata, "caminar");
            }
            else
            {
                // Si está fuera de la distancia de caminata, correr hacia el jugador
                SeguirJugador(velocidadCorrer, "correr");
            }
        }
        else
        {
            // Dentro de la distancia máxima, detenerse
            Detenerse();
        }

        // Rotar hacia el jugador
        RotarHaciaJugador();
    }

    private void SeguirJugador(float velocidad, string accionAnimacion)
    {
        // Avanzar hacia el jugador en el plano horizontal
        Vector3 direccionHaciaJugador = (jugador.position - transform.position).normalized;
        direccionHaciaJugador.y = 0;
        transform.Translate(direccionHaciaJugador * velocidad * Time.deltaTime, Space.World);

        // Activar la animación de movimiento correspondiente
        ActivarAnimacion(accionAnimacion);
    }

    private void RotarHaciaJugador()
    {
        Vector3 direccionHaciaJugador = (jugador.position - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(new Vector3(direccionHaciaJugador.x, 0, direccionHaciaJugador.z));
        float anguloDiferencia = Quaternion.Angle(transform.rotation, rotacionObjetivo);

        // Girar hacia el jugador según el ángulo de diferencia
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);

        // Determinar si debe girar y en qué dirección
        if (anguloDiferencia > 5f)
        {
            if (Vector3.Cross(transform.forward, direccionHaciaJugador).y > 0)
            {
                ActivarAnimacion("girarIzquierda");
            }
            else
            {
                ActivarAnimacion("girarDerecha");
            }
        }
    }

    private void Detenerse()
    {
        ActivarAnimacion("quieto");
    }

    private void ActivarAnimacion(string nombreAnimacion)
    {
        // Resetear todas las animaciones antes de activar la animación deseada
        animador.SetBool("quieto", false);
        animador.SetBool("caminar", false);
        animador.SetBool("correr", false);
        animador.SetBool("girarIzquierda", false);
        animador.SetBool("girarDerecha", false);

        // Activar la animación correspondiente
        animador.SetBool(nombreAnimacion, true);
    }
}
