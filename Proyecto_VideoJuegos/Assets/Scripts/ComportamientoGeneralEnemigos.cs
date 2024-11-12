using System.Collections;
using UnityEngine;

public class ComportamientoGeneralEnemigos : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidadMovimiento = 2f;
    [SerializeField] private float velocidadRotacion = 120f;
    [SerializeField] private float tiempoCambioDireccion = 2f;
    [SerializeField] private float tiempoPausa = 1f;
    [SerializeField] private float distanciaMaxima = 10f;

    [Header("Configuración de Persecución")]
    [SerializeField] private float distanciaPersecucion = 5f;
    public float distanciaDetencion = 1.5f;
    [SerializeField] private float velocidadRetorno = 3f; // Velocidad para volver al punto inicial

    private Transform jugador;
    private Animator animador;
    private Vector3 direccionMovimiento;
    private Quaternion rotacionObjetivo;
    private bool rotando;
    private bool enPausa;
    private Vector3 puntoInicio;
    private bool enPersecucion = false;
    private bool regresandoAInicio = false;

    private void Start()
    {
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }

        animador = GetComponent<Animator>();
        puntoInicio = transform.position;

        CambiarDireccion();
        StartCoroutine(CambiarDireccionAleatoria());
    }

    private void Update()
    {
        if (jugador == null || animador == null) return;

        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);
        float distanciaAlInicio = Vector3.Distance(transform.position, puntoInicio);

        // Verificar si está dentro del rango permitido desde el punto de inicio
        bool fueraDeRango = distanciaAlInicio > distanciaMaxima;

        if (distanciaAlJugador < distanciaPersecucion && !fueraDeRango)
        {
            // Si el jugador está cerca y no estamos fuera de rango, perseguir
            enPersecucion = true;
            regresandoAInicio = false;
            ManejarComportamientoPersecucion(distanciaAlJugador);
        }
        else if (fueraDeRango || regresandoAInicio)
        {
            // Si estamos fuera de rango o ya estábamos regresando, volver al punto inicial
            enPersecucion = false;
            regresandoAInicio = true;
            RegresarAPuntoInicial();
        }
        else
        {
            // Comportamiento normal de patrulla
            enPersecucion = false;
            regresandoAInicio = false;
            ManejarComportamientoPatrulla();
        }
    }

    private void RegresarAPuntoInicial()
    {
        Vector3 direccionAlInicio = (puntoInicio - transform.position).normalized;
        Quaternion rotacionHaciaInicio = Quaternion.LookRotation(direccionAlInicio);
        float distanciaAlInicio = Vector3.Distance(transform.position, puntoInicio);

        // Rotar hacia el punto inicial
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionHaciaInicio, velocidadRotacion * Time.deltaTime);

        float anguloDiferencia = Quaternion.Angle(transform.rotation, rotacionHaciaInicio);

        if (anguloDiferencia > 1f)
        {
            DetenerAnimaciones();
            if (Vector3.Cross(transform.forward, direccionAlInicio).y > 0)
            {
                animador.SetBool("enemigoRotarIzquierda", true);
            }
            else
            {
                animador.SetBool("enemigoRotarDerecha", true);
            }
        }
        else
        {
            DetenerAnimaciones();
            animador.SetBool("enemigoCaminar", true);
            transform.Translate(Vector3.forward * velocidadRetorno * Time.deltaTime);
        }

        // Si ya llegamos cerca del punto inicial, volver al comportamiento normal
        if (distanciaAlInicio < 0.5f)
        {
            regresandoAInicio = false;
            transform.position = puntoInicio;
            DetenerAnimaciones();
            animador.SetBool("enemigoQuieto", true);
        }
    }

    private void ManejarComportamientoPersecucion(float distanciaAlJugador)
    {
        if (distanciaAlJugador <= distanciaDetencion)
        {
            DetenerAnimaciones();
            animador.SetBool("enemigoQuieto", true);
            return;
        }

        Vector3 direccionHaciaJugador = (jugador.position - transform.position).normalized;
        Quaternion rotacionHaciaJugador = Quaternion.LookRotation(direccionHaciaJugador);
        float anguloDiferencia = Quaternion.Angle(transform.rotation, rotacionHaciaJugador);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionHaciaJugador, velocidadRotacion * Time.deltaTime);

        if (anguloDiferencia > 1f)
        {
            DetenerAnimaciones();
            if (Vector3.Cross(transform.forward, direccionHaciaJugador).y > 0)
            {
                animador.SetBool("enemigoRotarIzquierda", true);
            }
            else
            {
                animador.SetBool("enemigoRotarDerecha", true);
            }
        }
        else
        {
            DetenerAnimaciones();
            animador.SetBool("enemigoCorriendo", true);
            transform.Translate(Vector3.forward * velocidadMovimiento * Time.deltaTime);
        }
    }

    private void ManejarComportamientoPatrulla()
    {
        if (enPausa)
        {
            animador.SetBool("enemigoQuieto", true);
            animador.SetBool("enemigoCaminar", false);
            animador.SetBool("enemigoRotarIzquierda", false);
            animador.SetBool("enemigoRotarDerecha", false);
            return;
        }

        Quaternion rotacionActual = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(rotacionActual, rotacionObjetivo, velocidadRotacion * Time.deltaTime);

        float anguloDiferencia = Quaternion.Angle(rotacionActual, rotacionObjetivo);
        rotando = anguloDiferencia > 1f;

        if (rotando)
        {
            if (Vector3.Cross(transform.forward, direccionMovimiento).y > 0)
            {
                animador.SetBool("enemigoRotarIzquierda", true);
                animador.SetBool("enemigoRotarDerecha", false);
            }
            else
            {
                animador.SetBool("enemigoRotarIzquierda", false);
                animador.SetBool("enemigoRotarDerecha", true);
            }
            animador.SetBool("enemigoCaminar", false);
        }
        else
        {
            animador.SetBool("enemigoRotarIzquierda", false);
            animador.SetBool("enemigoRotarDerecha", false);
            animador.SetBool("enemigoCaminar", direccionMovimiento != Vector3.zero);
        }

        if (!rotando && DentroDeLimite())
        {
            transform.Translate(Vector3.forward * velocidadMovimiento * Time.deltaTime);
        }
    }

    private IEnumerator CambiarDireccionAleatoria()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoCambioDireccion);

            if (!enPersecucion && !regresandoAInicio)
            {
                CambiarDireccion();
                enPausa = true;
                animador.SetBool("enemigoQuieto", true);
                yield return new WaitForSeconds(tiempoPausa);
                enPausa = false;
                animador.SetBool("enemigoQuieto", false);
            }
        }
    }

    private void CambiarDireccion()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        direccionMovimiento = new Vector3(x, 0, z).normalized;

        if (direccionMovimiento != Vector3.zero)
        {
            rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
        }
    }

    private bool DentroDeLimite()
    {
        Vector3 distanciaDesdeInicio = transform.position - puntoInicio;
        distanciaDesdeInicio.y = 0;
        return distanciaDesdeInicio.magnitude <= distanciaMaxima;
    }

    private void DetenerAnimaciones()
    {
        animador.SetBool("enemigoQuieto", false);
        animador.SetBool("enemigoCaminar", false);
        animador.SetBool("enemigoRotarIzquierda", false);
        animador.SetBool("enemigoRotarDerecha", false);
        animador.SetBool("enemigoCorriendo", false);
    }
}