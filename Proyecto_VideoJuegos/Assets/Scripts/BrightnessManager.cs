using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BrightnessManager : MonoBehaviour
{
    private Slider brightnessSlider;
    private Image panelBrillo;

    private static BrightnessManager instance;

    private const float maxOpacity = 1f;
    private const float minOpacity = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupBrightnessControl();
    }

    private void SetupBrightnessControl()
    {
        // Buscar el slider y el panel por tag, incluyendo objetos inactivos
        brightnessSlider = FindObjectByTag<Slider>("SliderBrillo");
        panelBrillo = FindObjectByTag<Image>("PanelBrillo");

        if (brightnessSlider == null)
        {
            Debug.LogWarning("No se encontró el objeto con tag SliderBrillo en la escena. El control de brillo no estará disponible.");
            return;
        }

        if (panelBrillo == null)
        {
            Debug.LogWarning("No se encontró el objeto con tag PanelBrillo en la escena. El efecto de brillo no será visible.");
            return;
        }

        // Asegurarse de que los GameObjects del slider y panel estén activos
        brightnessSlider.gameObject.SetActive(true);
        panelBrillo.gameObject.SetActive(true);

        brightnessSlider.minValue = 40;
        brightnessSlider.maxValue = 200;

        brightnessSlider.value = PlayerPrefs.GetFloat("brillo", 100f);

        AdjustBrightness(brightnessSlider.value);

        brightnessSlider.onValueChanged.AddListener(ChangeSlider);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupBrightnessControl();
    }

    // Método genérico para encontrar objetos por tag, incluyendo objetos inactivos
    private T FindObjectByTag<T>(string tag) where T : Component
    {
        T[] allObjects = Resources.FindObjectsOfTypeAll<T>();
        foreach (T obj in allObjects)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }
        return null;
    }

    public void ChangeSlider(float valor)
    {
        PlayerPrefs.SetFloat("brillo", valor);
        AdjustBrightness(valor);
    }

    private void AdjustBrightness(float value)
    {
        if (panelBrillo != null)
        {
            float normalizedValue = Mathf.Clamp(value / 200f, minOpacity, maxOpacity);
            Color panelColor = panelBrillo.color;
            panelColor.a = 1f - normalizedValue;
            panelBrillo.color = panelColor;
        }
        else
        {
            Debug.LogWarning("PanelBrillo no está disponible. No se puede ajustar el brillo visualmente.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}