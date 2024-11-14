using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoSecuencial : MonoBehaviour
{
    [Header("Configuraci�n de Di�logo")]
    [SerializeField] private float rangoActivacion = 5f;   // Rango de activaci�n
    [SerializeField] private GameObject[] dialogos;        // Array de objetos de di�logo
    [SerializeField] private KeyCode teclaDialogo = KeyCode.E; // Tecla para avanzar di�logo

    private Transform jugador;
    private int indiceDialogoActual = -1;  // �ndice para controlar el di�logo actual
    private bool dentroDelRango = false;   // Determina si el jugador est� en rango

    private void Start()
    {
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }

        // Desactivar todos los di�logos al inicio
        foreach (GameObject dialogo in dialogos)
        {
            dialogo.SetActive(false);
        }
    }

    private void Update()
    {
        if (jugador == null) return;

        // Comprobar la distancia con el jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);
        dentroDelRango = distanciaAlJugador <= rangoActivacion;

        // Si el jugador est� dentro del rango y no hay di�logo activo, activa el primero
        if (dentroDelRango && indiceDialogoActual == -1)
        {
            ActivarSiguienteDialogo();
        }

        // Si se presiona la tecla de di�logo y el jugador est� dentro del rango, avanzar al siguiente di�logo
        if (Input.GetKeyDown(teclaDialogo) && dentroDelRango && indiceDialogoActual >= 0)
        {
            ActivarSiguienteDialogo();
        }
    }

    private void ActivarSiguienteDialogo()
    {
        // Desactivar el di�logo actual, si hay uno
        if (indiceDialogoActual >= 0 && indiceDialogoActual < dialogos.Length)
        {
            dialogos[indiceDialogoActual].SetActive(false);
        }

        // Avanzar al siguiente �ndice de di�logo
        indiceDialogoActual++;

        // Activar el siguiente di�logo si existe
        if (indiceDialogoActual < dialogos.Length)
        {
            dialogos[indiceDialogoActual].SetActive(true);
        }
        else
        {
            // Si se ha mostrado el �ltimo di�logo, resetear o terminar
            indiceDialogoActual = -1; // Para empezar de nuevo cuando el jugador vuelva al rango
        }
    }
}
