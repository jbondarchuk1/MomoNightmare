using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AimController;
using static GroundedMovementController;

public class PlayerMovement : MonoBehaviour
{
    #region MovementBools
    [HideInInspector] public bool isAiming = false;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canSprint = true;
    [HideInInspector] public bool canCrouch = true;
    [HideInInspector] public bool canJump = true;

    [HideInInspector] public bool lastTab = true; // false - left, true - right

    #endregion MovementBools
    #region AllControllers
    StarterAssetsInputs _input;
    new CapsuleCollider collider;
    CharacterController _controller;
    [HideInInspector] public Animator _animator;
    GameObject mainCamera;
    #endregion

    [Space]
    public AimController _aimController;
    [Space]
    public GroundedMovementController _groundedMovementController;

    #region Monobehaviour Methods
    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        _controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _input = StarterAssetsInputs.Instance;
        mainCamera = PlayerManager.Instance.camera.gameObject;

        _aimController.Initialize(_input, _animator);
        _groundedMovementController.Initialize(_animator, _controller, _input, mainCamera, collider);
    }

    private void LateUpdate()
    {
        HandleCamera();
    }
    private void Update()
    {
        Aim();
        Move();
        _groundedMovementController.canMove = this.canMove;
        _aimController.CanLook = this.canMove;
        HandleBools();
    }
    private void HandleBools()
    {
        AimState currState = GetAimState();
        isAiming = currState == AimState.Zoom || currState == AimState.FirstPerson;
        canSprint &= !isAiming
             && _groundedMovementController.State != MovementState.Crouch
             && _groundedMovementController.State != MovementState.Cling;
        canJump = canSprint || _groundedMovementController.State == MovementState.Crouch;

        _groundedMovementController.standingController.canSprint = canSprint;
        _groundedMovementController.CanJump = canJump;
    }
    #endregion

    private void Aim() => _aimController.Aim(GetAimState());
    private void HandleCamera() => _aimController.HandleCamera();
    private void Move() => _groundedMovementController.Move(_aimController.AllowCameraRotation);

    private AimState GetAimState()
    {
        AimState aimState = AimState.ThirdPerson;

        if (_input.mouseR && canMove)
        {
            if (_groundedMovementController.State == MovementState.Stand)
                aimState = AimState.Zoom;
        }

        return aimState;
    }
}

public class AnimationIDContainer
{
    public int _animIDSpeed { get; set; } = Animator.StringToHash("Speed");
    public int _animIDGrounded { get; set; } = Animator.StringToHash("Grounded");
    public int _animIDJump { get; set; } = Animator.StringToHash("isJumping");
    public int _animIDFreeFall { get; set; } = Animator.StringToHash("FreeFall");
    public int _animIDSpeedX { get; set; } = Animator.StringToHash("SpeedX");
    public int _animIDSpeedZ { get; set; } = Animator.StringToHash("SpeedZ");
    public int _animIDCrouch { get; set; } = Animator.StringToHash("isCrouching");
    public int _animIDADS { get; set; } = Animator.StringToHash("isADS");
}