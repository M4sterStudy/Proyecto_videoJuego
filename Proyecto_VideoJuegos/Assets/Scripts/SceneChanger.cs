using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Declaramos la variable de la escena de forma privada, pero serializable para que sea visible en el Inspector.
    [SerializeField] private string sceneName;

    // Método para cambiar de escena
    public void ChangeScene()
    {
        // Cambia a la escena especificada en el Inspector
        SceneManager.LoadScene(sceneName);
    }
}
