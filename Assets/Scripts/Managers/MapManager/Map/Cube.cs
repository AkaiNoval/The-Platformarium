using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] Cell currentCell;
    [SerializeField] GameObject dirtChild;

    public Cell CurrentCell
    {
        get => currentCell;
        set 
        { 
            currentCell = value;
            currentCell.SetCurrentCube(this);
        }
    }
    private void OnEnable()
    {
        CubeManager.AddIntoTheList(this);
    }
    private void OnDisable()
    {
        CubeManager.RemoeOutOfTheList(this);
    }
    public void SetActiveDirtPlatform(bool status) => dirtChild.SetActive(status);
    private bool ShouldShowDirt(int x, int y)
    {
        // Check if any adjacent cell is occupied (not exposed)
        Cell[,] grid = MapController.Instance.ProceduralTerrain.GetMapGrid();
        int mapSizeX = grid.GetLength(0);
        int mapSizeY = grid.GetLength(1);

        bool isExposed = true;

        if (x > 0)
        {
            isExposed &= !grid[x - 1, y].IsThisCellOccupied();
        }
        if (x < mapSizeX - 1)
        {
            isExposed &= !grid[x + 1, y].IsThisCellOccupied();
        }
        if (y > 0)
        {
            isExposed &= !grid[x, y - 1].IsThisCellOccupied();
        }
        if (y < mapSizeY - 1)
        {
            isExposed &= !grid[x, y + 1].IsThisCellOccupied();
        }

        return isExposed;
    }
}
