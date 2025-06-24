using System;
using System.Collections;
using System.Linq;
using Firebase.Database;
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
}

public class DatabaseManager : SingletonPersistent<DatabaseManager>
{
    [Header("References")]
    [SerializeField] private UserDataSO userData;
    [SerializeField] private ResourceData resourceData;
    [SerializeField] private RobotSkinDatabase skinDatabase;

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

            resourceData.Scrap = playerData.scrap;
            resourceData.Prismites = playerData.prismites;
            resourceData.EnergyCores = playerData.energyCores;

            skinDatabase.CurrentSkin = skinDatabase.FindSkinByID(playerData.currentSkinId);

            UpdateUnlockedSkins(playerData.unlockedSkins);
            OnLoadData?.Invoke();
            Debug.Log("LOAD DATA COMPLETE");
            dataLoaded = true;
        }
        else
        {
            SaveAllData();
            OnLoadData?.Invoke();
            Debug.Log("INITIAL SAVE COMPLETE");
            dataLoaded = true;
        }
    }

    public void SaveAllData()
    {
        PlayerData playerData = new PlayerData
        {
            userId = userData.UserId,
            scrap = resourceData.Scrap,
            prismites = resourceData.Prismites,
            energyCores = resourceData.EnergyCores,
            currentSkinId = skinDatabase.CurrentSkinId,
            unlockedSkins = skinDatabase.UnlockedSkins
        };

        string jsonData = JsonUtility.ToJson(playerData);
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
