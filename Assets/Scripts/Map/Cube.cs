using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] Cell currentCell;

    public Cell CurrentCell
    {
        get => currentCell;
        set => currentCell = value;
    }
}
