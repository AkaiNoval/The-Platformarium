using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeAnimController : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator= GetComponent<Animator>();
    }
    public void PlayAnimation(AnimKey animKeyToPlay)
    {
        string animNameToPlay = animKeyToPlay.ToString();
        animator.SetTrigger(animNameToPlay);
    }
}
public enum AnimKey
{
    Walking,
    Chopping,
    Dancing,
    Sleeping
}
