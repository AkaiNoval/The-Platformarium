using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlacementValidationSystem : MonoBehaviour
{

    /* Cache all the cell when checking to update it later if a building is built*/
    [SerializeField] private List<Cell> cellsToUpdate;
    public bool AreCellsValidForBuildMode(BuildingDataSO currentBuildingData,
                                          InputManager inputManager,
                                          PlacementRotator placementRotator,
                                          List<GameObject> highLightingCells)
    {
        cellsToUpdate = new List<Cell>();
        /* No building data selected or inputManager not properly initialized */
        if (inputManager.currentCell == null)
        {
            Debug.LogWarning($"CurrentCell is null");
            return false;
        }
        /* This case if a building size is 1x1 so only need to do the 1x1 check */
        if (highLightingCells.Count < 1)
        {
            return CheckingForValidationOf1x1BuildingSize(inputManager);
        }

        int buildingWidth = currentBuildingData.BuildingSize.x;
        int buildingHeight = currentBuildingData.BuildingSize.y;
        /* Cache the current position of the current cell */
        Vector2Int currentCellInGridPosition = inputManager.GetCellInGridPosition(inputManager.currentCell);
        /* Cache the map grid */
        var mapGrid = MapController.Instance.ProceduralTerrain.GetMapGrid();
        switch (placementRotator.GetPlacementRotationDegrees())
        {
            case PlacementRotationDegrees.UpDegree:
                return CheckingForValidationAtZeroDegrees(buildingWidth, buildingHeight, currentCellInGridPosition, mapGrid);
            case PlacementRotationDegrees.RightDegree:
                return CheckingForValidationAt90Degrees(buildingWidth, buildingHeight, currentCellInGridPosition, mapGrid);
            case PlacementRotationDegrees.DownDegree:
                return CheckingForValidationAt180Degrees(buildingWidth, buildingHeight, currentCellInGridPosition, mapGrid);
            case PlacementRotationDegrees.LeftDegree:
                return CheckingForValidationAt270Degrees(buildingWidth, buildingHeight, currentCellInGridPosition, mapGrid);
            default:
                break;
        }
        return true;   
    }
    public List<Cell> CellsToUpdate() => cellsToUpdate;

    #region ValidationCheckingMethods
    bool CheckingForValidationOf1x1BuildingSize(InputManager inputManager)
    {
        /* Check if the selected cell is occupied or not grass */
        if (inputManager.currentCell.IsThisCellOccupied() || inputManager.currentCell.cellType != CellType.Grass)
        {
            Debug.Log("This 1x1 cell is invalid for building");
            return false;
        }
        cellsToUpdate.Add(inputManager.currentCell);
        return true;
    }
    bool CheckingForValidationAtZeroDegrees(int buildingWidth, int buildingHeight, Vector2Int currentCellInGridPosition, Cell[,] mapGrid)
    {
        for (int xOffset = 0; xOffset < buildingWidth; xOffset++)
        {
            for (int yOffset = 0; yOffset < buildingHeight; yOffset++)
            {
                /* Calculate the position of the current cell within the building area*/
                Vector2Int positionToCheck = currentCellInGridPosition + new Vector2Int(xOffset, yOffset);

                if (IsPositionOutOfBound(positionToCheck, mapGrid))
                {
                    Debug.LogError("This shouldn't happen, the position to check is out of bound, WTF?");
                    Debug.LogError("Position To Check is: " + positionToCheck);
                    return false;
                }
                Cell cellToCheck = mapGrid[positionToCheck.x, positionToCheck.y];
                /* Check if the cell at the position is occupied or not grass */
                if (cellToCheck.IsThisCellOccupied() || cellToCheck.cellType != CellType.Grass)
                {
                    Debug.Log("This cell is invalid for building");
                    return false;
                }
                cellsToUpdate.Add(cellToCheck);
            }
        }
        return true;
    }
    bool CheckingForValidationAt90Degrees(int buildingWidth, int buildingHeight, Vector2Int currentCellInGridPosition, Cell[,] mapGrid)
    {
        for (int xOffset = 0; xOffset < buildingHeight; xOffset++)
        {
            for (int yOffset = 0; yOffset > -buildingWidth; yOffset--)
            {
                /* Calculate the position of the current cell within the building area*/
                Vector2Int positionToCheck = currentCellInGridPosition + new Vector2Int(xOffset, yOffset);

                if (IsPositionOutOfBound(positionToCheck, mapGrid))
                {
                    Debug.LogError("This shouldn't happen, the position to check is out of bound, WTF?");
                    Debug.LogError("Position To Check is: " + positionToCheck);
                    return false;
                }
                Cell cellToCheck = mapGrid[positionToCheck.x, positionToCheck.y];
                /* Check if the cell at the position is occupied or not grass */
                if (cellToCheck.IsThisCellOccupied() || cellToCheck.cellType != CellType.Grass)
                {
                    Debug.Log("This cell is invalid for building");
                    return false;
                }
                cellsToUpdate.Add(cellToCheck);
            }
        }
        return true;
    }
    bool CheckingForValidationAt180Degrees(int buildingWidth, int buildingHeight, Vector2Int currentCellInGridPosition, Cell[,] mapGrid)
    {
        for (int xOffset = 0; xOffset > -buildingWidth; xOffset--)
        {
            for (int yOffset = 0; yOffset > -buildingHeight; yOffset--)
            {
                /* Calculate the position of the current cell within the building area*/
                Vector2Int positionToCheck = currentCellInGridPosition + new Vector2Int(xOffset, yOffset);

                if (IsPositionOutOfBound(positionToCheck, mapGrid))
                {
                    Debug.LogError("This shouldn't happen, the position to check is out of bound, WTF?");
                    Debug.LogError("Position To Check is: " + positionToCheck);
                    return false;
                }
                Cell cellToCheck = mapGrid[positionToCheck.x, positionToCheck.y];
                /* Check if the cell at the position is occupied or not grass */
                if (cellToCheck.IsThisCellOccupied() || cellToCheck.cellType != CellType.Grass)
                {
                    Debug.Log("This cell is invalid for building");
                    return false;
                }
                cellsToUpdate.Add(cellToCheck);
            }
        }
        return true;
    }
    bool CheckingForValidationAt270Degrees(int buildingWidth, int buildingHeight, Vector2Int currentCellInGridPosition, Cell[,] mapGrid)
    {
        for (int xOffset = 0; xOffset > -buildingHeight; xOffset--)
        {
            for (int yOffset = 0; yOffset < buildingWidth; yOffset++)
            {
                /* Calculate the position of the current cell within the building area*/
                Vector2Int positionToCheck = currentCellInGridPosition + new Vector2Int(xOffset, yOffset);

                if (IsPositionOutOfBound(positionToCheck, mapGrid))
                {
                    Debug.LogError("This shouldn't have happened, the position to check is out of bound, WTF?");
                    Debug.LogError("Position To Check is: " + positionToCheck);
                    return false;
                }
                Cell cellToCheck = mapGrid[positionToCheck.x, positionToCheck.y];
                /* Check if the cell at the position is occupied or not grass */
                if (cellToCheck.IsThisCellOccupied() || cellToCheck.cellType != CellType.Grass)
                {
                    Debug.Log("This cell is invalid for building");
                    return false;
                }
                cellsToUpdate.Add(cellToCheck);
            }
        }
        return true;
    }
    bool IsPositionOutOfBound(Vector2Int positionToCheck, Cell[,] mapGrid)
    {
        return positionToCheck.x < 0 || positionToCheck.x >= mapGrid.GetLength(0) ||
               positionToCheck.y < 0 || positionToCheck.y >= mapGrid.GetLength(1);
    }
    #endregion
  
}
