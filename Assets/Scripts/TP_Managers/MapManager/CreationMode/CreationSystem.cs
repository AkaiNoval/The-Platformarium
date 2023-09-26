using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreationSystem : MonoBehaviour
{
    [SerializeField] SOPrototype currentSOPrototype;
    [SerializeField] GameObject mouseHighLight;
    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Creating && !EventSystem.current.IsPointerOverGameObject())
        {
            mouseHighLight.SetActive(true);
            Debug.Log("Time for world creating mode");
            SnapCurrentMousePosistionToCurrentCellPosition(mouseHighLight.transform);
            if (Input.GetMouseButtonDown(0))
            {
                InstantiatePrototypeBasedOnCurrentSituation(currentSOPrototype);
            }
        }
    }
    private void SnapCurrentMousePosistionToCurrentCellPosition(Transform mouseHighLightPosition)
    {
        /* Get first, snap later */
        Vector3 mousePos = InputManager.Instance.GetSelectedCellPosition();
        Vector3 snappedPos = InputManager.Instance.SnapHighLightCellGOBasedOnCurrentCellPos(mousePos);
        mouseHighLightPosition.position = snappedPos;

    }

    private void InstantiatePrototypeBasedOnCurrentSituation(SOPrototype prototype)
    {
        var spawnedPrototype = Instantiate(currentSOPrototype.BuildingPrefab, mouseHighLight.transform.position, Quaternion.identity);
    }
}
