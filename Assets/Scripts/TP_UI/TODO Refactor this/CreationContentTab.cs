using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationContentTab : MonoBehaviour
{
    /* Where for the tab generator to set the parent for the generated UIelement*/
    [field: SerializeField] public Transform itemUIInfoParent { get; private set; }
    /* For Tab generator to generate the exact same type for UIElementInfo*/
    [field: SerializeField] public List<SOPrototype> prototypeSOType { get; private set; }

    [field: SerializeField] public MaterialsInfo materialPrefab { get; private set; }
    [field: SerializeField] public UIItemElement uIItemElement { get; private set; }

    public void GenerateItemElement()
    {
        for (int i = 0; i < prototypeSOType.Count; i++)
        {
            UIItemElement instanceUIItemElement = Instantiate(uIItemElement, itemUIInfoParent);
            MaterialsInfo creationPointInfo = Instantiate(materialPrefab, instanceUIItemElement.rightMaterialUIInfo.transform);

            creationPointInfo.materialCost.text = prototypeSOType[i].creationPoint.ToString();

            instanceUIItemElement.itemName.text = prototypeSOType[i].name;
        }
    }
}
