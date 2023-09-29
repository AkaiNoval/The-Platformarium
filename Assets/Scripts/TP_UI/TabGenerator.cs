using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabGenerator : MonoBehaviour
{
    [field: SerializeField] List<BuildingContentTabHolder> contentHolderTabs;
    private void Awake()
    {
        contentHolderTabs = GetComponentsInChildren<BuildingContentTabHolder>(true).ToList();
    }
    private void Start()
    {
        foreach (var item in contentHolderTabs)
        {
            item.GenerateItemElement();
        }
    }
}
