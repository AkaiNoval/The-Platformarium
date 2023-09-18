using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlacementRotationDegrees
{
    /* Represents the 0-degree rotation (North or upward).*/
    UpDegree,
    /* Represents the 90-degree rotation (East or to the right).*/
    RightDegree,
    /* Represents the 180-degree rotation (South or downward).*/
    DownDegree,
    /* Represents the 270-degree rotation (West or to the left).*/
    LeftDegree

}
public class PlacementRotator : MonoBehaviour
{
    [SerializeField] PlacementRotationDegrees placementRotationDegree;
    public PlacementRotationDegrees GetPlacementRotationDegrees() => placementRotationDegree;
    public void SetPlacementRotationDegreesBackToDefault() => placementRotationDegree = PlacementRotationDegrees.UpDegree;
    public void IncreaseRotationByEnum()
    {
        int numberOfDegrees = Enum.GetValues(typeof(PlacementRotationDegrees)).Length;
        int currentDegree = (int)placementRotationDegree;
        currentDegree = (currentDegree + 1) % numberOfDegrees; // Increment and wrap around

        placementRotationDegree = (PlacementRotationDegrees)currentDegree;
    }
   
}
