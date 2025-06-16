using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RobotSkinDatabase robotSkins;
    [SerializeField] private Material robotMaterial;

    void Awake()
    {
        ApplySkin(robotSkins.CurrentSkin);
    }

    public void ChangeSkin(RobotSkinData newSkin)
    {
        if (IsSkinUnlocked(newSkin.SkinId))
        {
            robotSkins.CurrentSkin = newSkin;
            ApplySkin(newSkin);
        }
    }

    public void ChangeSkinByIndex(int index)
    {
        if (index >= 0 && index < robotSkins.AllSkins.Length)
        {
            RobotSkinData skin = robotSkins.AllSkins[index];
            ChangeSkin(skin);
        }
    }

    public void UnlockSkin(string skinID)
    {
        if (IsSkinUnlocked(skinID) == false)
        {
            for (int i = 0; i < robotSkins.AllSkins.Length; ++i)
            {
                if (robotSkins.AllSkins[i].SkinId == skinID)
                {
                    robotSkins.AllSkins[i].IsUnlocked = true;
                    robotSkins.UnlockedSkinsList.Add(robotSkins.AllSkins[i]);
                    return;
                }
            }
        }

        Debug.LogWarning("Skin with ID " + skinID + " is already unlocked or does not exist.");
    }

    public void UnlockSkin(int skinIndex)
    {
        if (IsSkinUnlocked(robotSkins.AllSkins[skinIndex].SkinId) == false)
        {
            robotSkins.AllSkins[skinIndex].IsUnlocked = true;
            robotSkins.UnlockedSkinsList.Add(robotSkins.AllSkins[skinIndex]);
        }

        Debug.LogWarning("Skin with ID " + robotSkins.AllSkins[skinIndex].SkinId + " is already unlocked or does not exist.");
    }


    public bool IsSkinUnlocked(string skinID)
    {
        for(int i = 0; i < robotSkins.UnlockedSkinsList.Count; ++i)
        {
            if (robotSkins.UnlockedSkinsList[i].SkinId == skinID)
            {
                return true;
            }
        }

        return false;
    }


    private void ApplySkin(RobotSkinData skin)
    {
        if(IsSkinUnlocked(skin.SkinId) == false)
        {
            Debug.LogWarning("The skin with ID " + skin.SkinId + " is not unlocked.");
            return;
        }

        if (skin != null)
        {
            robotMaterial.SetTexture("_BaseMap", skin.BaseMap);
            robotMaterial.SetTexture("_EmissionMap", skin.EmissionMap);
        }
        else
        {
            Debug.LogWarning("The skin is null, applying the default skin.");
            robotMaterial.SetTexture("_BaseMap", robotSkins.DefaultSkin.BaseMap);
            robotMaterial.SetTexture("_EmissionMap", robotSkins.DefaultSkin.EmissionMap);
        }

    }

    private void ApplySkin(int skinIndex)
    {
        if (IsSkinUnlocked(robotSkins.AllSkins[skinIndex].SkinId) == false)
        {
            Debug.LogWarning("The skin with ID " + robotSkins.AllSkins[skinIndex].SkinId + " is not unlocked.");
            return;
        }

        robotMaterial.SetTexture("_BaseMap", robotSkins.AllSkins[skinIndex].BaseMap);
        robotMaterial.SetTexture("_EmissionMap", robotSkins.AllSkins[skinIndex].EmissionMap);
    }
}