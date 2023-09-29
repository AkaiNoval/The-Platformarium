using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIItemElement : MonoBehaviour
{
    [field: SerializeField] public Image itemImage { get; set; }
    [field: SerializeField] public GameObject rightMaterialUIInfo { get; set; }
    [field: SerializeField] public GameObject leftMaterialUIInfo { get; set; }
    [field: SerializeField] public TMP_Text itemName { get; set; }
    [field: SerializeField] public BuildingDataSO selectedBuilding { get; set; }

    public void ChangeGameStateButton(string state) => GameManager.Instance.ChangeGameStateButton(state);
    public void InitBuildingHighLightButton() => PlacementManager.Instance.GoIntoBuildModeButton(selectedBuilding);
}
