using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class CreationSystem : MonoBehaviour
{
    [SerializeField] SOPrototype currentSOPrototype;
    [SerializeField] GameObject mouseHighLight;
    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Creating && !EventSystem.current.IsPointerOverGameObject())
        {
            mouseHighLight.SetActive(true);
            SnapCurrentMousePosistionToCurrentCellPosition(mouseHighLight.transform);
            if (Input.GetMouseButtonDown(0))
            {
                InstantiatePrototypeBasedOnCurrentSituation(currentSOPrototype);
            }
        }
        else
        {
            mouseHighLight.SetActive(false);
        }
    }
    private void SnapCurrentMousePosistionToCurrentCellPosition(Transform mouseHighLightPosition)
    {
        /* Get first, snap later */
        if (mouseHighLightPosition == null) return;
        Vector3 mousePos = InputManager.Instance.GetSelectedCellPosition();
        Vector3 snappedPos = InputManager.Instance.SnapHighLightCellGOBasedOnCurrentCellPos(mousePos);
        mouseHighLightPosition.position = snappedPos;

    }

    private void InstantiatePrototypeBasedOnCurrentSituation(SOPrototype prototype)
    {
        Vector2Int currentCellInGridPosition = InputManager.Instance.GetCellInGridPosition(InputManager.Instance.CurrentCell);
        if (currentCellInGridPosition == null) return;
        var mapGrid = MapController.Instance.ProceduralTerrain.GetMapGrid();
        if (mapGrid[currentCellInGridPosition.x, currentCellInGridPosition.y].IsThisCellOccupied()) return;
        if (mapGrid[currentCellInGridPosition.x, currentCellInGridPosition.y].cellType != CellType.Grass) return;
        var spawnedPrototype = Instantiate(currentSOPrototype.BuildingPrefab, mouseHighLight.transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
    }
}
