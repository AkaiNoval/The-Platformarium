using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemElement : MonoBehaviour
{
    [field: SerializeField] Image itemImage { get; set; }
    [field: SerializeField] GameObject rightMaterialUIInfo { get; set; }
    [field: SerializeField] GameObject leftMaterialUIInfo { get; set; }
    [field: SerializeField] TMP_Text itemName { get; set; }
}
