using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPrueba : MonoBehaviour
{
    public Transform objetivo;
    public float sensibilidadHorizontal = 2.0f;
    public float sensibilidadVertical = 2.0f;
    public float alturaMinima = -20.0f;
    public float alturaMaxima = 80.0f;

    private float rotacionVertical = 0;

    void Start()
    {
        // No bloqueamos el cursor en el inicio
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // Eliminamos el bloqueo del cursor al centro de la pantalla al hacer clic
        // if (Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     Cursor.lockState = CursorLockMode.Locked;
        // }

        float movimientoHorizontal = Input.GetAxis("Mouse X") * sensibilidadHorizontal;
        float movimientoVertical = Input.GetAxis("Mouse Y") * sensibilidadVertical;

        objetivo.Rotate(0, movimientoHorizontal, 0);

        rotacionVertical -= movimientoVertical;
        rotacionVertical = Mathf.Clamp(rotacionVertical, alturaMinima, alturaMaxima);
        transform.localRotation = Quaternion.Euler(rotacionVertical, 0, 0);
    }
}