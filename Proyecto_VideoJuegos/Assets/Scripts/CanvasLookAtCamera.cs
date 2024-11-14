using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtCamera : MonoBehaviour
{
    // C�mara que se asignar� desde el inspector
    public Camera targetCamera;

    void Update()
    {
        if (targetCamera != null)
        {
            // Asegura que el canvas siempre est� orientado hacia la c�mara asignada
            transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward,
                             targetCamera.transform.rotation * Vector3.up);
        }
        else
        {
            Debug.LogWarning("No se ha asignado una c�mara al CanvasLookAtCamera.");
        }
    }
}
