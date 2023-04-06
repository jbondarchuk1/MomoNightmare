using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimationEventHandler : MonoBehaviour
{
    private Animator animator;
    private AudioManager audioManager;

    public delegate void ShootEvent();
    public static event ShootEvent OnShoot;

    public delegate void StepLEvent();
    public static event StepLEvent OnStepL; 
    
    public delegate void StepREvent();
    public static event StepREvent OnStepR;

    public delegate void ReceiveAttackEvent();
    public static event ReceiveAttackEvent OnReceiveAttack;



    private void Start()
    {
        animator = GetComponent<Animator>();
        audioManager = PlayerManager.Instance.audioManager;
    }
    public void Shoot() => OnShoot?.Invoke();
    public void StepL() => OnStepL?.Invoke();
    public void StepR() => OnStepL?.Invoke();

    private void OnDestroy()
    {
        OnShoot = null;
        OnStepL = null;
        OnStepR = null;
        OnReceiveAttack = null;
    }
    // Single Instance Methods
    public void ReceiveAttack()
    {
        animator.SetBool("isAttacked", false);
        OnReceiveAttack?.Invoke();
    } 
    public void Swing()
    {
        audioManager.PlaySound("Swoosh");
        audioManager.PlaySound("Grunt", "Attack");
    }
}
