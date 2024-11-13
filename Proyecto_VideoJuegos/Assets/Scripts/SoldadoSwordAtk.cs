using UnityEngine;

public class SoldadoSwordAtk : MonoBehaviour
{
    [Header("Configuración de Distancia")]
    [SerializeField] private float distanciaAtaque = 3f;  // Distancia a la que el enemigo empieza el ataque

    [Header("Configuración de Animación")]
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
            // Si estamos dentro del rango de ataque y no hemos comenzado a atacar aún
            if (!atacando)
            {
                StartCoroutine(RealizarAtaque());
            }
        }
        else
        {
            // Si el jugador se aleja, se termina la animación en curso y se activa la animación de quieto
            if (atacando)
            {
                DetenerAtaque();
            }
        }
    }

    private System.Collections.IEnumerator RealizarAtaque()
    {
        // Desactivar la animación de enemigoQuieto
        animador.SetBool("enemigoQuieto", false);

        // Comienza el ataque
        atacando = true;

        // Fase 1: Sword Attack 1
        animador.SetBool("swordAtack1", true);
        yield return new WaitForSeconds(animador.GetCurrentAnimatorStateInfo(0).length);  // Espera hasta que termine la animación

        // Fase 2: Sword Attack 2
        animador.SetBool("swordAtack1", false);  // Desactiva la primera animación
        animador.SetBool("swordAtack2", true);
        yield return new WaitForSeconds(animador.GetCurrentAnimatorStateInfo(0).length);  // Espera hasta que termine la animación

        // Fase 3: Sword Attack 3
        animador.SetBool("swordAtack2", false);  // Desactiva la segunda animación
        animador.SetBool("swordAtack3", true);
        yield return new WaitForSeconds(animador.GetCurrentAnimatorStateInfo(0).length);  // Espera hasta que termine la animación

        // Al terminar la tercera animación, activamos la animación enemigoQuieto
        animador.SetBool("swordAtack3", false);  // Desactiva la tercera animación
        animador.SetBool("enemigoQuieto", true);  // Activa la animación de enemigoQuieto

        // Marcar que el ataque ha terminado
        atacando = false;
    }

    private void DetenerAtaque()
    {
        // Detener las animaciones de ataque
        animador.SetBool("swordAtack1", false);
        animador.SetBool("swordAtack2", false);
        animador.SetBool("swordAtack3", false);

        // Activar la animación de enemigoQuieto
        animador.SetBool("enemigoQuieto", true);

        // Marcar que el ataque ha terminado
        atacando = false;
    }
}
