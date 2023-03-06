using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AimBase
{
    public abstract bool AllowCameraRotation { get; protected set; }

    [SerializeField] protected float sensitivityX = 1;
    [SerializeField] protected float sensitivityY = 1;
    [SerializeField] protected CinemachineVirtualCamera activeCamera;
    [SerializeField] protected GameObject cinemachineCameraTarget;

    protected StarterAssetsInputs _input;
    protected GameObject _mainCamera;
    protected Animator _animator;
    protected float CameraAngleOverride = 0.0f;
    protected AnimationIDContainer animationIDs = new AnimationIDContainer();
    protected float TopClamp = 70.0f;
    protected float BottomClamp = -30.0f;
    [HideInInspector] public float _cinemachineTargetYaw = 0f;
    [HideInInspector] public float _cinemachineTargetPitch = 0f;
    
    public abstract void Aim();
    public abstract void Enter();
    public abstract void Exit();

    public virtual void HandleCamera()
    {
        if (_input.look.sqrMagnitude >= 0.01f)
        {
            _cinemachineTargetYaw += _input.look.x * Time.deltaTime * sensitivityX;
            _cinemachineTargetPitch += _input.look.y * Time.deltaTime * sensitivityY;
        }
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + 0f, _cinemachineTargetYaw, 0.0f);
    }

    public void Initialize(StarterAssetsInputs inputs, GameObject mainCam, Animator animator)
    {
        _input = inputs;
        _mainCamera = mainCam;
        _animator = animator;
    }
    protected static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

}
