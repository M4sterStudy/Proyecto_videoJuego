using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuPausa : MonoBehaviour
{
    // Referencia al objeto de menú de pausa
    [SerializeField] private GameObject menuPausaUI;

    [SerializeField] private GameObject menuPausaPanel;

    // Lista de otros paneles/interfaz que deberían desactivarse cuando se vuelve al menú de pausa
    [SerializeField] private List<GameObject> otrosPanelesUI;

    // Lista de escenas en las que el menú de pausa no aparecerá
    [SerializeField] private List<string> escenasExcluidas;

    // Objeto que contiene el AudioSource
    [SerializeField] private GameObject soundObject;

    private AudioSource toggleSound;

    private bool menuActivo = false;

    private void Start()
    {
        // Restablecer Time.timeScale a 1 en cada carga de escena
        Time.timeScale = 1f;

        // Verificar si la escena actual está en la lista de exclusión
        string escenaActual = SceneManager.GetActiveScene().name;
        if (escenasExcluidas.Contains(escenaActual))
        {
            // Si la escena actual está en la lista, desactivar el menú de pausa
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

    // Método para activar/desactivar el menú de pausa
    public void ToggleMenu()
    {
        menuActivo = !menuActivo;
        menuPausaUI.SetActive(menuActivo);
        ActivarMenuPrincipal();

        // Reproducir el sonido de activación/desactivación si el objeto y el AudioSource están asignados
        if (toggleSound != null)
        {
            toggleSound.Play();
        }

        // Pausar el juego si el menú está activo, excepto este objeto
        if (menuActivo)
        {
            Time.timeScale = 0f; // Pausar todo el juego
        }
        else
        {
            Time.timeScale = 1f; // Reanudar el juego
        }
    }

    // Método para activar la interfaz del menú principal y desactivar los demás paneles
    public void ActivarMenuPrincipal()
    {
        // Activar el menú principal de pausa
        menuPausaPanel.SetActive(true);

        // Desactivar todos los demás paneles que podrían haber quedado activos
        foreach (GameObject panel in otrosPanelesUI)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }
}
