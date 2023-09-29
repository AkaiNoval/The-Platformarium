using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Building
{
    [SerializeField] int maxCapacity;
    [SerializeField] int currentCapacity;
    [SerializeField] GameObject lowCap;
    [SerializeField] GameObject MedCap;
    [SerializeField] GameObject HighCap;
    public int CurrentCapacity 
    { 
        get => currentCapacity;
        set 
        {
            if(currentCapacity > (20 * maxCapacity)/100)
            {
                lowCap.SetActive(true);
            }
            if(currentCapacity > (50 * maxCapacity) / 100)
            {
                MedCap.SetActive(true);
            }
            if (currentCapacity > (70 * maxCapacity) / 100)
            {
                HighCap.SetActive(true);
            }
            currentCapacity = Mathf.Clamp(value, 0, MaxCapacity); 
        } 
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
            WorldContext.Instance.AllStorageOnTheMap.Add(this);
        }
    }
    private void OnDisable()
    {
        if (WorldContext.Instance != null)
        {
            WorldContext.Instance.AllStorageOnTheMap.Remove(this);
        }
    }
}
