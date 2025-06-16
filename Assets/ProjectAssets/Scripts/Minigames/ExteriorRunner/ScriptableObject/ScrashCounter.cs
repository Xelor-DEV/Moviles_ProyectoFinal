using UnityEngine;

[CreateAssetMenu(fileName = "ScrashCounter", menuName = "Game/ScrashCounter")]
public class ScrashCounter : ScriptableObject
{
    public int pickupsCollected;

    public void ResetCounter()
    {
        pickupsCollected = 0;
    }

    public void AddPickup()
    {
        pickupsCollected++;
    }
}

