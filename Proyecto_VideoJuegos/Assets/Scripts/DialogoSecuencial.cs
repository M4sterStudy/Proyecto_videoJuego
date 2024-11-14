using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoSecuencial : MonoBehaviour
{
    [Header("Configuración de Diálogo")]
    [SerializeField] private float rangoActivacion = 5f;   // Rango de activación
    [SerializeField] private GameObject[] dialogos;        // Array de objetos de diálogo
    [SerializeField] private KeyCode teclaDialogo = KeyCode.E; // Tecla para avanzar diálogo

    private Transform jugador;
    private int indiceDialogoActual = -1;  // Índice para controlar el diálogo actual
    private bool dentroDelRango = false;   // Determina si el jugador está en rango

    private void Start()
    {
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }

        // Desactivar todos los diálogos al inicio
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

        // Si el jugador está dentro del rango y no hay diálogo activo, activa el primero
        if (dentroDelRango && indiceDialogoActual == -1)
        {
            ActivarSiguienteDialogo();
        }

        // Si se presiona la tecla de diálogo y el jugador está dentro del rango, avanzar al siguiente diálogo
        if (Input.GetKeyDown(teclaDialogo) && dentroDelRango && indiceDialogoActual >= 0)
        {
            ActivarSiguienteDialogo();
        }
    }

    private void ActivarSiguienteDialogo()
    {
        // Desactivar el diálogo actual, si hay uno
        if (indiceDialogoActual >= 0 && indiceDialogoActual < dialogos.Length)
        {
            dialogos[indiceDialogoActual].SetActive(false);
        }

        // Avanzar al siguiente índice de diálogo
        indiceDialogoActual++;

        // Activar el siguiente diálogo si existe
        if (indiceDialogoActual < dialogos.Length)
        {
            dialogos[indiceDialogoActual].SetActive(true);
        }
        else
        {
            // Si se ha mostrado el último diálogo, resetear o terminar
            indiceDialogoActual = -1; // Para empezar de nuevo cuando el jugador vuelva al rango
        }
    }
}
