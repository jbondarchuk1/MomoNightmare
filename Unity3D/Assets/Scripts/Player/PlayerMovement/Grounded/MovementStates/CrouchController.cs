using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrouchController: MovementBase
{
    [field: SerializeField] public override float TargetSpeed { get; set; }
    [Header("Crouch:")]
    [SerializeField] private float crouchingDownSpeed = 5f;
    [SerializeField] private float crouchingHeight = 1;
    [SerializeField] private float crouchingOffset = .1f;
    [SerializeField] private Transform playerMesh;

    public override void Move(bool allowCamRot)
    {
        Crouch();
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        HandleRotation(allowCamRot);
        HandleSpeed(inputMagnitude);
        SetControllerMotion();
        SetAnimations();
    }

    protected override float GetTargetSpeed(float inputMagnitude)
    {
        float targetSpeed = TargetSpeed;
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;
        return targetSpeed;
    }

    public void Crouch()
    {
        collider.height = crouchingHeight;

        Vector3 playerMeshPos = playerMesh.localPosition;
        playerMeshPos.y = crouchingOffset;
        _controller.height = Mathf.Lerp(_controller.height, crouchingHeight, Time.deltaTime);
        playerMesh.localPosition = Vector3.Lerp(playerMesh.localPosition, playerMeshPos, Time.deltaTime);

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
