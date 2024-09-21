using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound; // Sonido del botón


    public void PlaySound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(buttonSound);
        }
    }
}
