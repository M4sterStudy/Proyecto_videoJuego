using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Puedes usar esta variable si deseas personalizar mensajes u otras acciones al cerrar el juego.
    [SerializeField] private string exitMessage = "Cerrando el juego...";

    // M�todo para cerrar el juego
    public void ExitApplication()
    {
        // Imprime un mensaje en la consola cuando se cierra el juego (opcional).
        Debug.Log(exitMessage);

        // Si estamos en el editor de Unity, esto no funcionar�, pero al compilarlo como una aplicaci�n, cerrar� el programa.
        Application.Quit();

    }
}
