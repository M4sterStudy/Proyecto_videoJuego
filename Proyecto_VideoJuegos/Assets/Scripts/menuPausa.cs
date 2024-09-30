using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuPausa : MonoBehaviour
{
    // Referencia al objeto de men� de pausa
    [SerializeField] private GameObject menuPausaUI;

    // Lista de escenas en las que el men� de pausa no aparecer�
    [SerializeField] private List<string> escenasExcluidas;

    // Objeto que contiene el AudioSource
    [SerializeField] private GameObject soundObject;

    private AudioSource toggleSound;

    private bool menuActivo = false;

    private void Start()
    {
        // Verificar si la escena actual est� en la lista de exclusi�n
        string escenaActual = SceneManager.GetActiveScene().name;
        if (escenasExcluidas.Contains(escenaActual))
        {
            // Si la escena actual est� en la lista, desactivar el men� de pausa
            gameObject.SetActive(false);
        }

        // Obtener el AudioSource del objeto asignado
        if (soundObject != null)
        {
            toggleSound = soundObject.GetComponent<AudioSource>();
            if (toggleSound == null)
            {
                Debug.LogWarning("El objeto de sonido no tiene un componente AudioSource.");
            }
        }
        else
        {
            Debug.LogWarning("El objeto de sonido no ha sido asignado.");
        }
    }

    private void Update()
    {
        // Detectar si se presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    // M�todo para activar/desactivar el men� de pausa
    private void ToggleMenu()
    {
        menuActivo = !menuActivo;
        menuPausaUI.SetActive(menuActivo);

        // Reproducir el sonido de activaci�n/desactivaci�n si el objeto y el AudioSource est�n asignados
        if (toggleSound != null)
        {
            toggleSound.Play();
        }

        // Pausar el juego si el men� est� activo, excepto este objeto
        if (menuActivo)
        {
            Time.timeScale = 0f; // Pausar todo el juego
        }
        else
        {
            Time.timeScale = 1f; // Reanudar el juego
        }
    }
}
