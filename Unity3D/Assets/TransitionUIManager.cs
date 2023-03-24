using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionUIManager : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Transition(bool fadeIn)
    {
        animator.SetBool("PumpkinFadeIn", fadeIn);
    }
}
