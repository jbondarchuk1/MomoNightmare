using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class AimController
{
    public enum AimState { ThirdPerson, Zoom, FirstPerson }
    public AimState State { get; private set; } = AimState.ThirdPerson;
    [Header("Parent Controller Vals:")]
    [SerializeField] protected CinemachineVirtualCamera aimCam;
    [SerializeField] protected GameObject cinemachineCameraTarget;
    [Space]
    [Header("All Child Vals:")]
    [SerializeField] float sensitivityX = 1;
    [SerializeField] float sensitivityY = 1;
    [Space]
    [SerializeField] private ThirdPersonZoomAimController zoom;
    [SerializeField] private ThirdPersonAimController thirdPerson;
    [SerializeField] private FirstPersonAimController firstPerson;
    public bool AllowCameraRotation { get; protected set; }

    protected float TopClamp = 70.0f;
    protected float BottomClamp = -30.0f;
    float _cinemachineTargetYaw = 0f;
    float _cinemachineTargetPitch = 0f;
    protected int _animIDADS;
    StarterAssetsInputs _input;


    public void Aim(AimState state)
    {
        if (state != State) ChangeState(state);
        Aim();
    }
    private void ChangeState(AimState state)
    {
        AimBase currState = getAim(State);
        AimBase nextState = getAim(state);
        State = state;
        currState.Exit();
        nextState.Enter();
    }
    private void Aim()
    {
        AimBase activeAim = getAim(State);
        this.AllowCameraRotation = activeAim.AllowCameraRotation;
        activeAim.Aim();
    }
    private AimBase getAim(AimState state)
    {
        AimBase aimBase = null;
        switch (state)
        {
            case AimState.ThirdPerson:
                aimBase = thirdPerson;
                break;
            case AimState.FirstPerson:
                aimBase = firstPerson;
                break;
            case AimState.Zoom:
                aimBase = zoom;
                break;
        }
        return aimBase;
    }

    public void Initialize(StarterAssetsInputs inputs, Animator animator)
    {
        _input = inputs;
        GameObject cam = PlayerManager.Instance.camera.gameObject;
        zoom.Initialize(inputs, cam, animator, this.aimCam, cinemachineCameraTarget);
        thirdPerson.Initialize(inputs, cam, animator, this.aimCam, cinemachineCameraTarget);
        firstPerson.Initialize(inputs, cam, animator, this.aimCam, cinemachineCameraTarget);
    }

    public void HandleCamera()
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
    protected static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
