using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabUI : MonoBehaviour
{
    /*For turning on or off tab based on the button you clicked on*/
    [SerializeField] GameObject[] tabs;

    public void ButtonTurnOnTabBasedOnInputIndex(int tabIndex) 
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].gameObject.SetActive(false);
        }
        tabs[tabIndex - 1].SetActive(true);
    }
}
