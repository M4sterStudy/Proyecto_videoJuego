using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio")]
    // AudioSource para m�sica y efectos de sonido
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Sliders de Volumen")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Awake()
    {


        // Implementaci�n del Singleton para que solo exista un AudioManager en todo el juego.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste a lo largo de las escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Cargar el volumen guardado al inicio de los efectos
        //float savedVolumefx = PlayerPrefs.GetFloat("SFXVolume", 1f);
        //SetSFXVolume(savedVolumefx); // Aplicar el volumen guardado

        // Cargar el volumen guardado al inicio de los sonidos
        //float savedVolumeMusic = PlayerPrefs.GetFloat("musicVolume", 1f);
        //SetMusicVolume(savedVolumeMusic); // Aplicar el volumen guardado

        // Configurar los AudioSources
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true; // La m�sica siempre se repetir�
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Asignar eventos a los sliders para ajustar el volumen
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // M�todo para reproducir la m�sica
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioClip de m�sica no asignado.");
        }
    }

    // M�todo para reproducir efectos de sonido
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxClip != null)
        {
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f); // Ajusta el volumen seg�n lo guardado
            sfxSource.PlayOneShot(sfxClip);
        }
        else
        {
            Debug.LogWarning("AudioClip de efectos de sonido no asignado.");
        }
    }




    // M�todos para controlar el volumen
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume); // Guardar el volumen
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;

        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume); // Guardar el volumen
    }
}
