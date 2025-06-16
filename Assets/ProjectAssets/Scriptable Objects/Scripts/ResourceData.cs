using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "Robot/ResourceData")]
public class ResourceData : ScriptableObject
{
    [SerializeField] private int scrap;
    [SerializeField] private int prismites;
    [SerializeField] private int energyCores;

    public int Scrap
    {
        get
        {
            return scrap;
        }
        set
        {
            scrap = value;
        }
    }

    public int Prismites
    {
        get
        {
            return prismites;
        }
        set
        {
            prismites = value;
        }
    }

    public int EnergyCores
    {
        get
        {
            return energyCores;
        }
        set
        {
            energyCores = value;
        }
    }
}