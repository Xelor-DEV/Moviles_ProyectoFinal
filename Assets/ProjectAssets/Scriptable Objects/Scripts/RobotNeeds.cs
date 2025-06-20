﻿using UnityEngine;

[CreateAssetMenu(fileName = "RobotNeeds", menuName = "Robot/Needs")]
public class RobotNeeds : ScriptableObject
{
    [Header("Values")]
    [Range(0, 1)]
    [SerializeField] private float armor = 0.5f;
    [Range(0, 1)] 
    [SerializeField] private float power = 0.5f;
    [Range(0, 1)] 
    [SerializeField] private float fun = 0.5f;

    [Header("Decay Settings")]
    [Tooltip("Percentage decay per interval (0.01 = 1%)")]
    public float globalDecayRate = 0.01f;
    public float decayInterval = 15f;

    [Header("Armor Settings")]
    public float armorRepairPerScrap = 0.05f;
    public float minArmorForDamage = 0.2f;

    public float Armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
        }
    }

    public float Power
    {
        get
        {
            return power;
        }
        set
        {
            power = value;
        }
    }

    public float Fun
    {
        get
        {
            return fun;
        }
        set
        {
            fun = value;
        }
    }
}