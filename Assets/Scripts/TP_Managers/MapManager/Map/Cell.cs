using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    /* Air is just for debugging, not really a use for 2D Array*/
    Air,
    Water,
    Grass
}
public class Cell : MonoBehaviour
{
    /* If there is anythong on the surface of this cell, set to true*/
    [SerializeField] private bool isOccupied;
    [SerializeField] Cube currentCube;
    public CellType cellType;

    public void UpdateOccupationStatus(bool occupationStatus) => isOccupied = occupationStatus;
    public bool IsThisCellOccupied() => isOccupied;
    public void SetCurrentCube(Cube cube) 
    { 
        this.currentCube = cube;
        //Debug.Log($"Set {currentCube.name} to this {name} ");
    }
    public Cube GetCurrentCube() => currentCube;
}
