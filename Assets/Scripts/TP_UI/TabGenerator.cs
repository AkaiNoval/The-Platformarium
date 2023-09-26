using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabGenerator : MonoBehaviour
{
    [field: SerializeField] List<ContentTabHolder> contentHolderTabs;
    private void Awake()
    {
        contentHolderTabs = GetComponentsInChildren<ContentTabHolder>(true).ToList();
    }
    private void Start()
    {
        foreach (var item in contentHolderTabs)
        {
            item.GenerateItemElement();
        }
    }
}
