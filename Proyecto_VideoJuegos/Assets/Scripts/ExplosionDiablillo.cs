using System.Collections;
using UnityEngine;

public class ExplosionDiablillo : MonoBehaviour
{
    [Header("Configuración de Explosión")]
    [SerializeField] private ParticleSystem sistemaParticulas; // Sistema de partículas de explosión
    [SerializeField] private float tiempoDesactivacion = 2f; // Tiempo después de la explosión para desactivar el objeto

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
            Debug.LogError("Sistema de partículas no asignado en el inspector.");
        }

        if (comportamientoEnemigo == null)
        {
            Debug.LogError("No se encontró el componente ComportamientoGeneralEnemigos en este objeto.");
        }

        if (rb == null)
        {
            Debug.LogError("No se encontró el componente Rigidbody en este objeto.");
        }
    }

    private void Update()
    {
        // No hacer nada si ya explotó o si faltan componentes
        if (haExplotado || jugador == null || comportamientoEnemigo == null) return;

        // Calcular la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        // Activar la explosión si está dentro de la distancia de detención
        if (distanciaAlJugador <= comportamientoEnemigo.distanciaDetencion)
        {
            ActivarExplosión();
        }
    }

    private void ActivarExplosión()
    {
        haExplotado = true;

        // Congelar posición y rotación en el Rigidbody para evitar cualquier movimiento o rotación
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }

        // Reproducir el sistema de partículas y activar animación de muerte
        if (sistemaParticulas != null)
        {
            sistemaParticulas.Play();
        }

        if (animador != null)
        {
            animador.SetBool("diablilloMuerte", true);
        }

        // Desactivar el objeto después de un retraso
        StartCoroutine(DesactivarObjeto());
    }

    private IEnumerator DesactivarObjeto()
    {
        yield return new WaitForSeconds(tiempoDesactivacion);
        gameObject.SetActive(false);
    }
}
