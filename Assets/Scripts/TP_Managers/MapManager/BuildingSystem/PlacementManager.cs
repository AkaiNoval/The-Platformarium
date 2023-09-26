using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlacementMode
{
    WorldEdit,
    BuildMode
}
public class PlacementManager : MonoBehaviour
{
    PlacementValidationSystem placementValidationSystem;
    PlacementRotator placementRotator;
    PlacementPreview placementPreview;

    [SerializeField] GameObject originCellHighLight;

    [SerializeField] PlacementMode placementMode;
    [SerializeField] BuildingDataSO currentBuildingData;

    [SerializeField] Material invalidMaterialForCellHighLight;
    [SerializeField] Material validMaterialForCellHighLight;

    bool allowedToBuild;
    private List<GameObject> childCellHighLights = new List<GameObject>();
    private Cell previousCell;
    private void Awake()
    {
        placementValidationSystem = GetComponentInChildren<PlacementValidationSystem>();
        placementRotator = GetComponentInChildren<PlacementRotator>();
        placementPreview = GetComponentInChildren<PlacementPreview>();
    }
    private void Update()
    {
        Debug.Log(childCellHighLights.Count);
        if(GameManager.Instance.gameState == GameState.Building
            && Input.GetMouseButton(0)
            && !EventSystem.current.IsPointerOverGameObject()
            && childCellHighLights.Count > 0)
        {
            Debug.Log("Getting Input");
            originCellHighLight.SetActive(true);
            Vector3 mousePos = InputManager.Instance.GetSelectedCellPosition();
            /* If the currentCell is the same as the previous cell no need to check anymore*/
            if (previousCell == InputManager.Instance.CurrentCell) return;
            previousCell = InputManager.Instance.CurrentCell;

            SetBuildingPreviewOnSnappedPosition(mousePos);
            CheckingForFreeOccupiedCell();          
        }
        else if(GameManager.Instance.gameState != GameState.Building)
        {
            originCellHighLight.SetActive(false);
        }
    }
    private void SetBuildingPreviewOnSnappedPosition(Vector3 currentMousePosition) => 
        originCellHighLight.transform.position = InputManager.Instance.SnapHighLightCellGOBasedOnCurrentCellPos(currentMousePosition);
    public void SetCurrentBuilding(BuildingDataSO selectedBuilding)
    {
        currentBuildingData = selectedBuilding;
        //childCellHighLights = placementValidationSystem.InstantiateHighlightsBasedOnBuildingSize(selectedBuilding);
    }
    private void CheckingForFreeOccupiedCell()
    {
        if (placementValidationSystem.AreCellsValidForBuildMode(currentBuildingData, InputManager.Instance, placementRotator, placementPreview.GetHighLightingCells()))
        {
            ChangeCellHighLightMaterial(validMaterialForCellHighLight);
            /* Place buildings also set all the cells to Occupied*/
            allowedToBuild = true;
        }
        else
        {
            ChangeCellHighLightMaterial(invalidMaterialForCellHighLight);
            /* Can't build anything*/
            allowedToBuild = false;
        }
    }
    void ChangeCellHighLightMaterial(Material materialToChange)
    {
        foreach (var cellHighLightChild in childCellHighLights)
        {
            if (cellHighLightChild != null)
            {
                if (cellHighLightChild.TryGetComponent(out Renderer renderer))
                {
                    renderer.material = materialToChange;
                }
            }
        }
        if (!placementPreview.TransparentBuilding) return;
        List<Renderer> renderers = placementPreview.TransparentBuilding.GetAllRenderersInTheBuilding();
        foreach (var renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.material = materialToChange;
            }
        }
    }
    public void RotateBuildingButton()
    {
        /* Ref to placementRotator and change the current rotation of the building */
        placementRotator.IncreaseRotationByEnum();
        CheckingForFreeOccupiedCell();
        //GameObject highLightingOriginCell = placementPreview.GetHighLightngOriginCell();
        PlacementRotationDegrees placementRotationDegrees = placementRotator.GetPlacementRotationDegrees();

        float rotationAngle = 0f;

        // Calculate the rotation angle based on the placementRotationDegrees
        switch (placementRotationDegrees)
        {
            case PlacementRotationDegrees.RightDegree:
                rotationAngle = 90f;
                break;
            case PlacementRotationDegrees.DownDegree:
                rotationAngle = 180f;
                break;
            case PlacementRotationDegrees.LeftDegree:
                rotationAngle = 270f;
                break;
            case PlacementRotationDegrees.UpDegree:
                rotationAngle = 0;
                break;
            default:
                rotationAngle = 0f;
                break;
        }
        originCellHighLight.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
    }
    public void PlaceBuildingsOnNonOccupiedCellButton()
    {
        if (!allowedToBuild) return;
        //GameObject highLightingOriginCell = placementPreview.GetHighLightngOriginCell();
        Instantiate(currentBuildingData.BuildingPrefab, InputManager.Instance.CurrentCell.transform.position + currentBuildingData.BuildingPrefab.transform.position, originCellHighLight.transform.rotation);
        /* When placed down the building, the free cells need to update their occupation status */
        MapController.Instance.ProceduralTerrain.UpdateCellOccupationStatus(true, placementValidationSystem.CellsToUpdate());
        /*When placed down the building, check 1 more*/
        CheckingForFreeOccupiedCell();
    }
    public void GoIntoBuildModeButton()
    {
        //Should be whenever player go into building state and pick a new house
        //TODO this should change the gameManager state to building state
        /*Should always go backt to default rotation before getting new building preview*/
        placementRotator.SetPlacementRotationDegreesBackToDefault();
        childCellHighLights = placementPreview.InstantiateHighlightsBasedOnBuildingSize(currentBuildingData);
        placementPreview.InstantiateBuildingPreview(currentBuildingData);
        CheckingForFreeOccupiedCell();
    }
}
