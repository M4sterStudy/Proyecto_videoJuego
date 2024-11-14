using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cofres : MonoBehaviour
{
    public GameObject aviso;
    public ParticleSystem particulas; // Referencia al sistema de partículas

    private bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aviso.SetActive(true);
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aviso.SetActive(false);
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        // Solo activar si el jugador está en rango y se presiona la tecla 'E'
        if (isPlayerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Reproducir el sistema de partículas
            if (particulas != null)
            {
                particulas.Play();
            }
            else
            {
                Debug.LogWarning("No hay un sistema de partículas asignado al cofre.");
            }

            // Ocultar el aviso después de abrir el cofre
            aviso.SetActive(false);

            // Opcional: desactivar el cofre o realizar otras acciones
            // gameObject.SetActive(false); // Desactiva el cofre si quieres que solo se pueda abrir una vez
        }
    }
}