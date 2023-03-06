using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StandingController: MovementBase
{
    [field: SerializeField] public override float TargetSpeed { get; set; }
    [field: SerializeField] public float SprintSpeed { get; set; }
    [HideInInspector] public bool canSprint = false;

    [SerializeField] private float standingHeight = 1.85f;
    [SerializeField] private float standingUpSpeed = 5f;
    [SerializeField] private float standingUpOffset = 0.16f;
    [SerializeField] private Transform playerMesh;

    public override void Move(bool allowCamRot)
    {
        CheckStamina();
        Stand();
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        if (isGrounded) HandleRotation(allowCamRot);
        HandleSpeed(inputMagnitude);
        SetControllerMotion();
        SetAnimations();
        HandleSounds();
    }

    protected override float GetTargetSpeed(float inputMagnitude)
    {
        float targetSpeed = (_input.sprint && canSprint) ? SprintSpeed : TargetSpeed;
        
        if (_input.move == Vector2.zero && isGrounded) 
            return 0.0f;
        if (!isGrounded) targetSpeed = _speed;
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
        collider.height = standingHeight;
        Vector3 playerMeshPos = playerMesh.localPosition;
        playerMeshPos.y = standingUpOffset;
        playerMesh.localPosition = Vector3.Lerp(playerMesh.localPosition, playerMeshPos, Time.deltaTime);
        _controller.height = Mathf.Lerp(_controller.height, standingHeight, Time.deltaTime);

        _input.crouch = false;
    }

    private void CheckStamina()
    {
        canSprint &= !PlayerManager.Instance.statManager.rechargingStamina;
    }

    public override void Enter() { }

    public override void Exit() 
    {
        AudioManager am = PlayerManager.Instance.audioManager;
        am.StopSound("Breath");
    }
}
