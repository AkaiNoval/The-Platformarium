using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Air,
    Water,
    Grass
}
public class Cell : MonoBehaviour
{
    private bool isOccupied;
    public CellType cellType;

    public void UpdateOccupationStatus(bool occupationStatus)
    {
        isOccupied = occupationStatus;
    }
    public bool IsThisCellOccupied()
    {
        return isOccupied;
    }
}
