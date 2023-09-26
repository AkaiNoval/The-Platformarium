using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UConsideration : ScriptableObject
{
    public string Name;

    /* Weight determines the importance of this consideration in the overall score */
    public float Weight;
    [TextArea]
    public string Description;
    /* Keep track how urgent an action is */
    private float score;

    /* Score should alway be clamped at 0-1*/
    public float Score { get => score; set => score = Mathf.Clamp01(value); }
    public virtual void Awake() => score = 0;

    /* For getting score based on conditions */
    /* Every considerations are scored differently */
    public abstract float ScoreConsideration(ColonistController colonist);
}
