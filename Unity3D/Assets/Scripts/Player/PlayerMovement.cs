using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameraObject;
    // private InputManager inputManager;
    private CapsuleCollider collider;
    private PlayerStats stats;
    private CharacterController _controller;
    public Animator _animator;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDVelocityZ;
    private int _animIDCrouch;
    

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
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // player
    public float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;


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
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private bool allowCameraRotation;

    private StarterAssetsInputs _input;
    private GameObject _mainCamera;
    private const float _threshold = 0.01f;

    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        stats = GetComponent<PlayerStats>();
        _controller = GetComponent<CharacterController>();

        if (_mainCamera == null)
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _input = GetComponent<StarterAssetsInputs>();
        AssignAnimationIDs();
    }
    private void LateUpdate()
    {
        HandleCamera();
    }

    private void Update()
    {
        HandleAllMovement();
    }

    public void HandleAllMovement()
    {
        GroundedCheck();
        JumpAndGravity();
        Move();
        HandleCrouch();
        Aim();

    }

    private void AssignAnimationIDs()
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

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = (_input.sprint && canSprint) ? SprintSpeed : MoveSpeed;
        if (isCrouching)
            targetSpeed = CrouchSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            if (allowCameraRotation) transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        _animator.SetFloat(_animIDSpeed, _speed);
        _animator.SetBool(_animIDCrouch, isCrouching);
    }


    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;
            _animator.SetBool(_animIDJump, false);
            _animator.SetBool(_animIDFreeFall, false);

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
                _verticalVelocity = -2f;

            // Jump
            if (_input.jump && _jumpTimeoutDelta <= 0.0f && canJump)
            {
                if (isCrouching)
                    Stand();
                else
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    _animator.SetBool(_animIDJump, true);
                }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
                _jumpTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
                _fallTimeoutDelta -= Time.deltaTime;
            else
                _animator.SetBool(_animIDFreeFall, true);

            // if we are not grounded, do not jump
            _input.jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void HandleCrouch()
    {
        bool crouch = _input.crouch;
        if (crouch) Crouch();
        else Stand();
    }

    private void Crouch()
    {
        if (collider.height > crouchingHeight)
        {
            Vector3 crouchingCenterVector = new Vector3(collider.center.x, 0.65f, collider.center.z);
            collider.center = Vector3.Lerp(collider.center, crouchingCenterVector, crouchingDownSpeed * Time.deltaTime);
            collider.height = Mathf.Lerp(collider.height, crouchingHeight, crouchingDownSpeed * Time.deltaTime);
        }
        isCrouching = true;
    }
    private void Stand()
    {
        if (collider.height < 1.6f)
        {
            Vector3 standingCenterVector = new Vector3(collider.center.x, 1.05f, collider.center.z);
            collider.center = Vector3.Lerp(collider.center, standingCenterVector, standingUpSpeed * Time.deltaTime);
            collider.height = Mathf.Lerp(collider.height, 1.85f, crouchingDownSpeed * Time.deltaTime);
        }
        isCrouching = false;
        _input.crouch = false;
    }


    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);


        _animator.SetBool(_animIDGrounded, Grounded);

    }

    private void Aim()
    {
        Vector3 mouseWorldPos = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            mouseWorldPos = hit.point;
        }


        if (_input.mouseR)
        {
            isAiming = true;
            allowCameraRotation = false;
            aimCam.gameObject.SetActive(true);
            Vector3 worldAimTarget = mouseWorldPos;
            worldAimTarget.y = transform.position.y;

            lastTab = _input.tab;
            float toVal = lastTab ? 1f : 0f;

            Cinemachine3rdPersonFollow follow = aimCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            float currVal = follow.CameraSide;
            follow.CameraSide = Mathf.Lerp(currVal, toVal, .3f);



            Vector3 aimDirection = (worldAimTarget - transform.position);
            transform.forward = Vector3.Lerp(transform.forward, aimDirection.normalized, Time.deltaTime * 20);

            // handle the spot light on the character
            //cameraLight.intensity = 30f;
        }
        else
        {
            isAiming = false;
            allowCameraRotation = true;
            aimCam.gameObject.SetActive(false);
            //cameraLight.intensity = 57.41f;
        }
    }

}
