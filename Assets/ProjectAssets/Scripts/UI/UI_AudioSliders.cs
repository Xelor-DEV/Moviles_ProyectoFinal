using UnityEngine;
using UnityEngine.UI;

public class UI_AudioSliders : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("References")]
    [SerializeField] private AudioConfig audioConfig;

    private void OnEnable()
    {
        SaveManager.OnSaveDataLoaded += LoadAudioSettings;
    }

    private void OnDisable()
    {
        SaveManager.OnSaveDataLoaded -= LoadAudioSettings;
    }

    void Start()
    {
        LoadAudioSettings();
    }

    public void LoadAudioSettings()
    {
        if (audioConfig == null) return;

        masterSlider.value = audioConfig.MasterVolume;
        musicSlider.value = audioConfig.MusicVolume;
        sfxSlider.value = audioConfig.SfxVolume;
    }

    public void SetMasterVolume(float volume)
    {
        if (audioConfig != null)
        {
            audioConfig.MasterVolume = volume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (audioConfig != null)
        {
            audioConfig.MusicVolume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (audioConfig != null)
        {
            audioConfig.SfxVolume = volume;
        }
    }
}