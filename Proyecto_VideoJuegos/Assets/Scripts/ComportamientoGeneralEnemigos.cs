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
    public bool haExplotado = false;  // Indicador de explosión
    [SerializeField] private float velocidadRetorno = 3f;

    private Transform jugador;
    private Animator animador;
    private Vector3 direccionMovimiento;
    private float rotacionObjetivoY;
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

        bool fueraDeRango = distanciaAlInicio > distanciaMaxima;

        if (distanciaAlJugador < distanciaPersecucion && !fueraDeRango)
        {
            enPersecucion = true;
            regresandoAInicio = false;
            ManejarComportamientoPersecucion(distanciaAlJugador);
        }
        else if (fueraDeRango || regresandoAInicio)
        {
            enPersecucion = false;
            regresandoAInicio = true;
            RegresarAPuntoInicial();
        }
        else
        {
            enPersecucion = false;
            regresandoAInicio = false;
            ManejarComportamientoPatrulla();
        }
    }

    private void RegresarAPuntoInicial()
    {
        Vector3 direccionAlInicio = (puntoInicio - transform.position).normalized;
        float anguloObjetivo = Mathf.Atan2(direccionAlInicio.x, direccionAlInicio.z) * Mathf.Rad2Deg;
        float distanciaAlInicio = Vector3.Distance(transform.position, puntoInicio);

        // Rotar solo en el eje Y
        float rotacionActual = transform.eulerAngles.y;
        float nuevaRotacionY = Mathf.MoveTowardsAngle(rotacionActual, anguloObjetivo, velocidadRotacion * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, nuevaRotacionY, 0);

        float anguloDiferencia = Mathf.Abs(Mathf.DeltaAngle(rotacionActual, anguloObjetivo));

        if (anguloDiferencia > 1f)
        {
            DetenerAnimaciones();
            if (Mathf.DeltaAngle(rotacionActual, anguloObjetivo) > 0)
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
            //animador.SetBool("enemigoQuieto", true);
            return;
        }

        Vector3 direccionHaciaJugador = (jugador.position - transform.position).normalized;
        float anguloObjetivo = Mathf.Atan2(direccionHaciaJugador.x, direccionHaciaJugador.z) * Mathf.Rad2Deg;
        float rotacionActual = transform.eulerAngles.y;
        float nuevaRotacionY = Mathf.MoveTowardsAngle(rotacionActual, anguloObjetivo, velocidadRotacion * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, nuevaRotacionY, 0);
        float anguloDiferencia = Mathf.Abs(Mathf.DeltaAngle(rotacionActual, anguloObjetivo));

        if (anguloDiferencia > 1f)
        {
            DetenerAnimaciones();
            if (Mathf.DeltaAngle(rotacionActual, anguloObjetivo) > 0)
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

    private bool DentroDeLimite()
    {
        // Calcula la distancia entre la posición actual y el punto de inicio
        float distanciaAlInicio = Vector3.Distance(transform.position, puntoInicio);

        // Verifica si la siguiente posición estaría dentro del límite
        Vector3 siguientePosicion = transform.position + transform.forward * velocidadMovimiento * Time.deltaTime;
        float distanciaSiguiente = Vector3.Distance(siguientePosicion, puntoInicio);

        // Retorna verdadero si la siguiente posición está dentro del límite establecido
        return distanciaSiguiente <= distanciaMaxima;
    }

    private void ManejarComportamientoPatrulla()
    {
        if (haExplotado) return;  // Si ha explotado, no hacer nada

        if (enPausa)
        {
            DetenerAnimaciones();
            animador.SetBool("enemigoQuieto", true);
            return;
        }

        // El resto de la lógica sigue igual
        float rotacionActual = transform.eulerAngles.y;
        float nuevaRotacionY = Mathf.MoveTowardsAngle(rotacionActual, rotacionObjetivoY, velocidadRotacion * Time.deltaTime);

        // Calcular la diferencia de ángulo antes de aplicar la rotación
        float anguloDiferencia = Mathf.Abs(Mathf.DeltaAngle(rotacionActual, rotacionObjetivoY));

        // Actualizar la rotación
        transform.rotation = Quaternion.Euler(0, nuevaRotacionY, 0);

        // Si necesita rotar más de 10 grados, priorizar la rotación
        if (anguloDiferencia > 10f)
        {
            DetenerAnimaciones();
            if (Mathf.DeltaAngle(rotacionActual, rotacionObjetivoY) > 0)
            {
                animador.SetBool("enemigoRotarIzquierda", true);
            }
            else
            {
                animador.SetBool("enemigoRotarDerecha", true);
            }
        }
        // Si la diferencia es menor a 10 grados, puede caminar mientras ajusta la rotación
        else
        {
            DetenerAnimaciones();
            if (direccionMovimiento != Vector3.zero && DentroDeLimite())
            {
                animador.SetBool("enemigoCaminar", true);
                transform.Translate(Vector3.forward * velocidadMovimiento * Time.deltaTime);
            }
            else
            {
                animador.SetBool("enemigoQuieto", true);
            }
        }
    }

    private void CambiarDireccion()
    {
        float anguloAleatorio = Random.Range(0f, 360f);
        rotacionObjetivoY = anguloAleatorio;

        // Calcular la dirección de movimiento basada en el ángulo
        float radianes = anguloAleatorio * Mathf.Deg2Rad;
        direccionMovimiento = new Vector3(Mathf.Sin(radianes), 0, Mathf.Cos(radianes)).normalized;
    }

    private IEnumerator CambiarDireccionAleatoria()
    {
        while (true)
        {
            if (!enPersecucion && !regresandoAInicio)
            {
                // Primero hacer una pausa
                DetenerAnimaciones();
                enPausa = true;
                animador.SetBool("enemigoQuieto", true);
                yield return new WaitForSeconds(tiempoPausa);

                // Luego cambiar dirección
                CambiarDireccion();
                enPausa = false;

                // Esperar el tiempo de movimiento
                yield return new WaitForSeconds(tiempoCambioDireccion);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
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