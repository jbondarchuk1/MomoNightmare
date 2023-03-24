using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyCharacter : MonoBehaviour, ICharacter
{
    ThirdPersonCharacter character;
    Animator animator;
    int isAttackingHash;

    public bool IsGrounded { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isAttackingHash = Animator.StringToHash("isAttacking");
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {

    }


}
