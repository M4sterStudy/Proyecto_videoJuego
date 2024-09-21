using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider brightnessSlider;

    // Referencia a la cámara
    private Camera mainCamera;

    private void Start()
    {
        // Obtener la cámara principal
        mainCamera = Camera.main;

        if(mainCamera == null)
        {
            Debug.Log("no hay camara");
        }

        // Cargar el brillo guardado o establecer un valor por defecto
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1f);
        brightnessSlider.value = savedBrightness;
        SetBrightness(savedBrightness);

        // Asignar el evento del slider
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
    }

    public void SetBrightness(float value)
    {
        // Ajustar el color de fondo de la cámara
        Color newColor = new Color(value, value, value, 1f);
        mainCamera.backgroundColor = newColor;

        // Ajustar el brillo de todos los elementos UI en el Canvas
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            foreach (Graphic graphic in canvas.GetComponentsInChildren<Graphic>())
            {
                graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, value);
                Debug.Log($"Ajustado el brillo de {graphic.name} a {value}"); // Mensaje de depuración
            }
        }

        // Guarda el brillo
        PlayerPrefs.SetFloat("Brightness", value);
    }
}
