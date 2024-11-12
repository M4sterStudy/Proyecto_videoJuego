using System.Collections;
using UnityEngine;

public class ExplosionDiablillo : MonoBehaviour
{
    [Header("Configuraci�n de Explosi�n")]
    [SerializeField] private ParticleSystem sistemaParticulas; // Sistema de part�culas de explosi�n
    [SerializeField] private float tiempoDesactivacion = 2f; // Tiempo despu�s de la explosi�n para desactivar el objeto

    private Transform jugador;
    private Animator animador;
    private ComportamientoGeneralEnemigos comportamientoEnemigo;
    private Rigidbody rb; // Referencia al Rigidbody
    private bool haExplotado = false;

    private void Start()
    {
        // Buscar el objeto del jugador
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }

        animador = GetComponent<Animator>();
        comportamientoEnemigo = GetComponent<ComportamientoGeneralEnemigos>();
        rb = GetComponent<Rigidbody>();

        if (sistemaParticulas == null)
        {
            Debug.LogError("Sistema de part�culas no asignado en el inspector.");
        }

        if (comportamientoEnemigo == null)
        {
            Debug.LogError("No se encontr� el componente ComportamientoGeneralEnemigos en este objeto.");
        }

        if (rb == null)
        {
            Debug.LogError("No se encontr� el componente Rigidbody en este objeto.");
        }
    }

    private void Update()
    {
        // No hacer nada si ya explot� o si faltan componentes
        if (haExplotado || jugador == null || comportamientoEnemigo == null) return;

        // Calcular la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        // Activar la explosi�n si est� dentro de la distancia de detenci�n
        if (distanciaAlJugador <= comportamientoEnemigo.distanciaDetencion)
        {
            ActivarExplosi�n();
        }
    }

    private void ActivarExplosi�n()
    {
        haExplotado = true;

        // Congelar posici�n y rotaci�n en el Rigidbody para evitar cualquier movimiento o rotaci�n
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }

        // Reproducir el sistema de part�culas y activar animaci�n de muerte
        if (sistemaParticulas != null)
        {
            sistemaParticulas.Play();
        }

        if (animador != null)
        {
            animador.SetBool("diablilloMuerte", true);
        }

        // Desactivar el objeto despu�s de un retraso
        StartCoroutine(DesactivarObjeto());
    }

    private IEnumerator DesactivarObjeto()
    {
        yield return new WaitForSeconds(tiempoDesactivacion);
        gameObject.SetActive(false);
    }
}
