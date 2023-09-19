using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] Cell currentCell;
    [SerializeField] GameObject dirtPart;

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
    public void SetActiveDirtPlatform(bool status) => dirtPart.SetActive(status);
    public bool ShouldShowDirt(int x, int z)
    {
        /* Get the map grid and its dimensions */
        Cell[,] grid = MapController.Instance.ProceduralTerrain.GetMapGrid();
        int mapSizeX = grid.GetLength(0);
        int mapSizeZ = grid.GetLength(1);

        /* Check if the cell is at the border of the map */
        if (x == 0 || x == mapSizeX - 1 || z == 0 || z == mapSizeZ - 1)
        {
            /* Cell is at the border, return true */
            return true;
        }
        /* Check adjacent cells for occupancy */
        Cube leftCube = x > 0 ? grid[x - 1, z].GetCurrentCube() : null;
        Cube rightCube = x < mapSizeX - 1 ? grid[x + 1, z].GetCurrentCube() : null;
        Cube lowerCube = z > 0 ? grid[x, z - 1].GetCurrentCube() : null;
        Cube upperCube = z < mapSizeZ - 1 ? grid[x, z + 1].GetCurrentCube() : null;

        /* Check if any adjacent cube is not exposed (not active) */
        if ((leftCube != null && !leftCube.gameObject.activeSelf) ||
            (rightCube != null && !rightCube.gameObject.activeSelf) ||
            (lowerCube != null && !lowerCube.gameObject.activeSelf) ||
            (upperCube != null && !upperCube.gameObject.activeSelf))
        {
            /* At least one adjacent cube is not exposed, return true */
            return true;
        }

        /* If none of the above conditions are met, return false */
        return false;
    }

}
