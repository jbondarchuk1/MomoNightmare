using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class AimController
{
    public enum AimState {None, ThirdPerson, Zoom, FirstPerson }
    [field: SerializeField]public AimState State { get; private set; } = AimState.None;
    [Header("Parent Controller Vals:")]

    [Space]
    [SerializeField] private ThirdPersonZoomAimController zoom;
    [SerializeField] private ThirdPersonAimController thirdPerson;
    [SerializeField] private FirstPersonAimController firstPerson;

    public bool ForceFirstPerson { get; set; } = false;


    public bool AllowCameraRotation { get; protected set; }

    protected int _animIDADS;

    public void HandleCamera()
    {
        getAim(State).HandleCamera();
    }
    public void Aim(AimState state)
    {
        state = ForceFirstPerson ? AimState.FirstPerson : state;
        if (state != State) ChangeState(state);
        Aim();
    }
    private void ChangeState(AimState state)
    {
        AimBase currState = getAim(State);
        AimBase nextState = getAim(state);
        
        if (currState != null)
        {
            nextState._cinemachineTargetYaw = currState._cinemachineTargetYaw;
            nextState._cinemachineTargetPitch = currState._cinemachineTargetPitch;
        }
        
        State = state;
        
        currState?.Exit();
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
        GameObject cam = PlayerManager.Instance.camera.gameObject;
        zoom.Initialize(inputs, cam, animator);
        thirdPerson.Initialize(inputs, cam, animator);
        firstPerson.Initialize(inputs, cam, animator);
    }
}
