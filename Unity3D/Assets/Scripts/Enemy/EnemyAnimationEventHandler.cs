using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    Animator animator;
    public int standUpDelay = 1;

    public event Action OnAttack;
    public event Action OnSwing;
    public event Action OnStepL;
    public event Action OnStepR;
    public event Action OnGrunt;
    public event Action OnEndDamage;
    public event Action OnSurprise;

    private void Start() => animator = GetComponent<Animator>();

    public void InvokeAttack() => OnAttack?.Invoke();
    public void InvokeSwing() => OnSwing?.Invoke();
    public void InvokeStepL() => OnStepL?.Invoke();
    public void InvokeStepR() => OnStepR?.Invoke();
    public void InvokeGrunt() => OnGrunt?.Invoke();
    public void InvokeEndDamage() => OnEndDamage?.Invoke();
    public void InvokeSurpise()
    {
        OnSurprise?.Invoke();
    } 
    public IEnumerator InvokeFallOver()
    {
        yield return new WaitForSeconds(standUpDelay);
        animator.SetBool("isFallOver", false);
        animator.SetBool("isDamaged", false);
    }

}