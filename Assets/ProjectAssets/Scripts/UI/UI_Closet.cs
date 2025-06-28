using UnityEngine;
using UnityEngine.UI;

public class UI_Closet : MonoBehaviour
{
    [SerializeField] private Button[] skinButtons;

    public void LockSkin(int index)
    {
        skinButtons[index].interactable = false;
    }

    public void UnlockSkin(int index)
    {
        skinButtons[index].interactable = true;
    }
}