using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtCamera : MonoBehaviour
{
    // Cámara que se asignará desde el inspector
    public Camera targetCamera;

    void Update()
    {
        if (targetCamera != null)
        {
            // Asegura que el canvas siempre esté orientado hacia la cámara asignada
            transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward,
                             targetCamera.transform.rotation * Vector3.up);
        }
        else
        {
            Debug.LogWarning("No se ha asignado una cámara al CanvasLookAtCamera.");
        }
    }
}
