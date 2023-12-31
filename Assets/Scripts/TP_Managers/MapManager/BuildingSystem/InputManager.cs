using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] Camera mainCam;

    /* Layer mask for raycasting */
    [SerializeField] LayerMask placementLayerMask;

    /* Last position where a valid raycast hit occurred */
    Vector3 lastPosition;
    [field: SerializeField] public Cell CurrentCell { get; private set; }
    public Vector3 GetSelectedCellPosition()
    {
        /* Get the current mouse position in screen coordinates */
        Vector3 mousePos = Input.mousePosition;

        /* Adjust mouse position's z-coordinate to the near clipping plane of the camera */
        /* Cannot select object that are not rendered by camera */
        mousePos.z = mainCam.nearClipPlane;

        /* Create a ray from the camera through the adjusted mouse position */
        Ray ray = mainCam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, placementLayerMask))
        {
            lastPosition = hit.point;
            if (hit.collider.gameObject.TryGetComponent(out Cell rayHitCell))
            {
                CurrentCell = rayHitCell;
            }
        }
        return lastPosition;
    }
    public Vector2Int GetCellInGridPosition(Cell currentCell)
    {
        ProceduralTerrainGenerator mapData = MapController.Instance.ProceduralTerrain;
        for (int x = 0; x < mapData.GetMapGrid().GetLength(0); x++)
        {
            for (int y = 0; y < mapData.GetMapGrid().GetLength(1); y++)
            {
                if (mapData.GetMapGrid()[x, y] == currentCell)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        /* If the cell is not found in the grid, return an invalid position*/
        return new Vector2Int(-1, -1);
    }
    public Vector3 SnapHighLightCellGOBasedOnCurrentCellPos(Vector3 mousePosition)//TODO: Refactor this based on current cell position
    {
        /* Snap the cellHighLight gameObject based on mouse position*/
        Vector3 snappedPosition = CurrentCell.transform.position;    
        return snappedPosition += new Vector3(0, 1.01f, 0);
    }

}
