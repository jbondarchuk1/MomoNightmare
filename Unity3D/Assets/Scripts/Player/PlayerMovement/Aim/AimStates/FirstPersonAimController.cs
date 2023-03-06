using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FirstPersonAimController : AimBase
{
    [SerializeField] protected LayerMask aimColliderLayerMask;
	public override bool AllowCameraRotation { get; protected set; } = false;
	public float RotationSpeed = 1.0f;

	public override void Aim() 
	{
		if (_input.look.sqrMagnitude >= 0.01f)
		{
			_cinemachineTargetPitch += _input.look.y * RotationSpeed * sensitivityY;
			_cinemachineTargetYaw += _input.look.x * RotationSpeed * sensitivityX;
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
			cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
			PlayerManager.Instance.transform.Rotate(Vector3.up * _input.look.x * RotationSpeed * sensitivityX);
		}
	}

    public override void Enter()
    {
		activeCamera.gameObject.SetActive(true);
    }

    public override void Exit()
    {
		activeCamera.gameObject.SetActive(false);
	}

}
