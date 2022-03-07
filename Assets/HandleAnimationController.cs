using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimationController : MonoBehaviour
{
    private AnimationState animState = AnimationState.Default;

    public AnimationState AnimState
    {
        get => animState;
        set
        {
            // Set the new state as well as deleting the old state
            if (animState != value)
            {
                animator.SetBool(animState.ToString(), false);
                animator.SetBool(value.ToString(), true);
                animState = value;
            }
        }
    }

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
}
