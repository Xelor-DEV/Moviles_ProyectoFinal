using System;
using System.Collections;
using System.Linq;
using Firebase.Database;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string userId;
    public int scrap;
    public int prismites;
    public int energyCores;
    public string currentSkinId;
    public string[] unlockedSkins;

    public float armor;
    public float power;
    public float fun;
    public string lastSaveTime;


    public PlayerData()
    {
        scrap = 10;
        prismites = 10;
        energyCores = 10;
        currentSkinId = "mighty";
        unlockedSkins = new string[] { "mighty" };
        armor = 0.5f;
        power = 0.5f;
        fun = 0.5f;
        lastSaveTime = DateTime.UtcNow.ToString("o");
    }
}

public class DatabaseManager : SingletonPersistent<DatabaseManager>
{
    [Header("References")]
    [SerializeField] private UserDataSO userData;
    [SerializeField] private ResourceData resourceData;
    [SerializeField] private RobotSkinDatabase skinDatabase;
    [SerializeField] private RobotNeeds robotNeeds;

    public static Action OnLoadData;

    private DatabaseReference dbReference;
    private bool dataLoaded = false;

    private string UserPath
    {
        get
        {
            return "users/" + userData.UserId;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
        if (!dataLoaded && userData.IsLoggedIn)
        {
            StartCoroutine(LoadUserData());
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (paused && dataLoaded)
        {
            SaveAllData();
            Debug.Log("SAVE PAUSE");
        }
    }

    public IEnumerator LoadUserData()
    {
        var dataTask = dbReference.Child(UserPath).GetValueAsync();
        yield return new WaitUntil(() => dataTask.IsCompleted);

        if (dataTask.Exception != null)
        {
            Debug.LogError("Load failed: " + dataTask.Exception);
            yield break;
        }

        if (dataTask.Result.Exists)
        {
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(dataTask.Result.GetRawJsonValue());

            if (robotNeeds != null && !string.IsNullOrEmpty(playerData.lastSaveTime))
            {
                CalculateAndApplyOfflineDecay(playerData);
            }

            ApplyPlayerData(playerData);
            OnLoadData?.Invoke();
            Debug.Log("LOAD DATA COMPLETE");
            dataLoaded = true;
        }
        else
        {
            PlayerData defaultData = new PlayerData();
            defaultData.userId = userData.UserId;
            ApplyPlayerData(defaultData);
            SavePlayerData(defaultData);
            OnLoadData?.Invoke();
            Debug.Log("INITIAL SAVE COMPLETE");
            dataLoaded = true;
        }
    }

    private void CalculateAndApplyOfflineDecay(PlayerData playerData)
    {
        try
        {
            DateTime lastSaveTime = DateTime.Parse(playerData.lastSaveTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
            TimeSpan timeSpan = DateTime.UtcNow - lastSaveTime;

            double totalMinutes = timeSpan.TotalMinutes;
            double intervalsPassed = totalMinutes / robotNeeds.offlineDecayIntervalMinutes;

            UI_ReturnMessage.Instance.ShowMessage(totalMinutes);

            if (intervalsPassed > 0)
            {
                float decayAmount = (float)(robotNeeds.offlineDecayRatePerInterval * intervalsPassed);

                playerData.armor = Mathf.Clamp01(playerData.armor - decayAmount);
                playerData.power = Mathf.Clamp01(playerData.power - decayAmount);
                playerData.fun = Mathf.Clamp01(playerData.fun - decayAmount);

                Debug.Log($"Applied offline decay: {decayAmount} over {totalMinutes} minutes");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error calculating offline decay: {e.Message}");
        }
    }

    private void ApplyPlayerData(PlayerData data)
    {
        resourceData.Scrap = data.scrap;
        resourceData.Prismites = data.prismites;
        resourceData.EnergyCores = data.energyCores;

        UpdateUnlockedSkins(data.unlockedSkins);

        RobotSkinData robotSkinData = skinDatabase.FindSkinByID(data.currentSkinId);
        SkinManager.Instance.ChangeSkin(robotSkinData);

        robotNeeds.Armor = data.armor;
        robotNeeds.Power = data.power;
        robotNeeds.Fun = data.fun;
    }

    public void SaveAllData()
    {
        PlayerData currentData = new PlayerData()
        {
            userId = userData.UserId,
            scrap = resourceData.Scrap,
            prismites = resourceData.Prismites,
            energyCores = resourceData.EnergyCores,
            currentSkinId = skinDatabase.CurrentSkinId,
            unlockedSkins = skinDatabase.UnlockedSkins,

            armor = robotNeeds.Armor,
            power = robotNeeds.Power,
            fun = robotNeeds.Fun,
            lastSaveTime = DateTime.UtcNow.ToString("o")
        };

        SavePlayerData(currentData);
    }

    private void SavePlayerData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        dbReference.Child(UserPath).SetRawJsonValueAsync(jsonData);
    }


    private void UpdateUnlockedSkins(string[] unlockedSkinIds)
    {
        skinDatabase.UnlockedSkinsList.Clear();

        foreach (RobotSkinData skin in skinDatabase.AllSkins)
        {
            if (unlockedSkinIds.Contains(skin.SkinId))
            {
                skin.IsUnlocked = true;
                skinDatabase.UnlockedSkinsList.Add(skin);
            }
            else
            {
                skin.IsUnlocked = false;
            }
        }
    }
}
