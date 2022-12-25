using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    Animator animator;

    public delegate void Attack();
    public event Attack OnAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void InvokeAttack()
    {
        Debug.Log("Invoking Attack");
        OnAttack?.Invoke();
    }
}
