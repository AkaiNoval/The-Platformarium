using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Building
{
    [SerializeField] int maxCapacity;
    [SerializeField] int currentCapacity;

    public int CurrentCapacity 
    { 
        get => currentCapacity; 
        set => currentCapacity = Mathf.Clamp(value,0,MaxCapacity); 
    }
    public int MaxCapacity 
    { 
        get => maxCapacity; 
        private set => maxCapacity = value; 
    }

    private void OnEnable()
    {
        if (WorldContext.Instance != null)
        {
            WorldContext.Instance.AllStorageInTheMap.Add(this);
        }
    }
    private void OnDisable()
    {
        if (WorldContext.Instance != null)
        {
            WorldContext.Instance.AllStorageInTheMap.Remove(this);
        }
    }
}
