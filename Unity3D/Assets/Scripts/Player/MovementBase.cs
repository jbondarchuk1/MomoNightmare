using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBase : MonoBehaviour
{
    public abstract bool contactClingObject { get; set; }



    public Transform cameraObject;
    // protected InputManager inputManager;
    protected CapsuleCollider collider;
    protected PlayerStats stats;
    protected CharacterController _controller;
    public Animator _animator;

    // animation IDs
    protected int _animIDSpeed;
    protected int _animIDGrounded;
    protected int _animIDJump;
    protected int _animIDFreeFall;
    protected int _animIDVelocityZ;
    protected int _animIDCrouch;


    // [Header("State Bools")]
    [HideInInspector] public bool Grounded;
    [HideInInspector] public bool canSprint = true;
    [HideInInspector] public bool canCrouch = true;
    [HideInInspector] public bool canJump = true;
    [HideInInspector] public bool isCrouching;
    [HideInInspector] public bool isAiming = false;
    [HideInInspector] public bool lastTab = true; // false - left, true - right

    [Header("Movement Speeds")]
    public float SprintSpeed;
    public float MoveSpeed;
    public float CrouchSpeed;
    public float RotationSmoothTime;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    
    // Jump and grav
    // timeout deltatime
    protected float _jumpTimeoutDelta;
    protected float _fallTimeoutDelta;

    // player
    public float _speed;
    protected float _animationBlend;
    protected float _targetRotation = 0.0f;
    protected float _rotationVelocity;
    protected float _verticalVelocity;
    protected float _terminalVelocity = 53.0f;


    [Header("Crouch Position")]
    public float standingHeight = 1.57f;
    public float crouchingHeight = 1;
    public float standingUpSpeed = 5f;
    public float crouchingDownSpeed = 5f;
    public float turnSmoothTime;
    public float turnSmoothVelocity;

    [Header("Jump and Gravity")]
    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;
    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;


    [Header("Player Grounded")]
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    public CinemachineVirtualCamera aimCam;
    public LayerMask aimColliderLayerMask;


    // cinemachine
    protected float _cinemachineTargetYaw;
    protected float _cinemachineTargetPitch;
    protected bool allowCameraRotation;

    protected StarterAssetsInputs _input;
    protected GameObject _mainCamera;
    protected const float _threshold = 0.01f;


    /// <summary>
    /// 
    /// ABSTRACT
    /// 
    /// </summary>
    public abstract void Move();
    public abstract void Crouch();





    protected void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        stats = GetComponent<PlayerStats>();
        _controller = GetComponent<CharacterController>();

        if (_mainCamera == null)
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    protected void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _input = GetComponent<StarterAssetsInputs>();
        AssignAnimationIDs();
    }
    protected void LateUpdate()
    {
        HandleCamera();
    }


    protected void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("isJumping");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDVelocityZ = Animator.StringToHash("VelocityZ");
        _animIDCrouch = Animator.StringToHash("isCrouching");
    }

    public void HandleCamera()
    {
        //// if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            _cinemachineTargetYaw += _input.look.x * Time.deltaTime;
            _cinemachineTargetPitch += _input.look.y * Time.deltaTime;
        }


        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    protected static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
