using System.Collections;
using System.Collections.Generic;
using TP.Resource;
using UnityEngine;

public class RBoulder : Resource, IHarvestable
{
    private void OnEnable()
    {
        if (WorldContext.Instance != null)
        {
            WorldContext.Instance.AllBouldersOnTheMap.Add(this);
        }
    }
    private void OnDisable()
    {
        if (WorldContext.Instance != null)
        {
            WorldContext.Instance.AllBouldersOnTheMap.Remove(this);
        }
    }
    public void Harvest()
    {
        Health--;
        if (Health > 0) return;
        animator.SetTrigger("Death");
        gameObject.SetActive(false);
        Debug.Log("You got some stone ");
    }
}
