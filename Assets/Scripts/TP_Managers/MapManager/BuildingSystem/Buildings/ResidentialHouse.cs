using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialHouse : Building
{
    private void OnEnable()
    {
        WorldContext.Instance.AllHousesOnTheMap.Add(this);
    }
    private void OnDisable()
    {
        WorldContext.Instance.AllHousesOnTheMap.Remove(this);
    }
}
