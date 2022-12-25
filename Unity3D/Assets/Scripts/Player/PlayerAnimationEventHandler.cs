using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimationEventHandler : MonoBehaviour
{
    private Animator animator;
    public delegate void ShootEvent();
    public static event ShootEvent OnShoot;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Shoot()
    {
        OnShoot.Invoke();
    }

    public void ReceiveAttack()
    {
        animator.SetBool("isAttacked", false);
    }

}
