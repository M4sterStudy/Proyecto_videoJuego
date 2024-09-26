using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class camara : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    private Player inputActions;

    private void Awake()
    {
        inputActions = new Player();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.personaje.camara.performed += OnLookPerformed;
        inputActions.personaje.camara.canceled += OnLookCanceled;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();

        // Pasar el input directamente a Cinemachine
        freeLookCam.m_XAxis.m_InputAxisValue = lookInput.x; // Eje horizontal
        freeLookCam.m_YAxis.m_InputAxisValue = lookInput.y; // Eje vertical
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        // Detener la rotación de la cámara cuando se cancela el input
        freeLookCam.m_XAxis.m_InputAxisValue = 0;
        freeLookCam.m_YAxis.m_InputAxisValue = 0;
    }
}