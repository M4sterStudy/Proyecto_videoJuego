using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoporteCodigo : MonoBehaviour
{
    [Header("Configuraci�n de Movimiento")]
    [SerializeField] private float velocidadCaminata = 2f;      // Velocidad al caminar
    [SerializeField] private float velocidadCorrer = 5f;        // Velocidad al correr
    [SerializeField] private float distanciaCaminata = 10f;     // Distancia para comenzar a caminar al seguir al jugador
    [SerializeField] private float distanciaMaxima = 2f;        // Distancia m�xima para detenerse al jugador
    [SerializeField] private float velocidadRotacion = 120f;    // Velocidad de rotaci�n para seguir al jugador

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
            Debug.LogError("No se encontr� el jugador en la escena.");
        }
        if (animador == null)
        {
            Debug.LogError("No se encontr� el componente Animator en el objeto de soporte.");
        }
    }

    private void Update()
    {
        if (jugador == null) return;

        // Calcular la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        // Determinar el estado de movimiento seg�n la distancia al jugador
        if (distanciaAlJugador > distanciaMaxima)
        {
            if (distanciaAlJugador <= distanciaCaminata)
            {
                // Si est� dentro de la distancia de caminata, comienza a caminar
                SeguirJugador(velocidadCaminata, "caminar");
            }
            else
            {
                // Si est� fuera de la distancia de caminata, correr hacia el jugador
                SeguirJugador(velocidadCorrer, "correr");
            }
        }
        else
        {
            // Dentro de la distancia m�xima, detenerse
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

        // Activar la animaci�n de movimiento correspondiente
        ActivarAnimacion(accionAnimacion);
    }

    private void RotarHaciaJugador()
    {
        Vector3 direccionHaciaJugador = (jugador.position - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(new Vector3(direccionHaciaJugador.x, 0, direccionHaciaJugador.z));
        float anguloDiferencia = Quaternion.Angle(transform.rotation, rotacionObjetivo);

        // Girar hacia el jugador seg�n el �ngulo de diferencia
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);

        // Determinar si debe girar y en qu� direcci�n
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
        // Resetear todas las animaciones antes de activar la animaci�n deseada
        animador.SetBool("quieto", false);
        animador.SetBool("caminar", false);
        animador.SetBool("correr", false);
        animador.SetBool("girarIzquierda", false);
        animador.SetBool("girarDerecha", false);

        // Activar la animaci�n correspondiente
        animador.SetBool(nombreAnimacion, true);
    }
}
