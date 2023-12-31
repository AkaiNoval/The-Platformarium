using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UAction : ScriptableObject
{
    public string Name;
    /*  An array of considerations that influence the overall score of the action in the context of the world. 
     *  Each consideration represents a factor that affects the desirability or suitability of the action. */
    public UConsideration[] considerations;
    /* Priority determines the importance of this consideration in the overall score */
    public float Priority;
    /* Keep track how urgent an action is */
    private float score;
    /* Score should alway be clamped at 0-1*/
    public float Score { get => score; set => score = Mathf.Clamp01(value); }
    public AnimKey animKey;
    //public Transform RequiredDestination { get; protected set; }
    public virtual void Awake() => score = 0;

    public abstract void Execute<T>(T colonist) where T : PrototypeController;
    public abstract void SetRequiredDestination<T>(T colonist) where T : PrototypeController;

    protected float GetPathLength(NavMeshPath path)
    {
        float pathLength = 0.0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return pathLength;
    }
}
