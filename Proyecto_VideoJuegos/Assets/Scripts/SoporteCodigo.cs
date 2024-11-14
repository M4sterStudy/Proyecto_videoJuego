using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoporteCodigo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidadCaminata = 2f;
    [SerializeField] private float velocidadCorrer = 5f;
    [SerializeField] private float distanciaCaminata = 10f;
    [SerializeField] private float distanciaMaxima = 2f;
    [SerializeField] private float velocidadRotacion = 120f;
    [SerializeField] private float umbralAlineacion = 5f;

    [Header("Configuración de Detección y Combate")]
    [SerializeField] private float rangoDeteccionEnemigos = 15f;
    [SerializeField] private float cooldownDisparo = 2f;
    [SerializeField] private GameObject flechaPrefab;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private TextMeshProUGUI textoDeteccion;
    [SerializeField] private float velocidadFlecha = 20f;
    [SerializeField] private LayerMask capaEnemigos;
    [SerializeField] private float tiempoGiroCompleto = 0.5f;  // Tiempo para completar el giro hacia el enemigo

    private Transform jugador;
    private Animator animador;
    private Transform enemigoActual;
    private bool puedeDisparar = false;  // Cambiado a false por defecto
    private bool estaDisparando = false;
    private float tiempoUltimoDisparo;
    private bool estaGirandoHaciaEnemigo = false;
    private bool enemigoDetectadoYAlineado = false;
    private Coroutine giroActual;

    private void Start()
    {
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
        if (textoDeteccion != null)
        {
            textoDeteccion.text = "";
        }
    }

    private void Update()
    {
        if (jugador == null) return;

        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);
        bool deberiaAtenderJugador = distanciaAlJugador > distanciaMaxima;

        // Si el jugador se aleja demasiado, cancelar cualquier acción con enemigos
        if (deberiaAtenderJugador && (estaGirandoHaciaEnemigo || enemigoDetectadoYAlineado))
        {
            CancelarAccionesEnemigo();
        }

        if (!estaDisparando && !estaGirandoHaciaEnemigo)
        {
            if (deberiaAtenderJugador)
            {
                // Comportamiento normal de seguimiento al jugador
                bool estaAlineado = RotarHaciaJugador();
                if (estaAlineado)
                {
                    if (distanciaAlJugador <= distanciaCaminata)
                    {
                        SeguirJugador(velocidadCaminata, "caminar");
                    }
                    else
                    {
                        SeguirJugador(velocidadCorrer, "correr");
                    }
                }
            }
            else
            {
                Detenerse();
                BuscarEnemigoCercano();
            }
        }

        // Sistema de disparo
        if (enemigoDetectadoYAlineado && puedeDisparar && !deberiaAtenderJugador)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(RealizarDisparo());
            }
        }
    }

    private void CancelarAccionesEnemigo()
    {
        if (giroActual != null)
        {
            StopCoroutine(giroActual);
            giroActual = null;
        }
        estaGirandoHaciaEnemigo = false;
        enemigoDetectadoYAlineado = false;
        puedeDisparar = false;
        if (textoDeteccion != null)
        {
            textoDeteccion.text = "";
        }
    }

    private void BuscarEnemigoCercano()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        Transform enemigoMasCercano = null;
        float distanciaMasCorta = rangoDeteccionEnemigos;

        foreach (GameObject enemigo in enemigos)
        {
            float distancia = Vector3.Distance(transform.position, enemigo.transform.position);
            if (distancia < distanciaMasCorta)
            {
                distanciaMasCorta = distancia;
                enemigoMasCercano = enemigo.transform;
            }
        }

        // Si encontramos un nuevo enemigo y no estamos girando ya
        if (enemigoMasCercano != enemigoActual && !estaGirandoHaciaEnemigo)
        {
            enemigoActual = enemigoMasCercano;
            if (enemigoActual != null)
            {
                estaGirandoHaciaEnemigo = true;
                enemigoDetectadoYAlineado = false;
                puedeDisparar = false;
                if (giroActual != null)
                {
                    StopCoroutine(giroActual);
                }
                giroActual = StartCoroutine(GirarHaciaEnemigo());
            }
            else
            {
                CancelarAccionesEnemigo();
            }
        }
    }

    private IEnumerator GirarHaciaEnemigo()
    {
        if (enemigoActual == null)
        {
            estaGirandoHaciaEnemigo = false;
            yield break;
        }

        Vector3 direccionHaciaEnemigo = (enemigoActual.position - transform.position).normalized;
        Quaternion rotacionInicial = transform.rotation;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(
            new Vector3(direccionHaciaEnemigo.x, 0, direccionHaciaEnemigo.z)
        );

        float tiempoTranscurrido = 0f;
        bool direccionGiroestablecida = false;
        bool girarIzquierda = false;

        // Determinar la dirección del giro al inicio
        if (!direccionGiroestablecida)
        {
            girarIzquierda = Vector3.Cross(transform.forward, direccionHaciaEnemigo).y > 0;
            direccionGiroestablecida = true;
        }

        // Activar la animación correspondiente
        if (girarIzquierda)
        {
            ActivarAnimacion("girarIzquierda");
        }
        else
        {
            ActivarAnimacion("girarDerecha");
        }

        while (tiempoTranscurrido < tiempoGiroCompleto)
        {
            if (enemigoActual == null)
            {
                estaGirandoHaciaEnemigo = false;
                yield break;
            }

            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / tiempoGiroCompleto;
            transform.rotation = Quaternion.Lerp(rotacionInicial, rotacionObjetivo, t);
            yield return null;
        }

        // Completar la rotación
        transform.rotation = rotacionObjetivo;

        // Activar estado de detección completa
        estaGirandoHaciaEnemigo = false;
        enemigoDetectadoYAlineado = true;
        puedeDisparar = true;

        // Mostrar mensaje de detección
        if (textoDeteccion != null)
        {
            textoDeteccion.text = "¡Enemigo detectado!\nPresiona F para disparar";
        }

        // Volver a la animación de quieto
        ActivarAnimacion("quieto");
    }

    private IEnumerator RealizarDisparo()
    {
        if (Time.time - tiempoUltimoDisparo < cooldownDisparo) yield break;

        estaDisparando = true;
        puedeDisparar = false;
        tiempoUltimoDisparo = Time.time;

        // Activar animación de disparo
        animador.SetBool("disparo", true);

        // Esperar antes de disparar la flecha
        yield return new WaitForSeconds(0.5f);

        // Disparar la flecha
        if (flechaPrefab != null && puntoDisparo != null && enemigoActual != null)
        {
            GameObject flecha = Instantiate(flechaPrefab, puntoDisparo.position, puntoDisparo.rotation);
            Vector3 direccionDisparo = (enemigoActual.position - puntoDisparo.position).normalized;
            Rigidbody rbFlecha = flecha.GetComponent<Rigidbody>();

            if (rbFlecha != null)
            {
                rbFlecha.velocity = direccionDisparo * velocidadFlecha;
            }
        }

        // Esperar el resto del cooldown
        yield return new WaitForSeconds(1.5f);

        // Resetear estados
        animador.SetBool("disparo", false);
        estaDisparando = false;
        puedeDisparar = true;
    }

    private bool RotarHaciaJugador()
    {
        if (estaGirandoHaciaEnemigo || enemigoDetectadoYAlineado) return false;

        Vector3 direccionHaciaJugador = (jugador.position - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(new Vector3(direccionHaciaJugador.x, 0, direccionHaciaJugador.z));
        float anguloDiferencia = Quaternion.Angle(transform.rotation, rotacionObjetivo);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);

        if (anguloDiferencia > umbralAlineacion)
        {
            if (Vector3.Cross(transform.forward, direccionHaciaJugador).y > 0)
            {
                ActivarAnimacion("girarIzquierda");
            }
            else
            {
                ActivarAnimacion("girarDerecha");
            }
            return false;
        }
        return true;
    }

    private void SeguirJugador(float velocidad, string accionAnimacion)
    {
        Vector3 direccionHaciaJugador = (jugador.position - transform.position).normalized;
        direccionHaciaJugador.y = 0;
        transform.Translate(direccionHaciaJugador * velocidad * Time.deltaTime, Space.World);
        ActivarAnimacion(accionAnimacion);
    }

    private void Detenerse()
    {
        ActivarAnimacion("quieto");
    }

    private void ActivarAnimacion(string nombreAnimacion)
    {
        if (estaDisparando) return;

        animador.SetBool("quieto", false);
        animador.SetBool("caminar", false);
        animador.SetBool("correr", false);
        animador.SetBool("girarIzquierda", false);
        animador.SetBool("girarDerecha", false);
        animador.SetBool("disparo", false);

        animador.SetBool(nombreAnimacion, true);
    }
}