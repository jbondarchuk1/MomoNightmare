//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerClingManager : MovementBase
//{
//    public ClingObject clingObject;
//    public override bool contactClingObject { get; set; } = true;



//    private new void Awake()
//    {
//        base.Awake();
//    }
//    private new void Start()
//    {
//        base.Start();
//    }
//    private new void LateUpdate()
//    {
//        base.LateUpdate();
//    }
//    private void Update()
//    {
//        HandleAllMovement();
//    }

//    public void HandleAllMovement()
//    {
//        Move();
//    }

//    public override void Move()
//    {
//        // SPEED - sprint, walk, crouch
//        float targetSpeed = (_input.sprint && canSprint) ? SprintSpeed : MoveSpeed;
//        if (isCrouching) targetSpeed = CrouchSpeed;
//        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

//        // COPIED
//        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
//        float speedOffset = 0.1f;
        
//        // MAGNITUDE OF THE INPUT
//        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        
//        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
//        {
//            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
//            _speed = Mathf.Round(_speed * 1000f) / 1000f;
//        }
//        else _speed = targetSpeed;
        
//        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
//        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

//        if (_input.move != Vector2.zero)
//        {
//            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
//            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
//            if (allowCameraRotation) transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
//        }

//        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
//        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
//        _animator.SetFloat(_animIDSpeed, _speed);
//        _animator.SetBool(_animIDCrouch, isCrouching);
//    }

//    public override void Crouch()
//    {
        
//    }
//}
