using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is derived from Movement Base until I move the jump into 
/// a separate state
/// </summary>
[System.Serializable]
public class GroundedMovementController: MovementBase
{
    public bool isGrounded { get; set; } = false;
    public bool CanMove { get; set; } = true;
    public bool CanJump { get; set; } = true;

    protected float _jumpTimeoutDelta;
    protected float _fallTimeoutDelta;
    protected float _terminalVelocity = 53.0f;
    [Space]
    [Header("Jump Values:")]
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;
    private float JumpTimeout = 0.50f;
    private float FallTimeout = 0.15f;

    [Space]
    [Header("Ground Values:")]
    public float GroundedOffset = -0.14f;
    public float GroundedCheckRadius = 0.28f;
    public LayerMask GroundLayers;

    public enum MovementState { Stand, Crouch, Cling, Jump }
    public MovementState State { get; set; } = MovementState.Stand;

    public override float TargetSpeed { get; set; }
    [Space]
    public StandingController standingController;
    [Space]
    public CrouchController crouchController;
    [Space]
    public ClingController clingController;

    public new void Initialize(Animator animator, CharacterController controller, StarterAssetsInputs inputs, GameObject mainCam, CapsuleCollider collider)
    {
        this._animator = animator;
        this._controller = controller;
        this._input = inputs;
        this._mainCamera = mainCam;
        this.collider = collider;

        MovementBase[] controllers = new MovementBase[] { standingController, crouchController, clingController };
        foreach (MovementBase c in controllers)
            c.Initialize(animator, controller, inputs, mainCam, collider);
    }

    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;
            _animator.SetBool(animationIDs._animIDJump, false);
            _animator.SetBool(animationIDs._animIDFreeFall, false);

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f) _verticalVelocity = -2f;

            // Jump
            if (_input.jump && _jumpTimeoutDelta <= 0.0f && CanJump && CanMove)
            {
                if (_input.crouch) standingController.Stand();
                else
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    _animator.SetBool(animationIDs._animIDJump, true);
                }
            }
            else _input.jump = false;

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            _jumpTimeoutDelta = JumpTimeout;
            if (_fallTimeoutDelta >= 0.0f)
                _fallTimeoutDelta -= Time.deltaTime;
            else _animator.SetBool(animationIDs._animIDFreeFall, true);
            _input.jump = false;
        }
        if (_verticalVelocity < _terminalVelocity)
            _verticalVelocity += Gravity * Time.deltaTime;
        
        UpdateVerticalSpeed();
    }

    private void HandleState()
    {
        MovementState newState;
        
        bool crouch = _input.crouch;
        bool isClinging = clingController.isClinging;
        if (crouch)
        {
            crouchController.Crouch();
            if (isClinging) newState = MovementState.Cling;
            else newState = MovementState.Crouch;
        }
        else
        {
            standingController.Stand();
            newState = MovementState.Stand;
        }

        if (newState != State) ChangeStates(newState);
    }
    private void ChangeStates(MovementState newState)
    {
        GetState(State).Exit();
        GetState(newState).Enter();
        State = newState;
    }
    
    private MovementBase GetState(MovementState state)
    {
        switch (state)
        {
            case MovementState.Stand:
                return standingController;
            case MovementState.Crouch:
                return crouchController;
            case MovementState.Cling:
                return clingController;
            
            case MovementState.Jump:
            default: return null;
        }
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(PlayerManager.Instance.transform.position.x, PlayerManager.Instance.transform.position.y - GroundedOffset, PlayerManager.Instance.transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, GroundedCheckRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        _animator.SetBool(animationIDs._animIDGrounded, isGrounded);
    }

    public override void Move(bool allowCamRot)
    {
        clingController.CheckForClingObject();
        HandleState();
        UpdateSpeedVals();
        GetState(State).Move(allowCamRot);
        GroundedCheck();
        JumpAndGravity();
    }

    private void UpdateSpeedVals()
    {
        MovementBase activeController = GetState(State);
        TargetSpeed = activeController.TargetSpeed;
        _speed = activeController._speed;
        _speedX = activeController._speedX;
        _speedZ = activeController._speedZ;
        _rotationVelocity = activeController._rotationVelocity;
        _verticalVelocity = activeController._verticalVelocity;
        _targetRotation = activeController._targetRotation;

        MovementBase[] controllers = new MovementBase[] { standingController, crouchController, clingController };
        foreach(MovementBase controller in controllers)
            controller.Update(_speed, _speedX, _speedZ, _rotationVelocity, _verticalVelocity, _targetRotation);
    }
    private void UpdateVerticalSpeed()
    {
        MovementBase[] controllers = new MovementBase[] { standingController, crouchController, clingController };
        foreach (MovementBase controller in controllers)
            controller._verticalVelocity = this._verticalVelocity;
    }

    #region Ignore
    protected override float GetTargetSpeed(float inputMagnitude) => TargetSpeed;

    public override void Enter() { }

    public override void Exit() { }

    #endregion
}
