﻿using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private ResourceData resourceData;

    [Header("Events")]
    public UnityEvent<int> onScrapChanged = new UnityEvent<int>();
    public UnityEvent<int> onPrismitesChanged = new UnityEvent<int>();
    public UnityEvent<int> onEnergyCoresChanged = new UnityEvent<int>();

    public void AddScrap(int amount)
    {
        resourceData.Scrap += amount;
        onScrapChanged.Invoke(resourceData.Scrap);
    }

    public void RemoveScrap(int amount)
    {
        resourceData.Scrap = Mathf.Max(0, resourceData.Scrap - amount);
        onScrapChanged.Invoke(resourceData.Scrap);
    }

    public void AddPrismites(int amount)
    {
        resourceData.Prismites += amount;
        onPrismitesChanged.Invoke(resourceData.Prismites);
    }

    public void RemovePrismites(int amount)
    {
        resourceData.Prismites = Mathf.Max(0, resourceData.Prismites - amount);
        onPrismitesChanged.Invoke(resourceData.Prismites);
    }

    public void AddEnergyCores(int amount)
    {
        resourceData.EnergyCores += amount;
        onEnergyCoresChanged.Invoke(resourceData.EnergyCores);
    }

    public void RemoveEnergyCores(int amount)
    {
        resourceData.EnergyCores = Mathf.Max(0, resourceData.EnergyCores - amount);
        onEnergyCoresChanged.Invoke(resourceData.EnergyCores);
    }

    public bool CanAffordScrap(int amount)
    {
        return resourceData.Scrap >= amount;
    }

    public bool CanAffordPrismites(int amount)
    {
        return resourceData.Prismites >= amount;
    }

    public bool CanAffordEnergyCores(int amount)
    {
        return resourceData.EnergyCores >= amount;
    }
}