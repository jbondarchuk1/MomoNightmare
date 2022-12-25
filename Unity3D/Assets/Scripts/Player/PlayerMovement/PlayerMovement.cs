using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovementBase
{
    public override bool contactClingObject { get; set; } = false;
    private Vector3 clingRotation = Vector3.zero;
    private Ray ray;
    [Range(0, 2)] public float checkDistance = 1f;
    // private Transform Pl


    private ClingObject nearestClingObject = null;

    private new void Awake()
    {
        base.Awake();
    }
    private new void Start()
    {
        base.Start();
    }
    private new void LateUpdate()
    {
        base.LateUpdate();
    }
    private void Update()
    {
        HandleAllMovement();
    }

    public void HandleAllMovement()
    {
        GroundedCheck();
        HandleCrouch();
        Aim();
        Move();
        JumpAndGravity();
    }

    private void CheckForClingObject()
    {
        Vector3 pos = transform.position;
        pos.y += 1;
        ray = new Ray(pos, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out RaycastHit hit, .5f, LayerMask.GetMask("Obstruction")))
        {
            Debug.DrawRay(pos, hit.normal, Color.red, Mathf.Infinity);
            if (hit.transform.gameObject.TryGetComponent(out ClingObject co))
            {
                nearestClingObject = co;
                // FUNCTIONAL PART
                if (isCrouching)
                {
                    contactClingObject = true;
                    // assume a localRotation of 0 compared to parent
                    clingRotation = hit.transform.rotation.eulerAngles;
                }
            }
        }
    }

    private bool CheckForStop()
    {
        return nearestClingObject.MinDistanceToStop(transform) < checkDistance;
    }

    private void HandleSpeed(float inputMagnitude)
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = (_input.sprint && canSprint) ? SprintSpeed : MoveSpeed;
        
        if (isCrouching) targetSpeed = CrouchSpeed;
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else _speed = targetSpeed;


        _speedX = Mathf.Lerp(_speedX, _input.move.x, Time.deltaTime * SpeedChangeRate);
        _speedZ = Mathf.Lerp(_speedZ, _input.move.y, Time.deltaTime * SpeedChangeRate);
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

        AudioManager am = PlayerManager.Instance.audioManager;
        if (_speed == SprintSpeed) am.Play("Breath", "Running");
        else am.Stop("Breath","Running");
    }

    public override void Move()
    {
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        CheckForClingObject();
        if (contactClingObject)
            contactClingObject = !CheckForStop();

        if (contactClingObject)  inputMagnitude = MoveInCling(_input.move.normalized, inputMagnitude);
        else DisableClingAnimations();
        
        HandleSpeed(inputMagnitude);

        if (_input.move != Vector2.zero)
        {
            if (!contactClingObject && !isAiming) allowCameraRotation = true;

            if (!contactClingObject)
            {
                Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            }

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            

            if (allowCameraRotation) transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        Vector3 motion = targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
        
        _controller.Move(motion);

        _animator.SetFloat(_animIDSpeed,  _speed);
        _animator.SetFloat(_animIDSpeedX, _speedX);
        _animator.SetFloat(_animIDSpeedZ, _speedZ);
        _animator.SetBool(_animIDCrouch,  isCrouching);
    }

    // if the angle is positive, player is on the left  and the rotation the player moves in is rotationL
    // if the angle is negative, player is on the right and the rotation the player moves in is rotationR

    // the speed the player should be moving in that direction is determined by abs(angle).
    // An angle of 0 should not move at all
    // an angle of 180 should move 100%
    private float MoveInCling(Vector2 inputDirection, float inputMagnitude)
    {
        Vector3 rotation = clingRotation;

        Quaternion rotationL = Quaternion.Euler(rotation.x, rotation.y - 90.1f, rotation.z);
        Quaternion rotationR = Quaternion.Euler(rotation.x, rotation.y + 90.1f, rotation.z);


        inputDirection.Normalize();
        float rotationOfMovement = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
        if (rotationOfMovement < 0f) rotationOfMovement += 360;
        float angle = rotationOfMovement - rotation.y;
        if (angle < 0f) angle += 360;


        Quaternion mainRotation;

        if (angle > 5 && angle < 180)
        {
            mainRotation = rotationR;
            HandleClingAnimations(0);
        }
        else if (angle > 180)
        {
            mainRotation = rotationL;
            HandleClingAnimations(1);
        }
        else
        {
            contactClingObject = false;
            HandleClingAnimations();
            return 0;
        }


        Vector3 euler = mainRotation.eulerAngles;
        mainRotation.eulerAngles = euler;
        if (inputMagnitude > 0) _targetRotation = mainRotation.eulerAngles.y;
        if (!isCrouching)
        {
            contactClingObject = false;
            return 0;
        }


        return inputMagnitude;
    }

    /// <summary>
    /// Parameter 0 = R
    /// Parameter 1 = L
    /// any other parameter = no cling (default of -1)
    /// </summary>
    private void HandleClingAnimations(int clingStatus = -1, int location = -1)
    {
        switch (clingStatus)
        {
            case 0:
                _animator.SetBool("isCling", true);
                _animator.SetBool("isClingR", true);
                break;
            case 1:
                _animator.SetBool("isCling", true);
                _animator.SetBool("isClingR", false);
                break;
            default:
                DisableClingAnimations();
                break;
        }
    }
    private void DisableClingAnimations()
    {
        _animator.SetBool("isCling", false);
        _animator.SetBool("isClingR", false);
    }
    private void ResetModelRotation()
    {
        // TODO: implement
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
            else _input.jump = false;

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

    public override void Crouch()
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

    /// <summary>
    /// Handles the right click to 3rd person shooter aiming function.
    /// When the user right clicks, they aim, another right click  will go back to the normal camera view.
    /// </summary>
    private void Aim()
    {
        Vector3 mouseWorldPos = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            mouseWorldPos = hit.point;
        }

        if (_input.mouseR && !isCrouching)
        {
            isAiming = true;
            canSprint = false;
            canJump = false;
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
        }
        else
        {
            isAiming = false;
            canJump = true;
            canSprint = true;
            allowCameraRotation = true;
            aimCam.gameObject.SetActive(false);
        }
        _animator.SetBool(_animIDADS, isAiming);
    }
}
