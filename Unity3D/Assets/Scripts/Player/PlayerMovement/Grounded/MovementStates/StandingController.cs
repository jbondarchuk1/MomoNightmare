using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StandingController: MovementBase
{
    [field: SerializeField] public override float TargetSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [HideInInspector] public bool canSprint = false;
    public float standingHeight = 1.85f;
    public float standingUpSpeed = 5f;

    public override void Move(bool allowCamRot)
    {
        CheckStamina();
        Stand();
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        HandleRotation(allowCamRot);
        HandleSpeed(inputMagnitude);
        SetControllerMotion();
        SetAnimations();
        HandleSounds();
    }

    protected override float GetTargetSpeed(float inputMagnitude)
    {
        float targetSpeed = (_input.sprint && canSprint) ? SprintSpeed : TargetSpeed;
        
        if (_input.move == Vector2.zero) 
            return 0.0f;
        return targetSpeed;
    }

    private void HandleSounds()
    {
        AudioManager am = PlayerManager.Instance.audioManager;
        if (_speed == SprintSpeed) am.FadeToSound("Breath", "Running", .2f);
        else am.StopSound("Breath");
    }

    public void Stand()
    {
        if (collider.height < 1.6f)
        {
            Vector3 standingCenterVector = new Vector3(collider.center.x, 1.05f, collider.center.z);
            collider.center = Vector3.Lerp(collider.center, standingCenterVector, standingUpSpeed * Time.deltaTime);
            collider.height = Mathf.Lerp(collider.height, standingHeight, standingUpSpeed * Time.deltaTime);
        }
        _input.crouch = false;
    }

    private void CheckStamina()
    {
        canSprint &= !PlayerManager.Instance.statManager.rechargingStamina;
    }

    public override void Enter() { }

    public override void Exit() { }
}
