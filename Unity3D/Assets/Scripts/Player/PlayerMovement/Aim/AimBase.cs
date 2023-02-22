using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AimBase
{
    public abstract bool AllowCameraRotation { get; protected set; }
    
    protected StarterAssetsInputs _input;
    protected GameObject _mainCamera;
    protected Animator _animator;
    protected CinemachineVirtualCamera aimCam;
    protected float CameraAngleOverride = 0.0f;
    protected GameObject CinemachineCameraTarget;
    protected AnimationIDContainer animationIDs = new AnimationIDContainer();

    public abstract void Aim();
    public abstract void Enter();
    public abstract void Exit();
    public void Initialize(
        StarterAssetsInputs inputs, GameObject mainCam, 
        Animator animator, CinemachineVirtualCamera aimCam, GameObject cinemachineCameraTarget)
    {
        _input = inputs;
        _mainCamera = mainCam;
        _animator = animator;
        this.aimCam = aimCam;
        this.CinemachineCameraTarget = cinemachineCameraTarget;
    }
}
