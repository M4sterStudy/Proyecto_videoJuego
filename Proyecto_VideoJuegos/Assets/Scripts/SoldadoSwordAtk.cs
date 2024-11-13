using UnityEngine;

public class SoldadoSwordAtk : MonoBehaviour
{
    [Header("Configuraci�n de Distancia")]
    [SerializeField] private float distanciaAtaque = 3f;  // Distancia a la que el enemigo empieza el ataque

    [Header("Configuraci�n de Animaci�n")]
    [SerializeField] private Animator animador;  // Referencia al componente Animator

    private Transform jugador;  // Referencia al jugador
    private bool atacando = false;  // Para saber si estamos en proceso de ataque

    private void Start()
    {
        // Buscar el objeto del jugador en la escena
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }

        if (animador == null)
        {
            animador = GetComponent<Animator>();  // Asigna el animator si no se ha asignado en el inspector
        }
    }

    private void Update()
    {
        if (jugador == null || animador == null) return;

        // Calcular la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= distanciaAtaque)
        {
            // Si estamos dentro del rango de ataque y no hemos comenzado a atacar a�n
            if (!atacando)
            {
                StartCoroutine(RealizarAtaque());
            }
        }
        else
        {
            // Si el jugador se aleja, se termina la animaci�n en curso y se activa la animaci�n de quieto
            if (atacando)
            {
                DetenerAtaque();
            }
        }
    }

    private System.Collections.IEnumerator RealizarAtaque()
    {
        // Desactivar la animaci�n de enemigoQuieto
        animador.SetBool("enemigoQuieto", false);

        // Comienza el ataque
        atacando = true;

        // Fase 1: Sword Attack 1
        animador.SetBool("swordAtack1", true);
        yield return new WaitForSeconds(animador.GetCurrentAnimatorStateInfo(0).length);  // Espera hasta que termine la animaci�n

        // Fase 2: Sword Attack 2
        animador.SetBool("swordAtack1", false);  // Desactiva la primera animaci�n
        animador.SetBool("swordAtack2", true);
        yield return new WaitForSeconds(animador.GetCurrentAnimatorStateInfo(0).length);  // Espera hasta que termine la animaci�n

        // Fase 3: Sword Attack 3
        animador.SetBool("swordAtack2", false);  // Desactiva la segunda animaci�n
        animador.SetBool("swordAtack3", true);
        yield return new WaitForSeconds(animador.GetCurrentAnimatorStateInfo(0).length);  // Espera hasta que termine la animaci�n

        // Al terminar la tercera animaci�n, activamos la animaci�n enemigoQuieto
        animador.SetBool("swordAtack3", false);  // Desactiva la tercera animaci�n
        animador.SetBool("enemigoQuieto", true);  // Activa la animaci�n de enemigoQuieto

        // Marcar que el ataque ha terminado
        atacando = false;
    }

    private void DetenerAtaque()
    {
        // Detener las animaciones de ataque
        animador.SetBool("swordAtack1", false);
        animador.SetBool("swordAtack2", false);
        animador.SetBool("swordAtack3", false);

        // Activar la animaci�n de enemigoQuieto
        animador.SetBool("enemigoQuieto", true);

        // Marcar que el ataque ha terminado
        atacando = false;
    }
}
