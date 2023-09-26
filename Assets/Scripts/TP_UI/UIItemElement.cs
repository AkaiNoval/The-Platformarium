using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemElement : MonoBehaviour
{
    [field: SerializeField] public Image itemImage { get; set; }
    [field: SerializeField] public GameObject rightMaterialUIInfo { get; set; }
    [field: SerializeField] public GameObject leftMaterialUIInfo { get; set; }
    [field: SerializeField] public TMP_Text itemName { get; set; }
}
