using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentTabHolder : MonoBehaviour
{
    /* Where for the tab generator to set the parent for the generated UIelement*/
    [field: SerializeField] public Transform itemUIInfoParent { get; private set; }
    /* For Tab generator to generate the exact same type for UIElementInfo*/
    [field: SerializeField] public List<BuildingDataSO> buildingDataSOType { get; private set; }

    [field: SerializeField] public MaterialsInfo materialPrefab { get; private set; }
    [field: SerializeField] public UIItemElement uIItemElement { get; private set; }

    public void GenerateItemElement()
    {
        for (int i = 0; i < buildingDataSOType.Count; i++)
        {
            UIItemElement instanceUIItemElement = Instantiate(uIItemElement, itemUIInfoParent);
            MaterialsInfo creationPointInfo = Instantiate(materialPrefab, instanceUIItemElement.rightMaterialUIInfo.transform);

            creationPointInfo.materialCost.text = buildingDataSOType[i].creationPoint.ToString();

            instanceUIItemElement.itemName.text = buildingDataSOType[i].name;
            for (int z = 0; z < buildingDataSOType[i].BuildingMaterialsCost.Count; z++)
            {
                MaterialsInfo instanceMaterialInfo = Instantiate(materialPrefab, instanceUIItemElement.leftMaterialUIInfo.transform);
                instanceMaterialInfo.materialImage.sprite = buildingDataSOType[i].BuildingMaterialsCost[z].Material.sprite;
                instanceMaterialInfo.materialCost.text = buildingDataSOType[i].BuildingMaterialsCost[z].Amount.ToString();
            }
            
        }
    }
}
