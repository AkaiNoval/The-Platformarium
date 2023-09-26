using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class CubeManager
{
    public static List<Cube> allCubesOnTheMap = new List<Cube>();
    public static void AddIntoTheList(Cube newCube) => allCubesOnTheMap.Add(newCube);
    public static void RemoeOutOfTheList(Cube newCube) => allCubesOnTheMap.Remove(newCube);
    
}
public enum TerrainEditType
{
    Raise,
    Lower
}
public class TerrainEdit : MonoBehaviour
{
    [Range(1,5)]
    [SerializeField] int brushSize;
    [SerializeField] GameObject mouseHighLight;
    [SerializeField] GameObject highLightingCellPrefab;
    [SerializeField] TerrainEditType worldEditType;
    GameObject[,] highLightingCells;
    public int BrushSize 
    { 
        get => brushSize;
        set
        {
            brushSize = Mathf.Clamp(value, 1, maxBrushSize);
            HighLightBasedOnBrushSize(value, highLightingCells);
        }
    }
    int maxBrushSize = 5;
    private void Start()
    {
        InstantiateHighlightsBasedOnBrushSize(maxBrushSize, mouseHighLight, highLightingCellPrefab);       
    }
    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.WorldEditing && !EventSystem.current.IsPointerOverGameObject())
        {        
            mouseHighLight.SetActive(true);
            HighLightBasedOnBrushSize(BrushSize, highLightingCells);
            Debug.Log("Time for world editing");
            if (mouseHighLight == null) return;
            SnapCurrentMousePosistionToCurrentCellPosition(mouseHighLight.transform);
            if (Input.GetMouseButton(0))
            {
                TerrainEditing(BrushSize, worldEditType);
            }
        }
        else if (GameManager.Instance.gameState != GameState.WorldEditing)
        {
            mouseHighLight.SetActive(false);
        }
    }
    private void SnapCurrentMousePosistionToCurrentCellPosition(Transform mouseHighLightPosition)
    {
        /* Get first, snap later */
        Vector3 mousePos = InputManager.Instance.GetSelectedCellPosition();
        Vector3 snappedPos = InputManager.Instance.SnapHighLightCellGOBasedOnCurrentCellPos(mousePos);
        mouseHighLightPosition.position = snappedPos;
        
    }
    private void InstantiateHighlightsBasedOnBrushSize(int maxBrushSize,
                                                      GameObject MouseHighLight,
                                                      GameObject highLightingCell)
    {
        highLightingCells = new GameObject[maxBrushSize, maxBrushSize];
        for (int x = 0; x < maxBrushSize; x++)
        {
            for (int z = 0; z < maxBrushSize; z++)
            {
                Vector3 position = new Vector3(x, 0, z) + MouseHighLight.transform.position;
                GameObject newHighLight = Instantiate(highLightingCell, position, Quaternion.identity);
                newHighLight.transform.parent = MouseHighLight.transform;
                newHighLight.name = $"Brush high light ({x}, {z})";
                highLightingCells[x, z] = newHighLight;
                newHighLight.SetActive(false);
            }
        }
    }
    private void HighLightBasedOnBrushSize(int localBrushSize, GameObject[,] localHighLightingCells)
    {
        /* Ensure brushSize is within a valid range */
        localBrushSize = Mathf.Clamp(localBrushSize, 1, maxBrushSize);

        for (int x = 0; x < localHighLightingCells.GetLength(0); x++)
        {
            for (int z = 0; z < localHighLightingCells.GetLength(1); z++)
            {
                /* Set the cell active if it's within the brush size, else set it inactive */
                localHighLightingCells[x, z].SetActive(x < localBrushSize && z < localBrushSize);
            }
        }
    }

    private void TerrainEditing(int localBrushSize, TerrainEditType localWorldEditType)
    {
        /* Get the current cell's position*/
        Cell currentCell = InputManager.Instance.CurrentCell;
        /* Get the current cell's position in grid coordinates */
        Vector2Int currentCellGridPos = InputManager.Instance.GetCellInGridPosition(currentCell);
        Cell[,] grid = MapController.Instance.ProceduralTerrain.GetMapGrid();

        bool anyCubesChanged = false; // Flag to check if any cubes were activated or deactivated

        for (int xOffset = 0; xOffset < localBrushSize; xOffset++)
        {
            for (int yOffset = 0; yOffset < localBrushSize; yOffset++)
            {
                /* Calculate the grid position of the target cell */
                int targetX = currentCellGridPos.x + xOffset;
                int targetY = currentCellGridPos.y + yOffset;

                /* Check if the target cell is within the valid grid range */
                if (targetX >= 0 && targetX < grid.GetLength(0) && targetY >= 0 && targetY < grid.GetLength(1))
                {
                    Cell targetCell = grid[targetX, targetY];
                    Cube targetCube = targetCell.GetCurrentCube();

                    if (targetCell.IsThisCellOccupied())
                    {
                        Debug.LogWarning($"{targetCell.name} is already occupied. TerrainEdit operation canceled.");
                        continue;
                    }
                    /* Check if the target cube exists*/
                    if (targetCube != null)
                    {
                        if (localWorldEditType == TerrainEditType.Raise)
                        {
                            /* Raise: Activate the cube if it's not already active */
                            if (!targetCube.gameObject.activeSelf)
                            {
                                targetCube.gameObject.SetActive(true);
                                anyCubesChanged = true;
                            }
                        }
                        else if (localWorldEditType == TerrainEditType.Lower)
                        {
                            /* Lower: Deactivate the cube if it's active */
                            if (targetCube.gameObject.activeSelf)
                            {
                                targetCube.gameObject.SetActive(false);
                                anyCubesChanged = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Target Cube of the {targetCell.name} is null");
                        Debug.LogWarning($"Maybe you should wait for the map to finish loading?");
                    }
                }
                else
                {
                    Debug.LogWarning($"The cells you are trying to access are out of range");
                }
            }
        }
        /* Add a check for all cubes already activated to skip unnecessary updates */
        if (!anyCubesChanged)
        {
            Debug.LogWarning("No cubes were changed in the brush area.");
        }
    }
}
