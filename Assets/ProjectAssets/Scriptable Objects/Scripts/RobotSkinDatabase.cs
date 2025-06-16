using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RobotSkinDatabase", menuName = "Skins/RobotSkinDatabase")]
public class RobotSkinDatabase : ScriptableObject
{
    [SerializeField] private RobotSkinData defaultSkin;
    [SerializeField] private RobotSkinData currentSkin;
    [SerializeField] private RobotSkinData[] allSkins;
    [SerializeField] private List<RobotSkinData> unlockedSkins;

    public string CurrentSkinId
    {
        get
        {
            return currentSkin != null ? currentSkin.SkinId : defaultSkin.SkinId;
        }
    }

    public string[] UnlockedSkins
    {
        get
        {
            if(unlockedSkins != null)
            {
                string[] unlockedSkinIds = new string[unlockedSkins.Count];
                for (int i = 0; i < unlockedSkinIds.Length; ++i)
                {
                    unlockedSkinIds[i] = unlockedSkins[i].SkinId;
                }
                return unlockedSkinIds;
            }
            else
            {
                unlockedSkins = new List<RobotSkinData>();
                unlockedSkins.Add(defaultSkin);
                string[] unlockedSkinIds = new string[unlockedSkins.Count];
                for (int i = 0; i < unlockedSkinIds.Length; ++i)
                {
                    unlockedSkinIds[i] = unlockedSkins[i].SkinId;
                }
                return unlockedSkinIds;
            }
        }
    }

    public List<RobotSkinData> UnlockedSkinsList
    {
        get
        {
            if (unlockedSkins != null)
            {
                return unlockedSkins;
            }
            else
            {
                unlockedSkins = new List<RobotSkinData>();
                unlockedSkins.Add(defaultSkin);
                return unlockedSkins;
            }
        }
    }

    public RobotSkinData[] AllSkins
    {
        get
        {
            return allSkins;
        }
    }

    public RobotSkinData DefaultSkin
    {
        get
        {
            return defaultSkin;
        }
    }

    public RobotSkinData CurrentSkin
    {
        get
        {
            return currentSkin;
        }
        set
        {
            currentSkin = value;
        }
    }
}