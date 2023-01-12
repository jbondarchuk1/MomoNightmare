using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    Animator animator;

    public delegate void Attack();
    public event Attack OnAttack;

    public delegate void Swing();
    public event Swing OnSwing;

    public delegate void StepL();
    public event StepL OnStepL;

    public delegate void StepR();
    public event StepR OnStepR;

    public delegate void Grunt();
    public event Grunt OnGrunt;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void InvokeAttack() => OnAttack?.Invoke();
    public void InvokeSwing() => OnSwing?.Invoke();
    public void InvokeStepL() => OnStepL?.Invoke();
    public void InvokeStepR() => OnStepR?.Invoke();
    public void InvokeGrunt() => OnGrunt?.Invoke();
}
