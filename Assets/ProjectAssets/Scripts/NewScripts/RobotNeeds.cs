using UnityEngine;

[CreateAssetMenu(fileName = "RobotNeeds", menuName = "Robot/Needs")]
public class RobotNeeds : ScriptableObject
{
    [Range(0, 1)] public float armor = 1f;
    public float armorDecayRate = 0.01f;
    public float armorRepairPerScrap = 0.25f;
    public float minArmorForDamage = 0.2f;
}