using System.Collections;
using System.Collections.Generic;
using TP.Resource;
using UnityEngine;

public class RBerryBush : Resource, IHarvestable
{
    public GameObject fruit;
    private void OnEnable()
    {
        if (WorldContext.Instance != null)
        {
            WorldContext.Instance.AllBushesOnTheMap.Add(this);
        }
    }
    private void OnDisable()
    {
        if (WorldContext.Instance != null)
        {
            WorldContext.Instance.AllBushesOnTheMap.Remove(this);
        }
    }
    public void Harvest()
    {
        fruit.SetActive(false);
        WorldContext.Instance.AllBushesOnTheMap.Remove(this);
    }
}
