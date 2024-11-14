using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cofres : MonoBehaviour
{
    public GameObject aviso;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aviso.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aviso.SetActive(false);
        }
        
    }
}
