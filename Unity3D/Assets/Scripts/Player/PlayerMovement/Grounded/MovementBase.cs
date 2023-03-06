using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public abstract class MovementBase
{
    [SerializeField] protected float rotationSmoothTime = 1f;
    [SerializeField] protected float speedChangeRate = 10.0f;

    // initialized from top level
    protected Animator _animator;
    protected CharacterController _controller;
    protected StarterAssetsInputs _input;
    protected GameObject _mainCamera;
    protected new CapsuleCollider collider;

    protected AnimationIDContainer animationIDs = new AnimationIDContainer(); 
    public abstract float TargetSpeed { get; set; }
    [HideInInspector] public float _speed;
    [HideInInspector] public float _speedX;
    [HideInInspector] public float _speedZ;
    [HideInInspector] public float _rotationVelocity;
    [HideInInspector] public float _verticalVelocity;
    [HideInInspector] public float _targetRotation = 0.0f;
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool canMove = true;


    protected float _animationBlend;

    public void Initialize(Animator _animator, 
        CharacterController _controller, StarterAssetsInputs _input,
        GameObject _mainCamera, CapsuleCollider collider)
    {
        this._animator = _animator;
        this._controller = _controller;
        this._input = _input;
        this._mainCamera = _mainCamera;
        this.collider = collider;
    }
    public abstract void Move(bool allowCamRot);
    public abstract void Enter();
    public abstract void Exit();

    public void Update(float speed, float speedX, float speedZ, float rotationVelocity, float verticalVelocity, float targetRotation)
    {
        this._speed = speed;
        this._speedX = speedX;
        this._speedZ = speedZ;
        this._rotationVelocity = rotationVelocity;
        this._verticalVelocity = verticalVelocity;
        this._targetRotation = targetRotation;
    }

    protected void HandleSpeed(float inputMagnitude)
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = GetTargetSpeed(inputMagnitude);
        if (!canMove) targetSpeed = 0;
        
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset && isGrounded)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else _speed = targetSpeed;

        _speedX = Mathf.Lerp(_speedX, _input.move.x, Time.deltaTime * speedChangeRate);
        _speedZ = Mathf.Lerp(_speedZ, _input.move.y, Time.deltaTime * speedChangeRate);
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

        if (inputMagnitude == 0)
            Debug.Log($"input mag: {inputMagnitude}, target speed: {targetSpeed}, speed: {_speed}");
    }

    protected abstract float GetTargetSpeed(float inputMagnitude);

    protected void SetAnimations()
    {
        _animator.SetFloat(animationIDs._animIDSpeed, _speed);
        _animator.SetFloat(animationIDs._animIDSpeedX, _speedX);
        _animator.SetFloat(animationIDs._animIDSpeedZ, _speedZ);
    }
    
    protected void SetControllerMotion()
    {
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        Vector3 motion = targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
        _controller.Move(motion);
    }
    protected void HandleRotation(bool allowCamRot)
    {
        if (_input.move != Vector2.zero)
        {
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
        }
        // rotation
        float rotation = Mathf.SmoothDampAngle(PlayerManager.Instance.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);
        if (allowCamRot && canMove) PlayerManager.Instance.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
}
