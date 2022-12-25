using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour, ICharacter
{
    Animator animator;
    int isAttackingHash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isAttackingHash = Animator.StringToHash("isAttacking");
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {

    }


}
