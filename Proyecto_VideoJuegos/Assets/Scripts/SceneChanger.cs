using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Declaramos la variable de la escena de forma privada, pero serializable para que sea visible en el Inspector.
    [SerializeField] private string sceneName;

    // Método para cambiar de escena con un retraso
    public void ChangeScene()
    {
        // Inicia la corutina que espera antes de cambiar de escena
        StartCoroutine(ChangeSceneWithDelay());
    }

    // Corutina que espera 0.30 segundos antes de cambiar de escena
    private IEnumerator ChangeSceneWithDelay()
    {
        // Espera 0.30 segundos
        yield return new WaitForSeconds(0f);

        // Cambia a la escena especificada en el Inspector
        SceneManager.LoadScene(sceneName);
    }
}
