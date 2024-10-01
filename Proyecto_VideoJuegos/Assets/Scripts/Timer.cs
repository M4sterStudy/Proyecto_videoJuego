using UnityEngine;
using TMPro; // Para usar TextMeshPro

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Arrastra tu TextMeshPro UI aqu� desde el Inspector

    private float elapsedTime = 0f;   // Tiempo transcurrido
    private bool isRunning = false;   // Estado del temporizador

    private void Start()
    {
        StartTimer();  // Iniciar el temporizador autom�ticamente (opcional)
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime; // Sumar el tiempo transcurrido
            UpdateTimerDisplay(elapsedTime); // Actualizar el texto del temporizador
        }
    }

    // M�todo para iniciar el temporizador
    public void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f; // Reiniciar el tiempo si es necesario
    }

    // M�todo para detener el temporizador
    public void StopTimer()
    {
        isRunning = false;
    }

    // M�todo para reiniciar el temporizador
    public void ResetTimer()
    {
        isRunning = false;
        elapsedTime = 0f;
        UpdateTimerDisplay(elapsedTime);
    }

    // Actualizar la visualizaci�n del temporizador en el TextMeshPro
    private void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);  // Obtener los minutos
        int seconds = Mathf.FloorToInt(time % 60F);  // Obtener los segundos
        int milliseconds = Mathf.FloorToInt((time * 1000F) % 1000); // Obtener los milisegundos si lo necesitas

        // Mostrar el tiempo en formato MM:SS
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
