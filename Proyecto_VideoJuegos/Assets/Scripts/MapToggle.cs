using UnityEngine;

public class MapToggle : MonoBehaviour
{
    public GameObject[] miniMapObjects;    // Los objetos que forman el minimapa
    public GameObject[] largeMapObjects;   // Los objetos que forman el mapa grande

    private bool isMiniMapActive = false;
    private bool isLargeMapActive = false;

    void Update()
    {
        // Alternar el minimapa con la tecla 'N'
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (!isLargeMapActive) // Solo activar si el mapa grande está desactivado
            {
                isMiniMapActive = !isMiniMapActive;
                foreach (GameObject obj in miniMapObjects)
                {
                    obj.SetActive(isMiniMapActive);
                }
            }
        }

        // Alternar el mapa grande con la tecla 'M'
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isMiniMapActive) // Solo activar si el minimapa está desactivado
            {
                isLargeMapActive = !isLargeMapActive;
                foreach (GameObject obj in largeMapObjects)
                {
                    obj.SetActive(isLargeMapActive);
                }
            }
        }
    }
}
