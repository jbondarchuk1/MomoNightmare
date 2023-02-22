using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrouchController: MovementBase
{
    [field: SerializeField] public override float TargetSpeed { get; set; }
    [Header("Crouch:")]
    public float crouchingDownSpeed = 5f;
    public float crouchingHeight = 1;

    public override void Move(bool allowCamRot)
    {
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        Crouch();
        HandleRotation(allowCamRot);
        HandleSpeed(inputMagnitude);
        SetControllerMotion();
        SetAnimations();
    }

    // overrides base method
    protected override float GetTargetSpeed(float inputMagnitude)
    {
        float targetSpeed = TargetSpeed;
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;
        return targetSpeed;
    }

    public void Crouch()
    {
        if (collider.height > crouchingHeight)
        {
            Vector3 crouchingCenterVector = new Vector3(collider.center.x, 0.65f, collider.center.z);
            collider.center = Vector3.Lerp(collider.center, crouchingCenterVector, crouchingDownSpeed * Time.deltaTime);
            collider.height = Mathf.Lerp(collider.height, crouchingHeight, crouchingDownSpeed * Time.deltaTime);
        }
        _input.crouch = true;
    }

    public new void SetAnimations()
    {
        base.SetAnimations();
    }

    public override void Enter()
    {
        _animator.SetBool(animationIDs._animIDCrouch, true);
    }

    public override void Exit()
    {
        _animator.SetBool(animationIDs._animIDCrouch, false);

    }
}
