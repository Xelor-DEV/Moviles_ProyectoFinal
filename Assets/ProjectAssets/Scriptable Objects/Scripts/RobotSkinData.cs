using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "Skins/SkinData")]
public class RobotSkinData : ScriptableObject
{
    [Header("Identification")]
    [SerializeField] private string skinID;
    [SerializeField] private string skinName;
    [Header("Textures")]
    [SerializeField] private Texture2D baseMap;
    [SerializeField] private Texture2D emissionMap;
    [Header("Status")]
    [SerializeField] private bool isUnlocked;

    public string SkinId
    {
        get
        {
            return skinID;
        }
    }

    public string SkinName
    {
        get
        {
            return skinName;
        }
    }

    public Texture2D BaseMap
    {
        get
        {
            return baseMap;
        }
    }

    public Texture2D EmissionMap
    {
        get
        {
            return emissionMap;
        }
    }

    public bool IsUnlocked
    {
        get
        {
            return isUnlocked;
        }
        set
        {
            isUnlocked = value;
        }
    }
}