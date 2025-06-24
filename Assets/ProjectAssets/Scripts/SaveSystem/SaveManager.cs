using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RobotNeeds robotNeeds;
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private UI_ReturnMessage returnMessage;

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
        SaveSystem.SaveGame(robotNeeds, audioConfig);
    }

    private void LoadGameData()
    {
        SaveData data = SaveSystem.LoadGame();

        if (data != null)
        {
            audioConfig.MasterVolume = data.masterVolume;
            audioConfig.MusicVolume = data.musicVolume;
            audioConfig.SfxVolume = data.sfxVolume;

            if (DateTime.TryParseExact(
                data.saveDateTime,
                "o",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.RoundtripKind,
                out DateTime saveTime))
            {
                ApplyNeedsDecay(data, saveTime);
            }
            else
            {
                RestoreNeeds(data);
            }
        }
    }

    private void ApplyNeedsDecay(SaveData data, DateTime saveTime)
    {
        TimeSpan timePassed = DateTime.UtcNow - saveTime;
        float totalSeconds = (float)timePassed.TotalSeconds;

        if (totalSeconds < 8f)
        {
            RestoreNeeds(data);
            return;
        }

        float intervalsPassed = totalSeconds / robotNeeds.decayInterval;

        float decayAmount = robotNeeds.globalDecayRate * intervalsPassed;

        robotNeeds.Armor = Mathf.Clamp01(data.armor - decayAmount);
        robotNeeds.Power = Mathf.Clamp01(data.power - decayAmount);
        robotNeeds.Fun = Mathf.Clamp01(data.fun - decayAmount);

        float totalDecay = (data.armor - robotNeeds.Armor) +
                          (data.power - robotNeeds.Power) +
                          (data.fun - robotNeeds.Fun);

        returnMessage.ShowMessage(totalDecay);

        Debug.Log(totalDecay + " " + decayAmount);

        robotNeeds.SendNotificationPower();
        robotNeeds.SendNotificationFun();
        robotNeeds.SendNotificationArmor();
    }

    private void RestoreNeeds(SaveData data)
    {
        robotNeeds.Armor = data.armor;
        robotNeeds.Power = data.power;
        robotNeeds.Fun = data.fun;
    }
}