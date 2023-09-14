using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementPreview : MonoBehaviour
{
    [SerializeField] GameObject highLightingCellPrefab, highLightingOriginCell;
    private List<GameObject> highLightingCells;
    public List<GameObject> InstantiateHighlightsBasedOnBuildingSize(BuildingDataSO currentBuildingData)
    {
        /* Whenever we created highlightingcells  we should always reset our main cell rotation*/
        highLightingOriginCell.transform.rotation = Quaternion.Euler(0, 0, 0);
        highLightingCells = new List<GameObject>();
        if (currentBuildingData == null)
        {
            Debug.LogWarning("There is no selected buidling, please select one");
            return highLightingCells;
        }
        DestroyAllChildren();//TODO Refactor for ObjectPooling
        /* Based on building size, create highlighting cells*/
        int buildingWidth = currentBuildingData.BuildingSize.x;
        int buildingHeight = currentBuildingData.BuildingSize.y;

        for (int x = 0; x < buildingWidth; x++)
        {
            for (int y = 0; y < buildingHeight; y++)
            {

                Vector3 position = new Vector3(x, 0, y) + highLightingOriginCell.transform.position;
                GameObject newCell = Instantiate(highLightingCellPrefab, position, Quaternion.identity);
                newCell.transform.parent = highLightingOriginCell.transform;
                highLightingCells.Add(newCell);
            }
        }
        return highLightingCells;
    }
    void DestroyAllChildren()
    {
        highLightingCells = new List<GameObject>();
        int childCount = highLightingOriginCell.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = highLightingOriginCell.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
    public List<GameObject> GetHighLightingCells() => highLightingCells;
    public GameObject GetHighLightngOriginCell() => highLightingOriginCell;
    public Building TransparentBuilding { get; set; }

    public void InstantiateBuildingPreview(BuildingDataSO currentBuildingData)
    {
        TransparentBuilding = Instantiate(currentBuildingData.BuildingPrefab);
        TransparentBuilding.transform.parent = highLightingOriginCell.transform;
        TransparentBuilding.transform.localPosition = Vector3.zero;
    }
}
