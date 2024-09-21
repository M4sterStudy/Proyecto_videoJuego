using UnityEngine;
using System.Collections.Generic;

public class SceneAudioController : MonoBehaviour
{
    [Header("Listas de GameObjects que contienen m�sica y efectos de sonido")]
    [SerializeField] private List<GameObject> musicGameObjects;  // Lista de GameObjects que contienen m�sica
    [SerializeField] private List<GameObject> sfxGameObjects;    // Lista de GameObjects que contienen efectos de sonido

    private List<AudioClip> sceneMusicClips = new List<AudioClip>(); // Lista de clips de m�sica
    private List<AudioClip> sceneSFXClips = new List<AudioClip>();   // Lista de clips de efectos de sonido

    private void Start()
    {
        // Obtener la lista de clips de m�sica desde los GameObjects designados
        foreach (GameObject musicObject in musicGameObjects)
        {
            AudioSource audioSource = musicObject.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip != null)
            {
                sceneMusicClips.Add(audioSource.clip); // Agregar el clip de m�sica a la lista
            }
        }

        // Obtener la lista de clips de efectos de sonido desde los GameObjects designados
        foreach (GameObject sfxObject in sfxGameObjects)
        {
            AudioSource audioSource = sfxObject.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip != null)
            {
                sceneSFXClips.Add(audioSource.clip); // Agregar el clip de efecto de sonido a la lista
            }
        }

        // Reproducir la primera pista de m�sica autom�ticamente, si hay alguna disponible
        if (sceneMusicClips.Count > 0)
        {
            AudioManager.instance.PlayMusic(sceneMusicClips[0]); // Reproducir la primera pista de m�sica
        }
    }

    // M�todo para reproducir una pista de m�sica espec�fica seg�n el �ndice
    public void PlaySceneMusic(int index)
    {
        if (index >= 0 && index < sceneMusicClips.Count)
        {
            AudioManager.instance.PlayMusic(sceneMusicClips[index]);
        }
        else
        {
            Debug.LogWarning("�ndice de m�sica fuera de rango.");
        }
    }

    // M�todo para reproducir un efecto de sonido espec�fico seg�n el �ndice
    public void PlaySceneSFX(int index)
    {
        if (index >= 0 && index < sceneSFXClips.Count)
        {
            AudioManager.instance.PlaySFX(sceneSFXClips[index]);
        }
        else
        {
            Debug.LogWarning("�ndice de efectos de sonido fuera de rango.");
        }
    }
}
