using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Puedes usar esta variable si deseas personalizar mensajes u otras acciones al cerrar el juego.
    [SerializeField] private string exitMessage = "Cerrando el juego...";

    // Método para cerrar el juego
    public void ExitApplication()
    {
        // Imprime un mensaje en la consola cuando se cierra el juego (opcional).
        Debug.Log(exitMessage);

        // Si estamos en el editor de Unity, esto no funcionará, pero al compilarlo como una aplicación, cerrará el programa.
        Application.Quit();

    }
}
