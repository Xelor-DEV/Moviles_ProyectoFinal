using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private UI_ReturnMessage returnMessage;

    public static Action OnSaveDataLoaded;

    private void Awake()
    {
        LoadGameData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(audioConfig);
    }

    private void LoadGameData()
    {
        SaveData data = SaveSystem.LoadGame();

        if (data != null)
        {
            audioConfig.MasterVolume = data.masterVolume;
            audioConfig.MusicVolume = data.musicVolume;
            audioConfig.SfxVolume = data.sfxVolume;
        }

        OnSaveDataLoaded?.Invoke();
    }
}