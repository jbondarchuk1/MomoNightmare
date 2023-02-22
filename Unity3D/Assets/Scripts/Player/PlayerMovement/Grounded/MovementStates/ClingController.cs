using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClingController : MovementBase
{
    [field: SerializeField] public override float TargetSpeed { get; set; } = 1f;
    public bool isClinging { get; set; }
    [SerializeField][Range(0, 2)] private float checkDistance = 1f;

    private enum ClingStatus { ClingL, ClingR, Disabled }
    private ClingObject NearestClingObject { get; set; }
    private Vector3 ClingRotation { get; set; }

    #region Public
    public void CheckForClingObject()
    {
        Vector3 pos = PlayerManager.Instance.transform.position;
        pos.y += 1;
        Ray ray = new Ray(pos, PlayerManager.Instance.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out RaycastHit hit, .5f, LayerMask.GetMask("Obstruction")))
        {
            Debug.DrawRay(pos, hit.normal, Color.red, Mathf.Infinity);
            if (hit.transform.gameObject.TryGetComponent(out ClingObject co))
            {
                // FUNCTIONAL PART
                if (_input.crouch)
                {
                    isClinging = true;
                    NearestClingObject = co;
                    ClingRotation = hit.transform.rotation.eulerAngles;
                }
            }
        }
    }

    public override void Move(bool allowCamRot)
    {
        float inputMagnitude = _input.move.magnitude;
        inputMagnitude = GetClingInputMagnitude(_input.move.normalized, inputMagnitude);
        isClinging = isClinging && !CheckForStop() && _input.crouch;
        HandleRotation(allowCamRot);
        HandleSpeed(inputMagnitude);
        SetControllerMotion();
        SetAnimations();
    }
    public override void Enter()
    {
        _animator.SetBool(animationIDs._animIDCrouch, true);
        isClinging = true;
    }
    public override void Exit()
    {
        _animator.SetBool(animationIDs._animIDCrouch, false);
        HandleClingAnimations(); // disabled by default
        isClinging = false;
    }
    #endregion

    // if the angle is positive, player is on the left  and the rotation the player moves in is rotationL
    // if the angle is negative, player is on the right and the rotation the player moves in is rotationR

    // the speed the player should be moving in that direction is determined by abs(angle).
    // An angle of 0 should not move at all
    // an angle of 180 should move 100%
    private float GetClingInputMagnitude(Vector2 inputDirection, float inputMagnitude)
    {
        Vector3 rotation = ClingRotation;

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
            HandleClingAnimations(ClingStatus.ClingR);
        }
        else if (angle > 180)
        {
            mainRotation = rotationL;
            HandleClingAnimations(ClingStatus.ClingL);
        }
        else
        {
            isClinging = false;
            HandleClingAnimations(ClingStatus.Disabled);
            return 0;
        }

        Vector3 euler = mainRotation.eulerAngles;
        mainRotation.eulerAngles = euler;
        if (inputMagnitude > 0) _targetRotation = mainRotation.eulerAngles.y;
        if (!_input.crouch)
        {
            isClinging = false;
            return 0;
        }

        return inputMagnitude;
    }
    /// <summary>
    /// Parameter 0 = R
    /// Parameter 1 = L
    /// any other parameter = no cling (default of -1)
    /// </summary>
    private void HandleClingAnimations(ClingStatus clingStatus = ClingStatus.Disabled)
    {
        switch (clingStatus)
        {
            case ClingStatus.ClingR:
                _animator.SetBool("isCling", true);
                _animator.SetBool("isClingR", true);
                break;
            case ClingStatus.ClingL:
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

    protected new void HandleRotation(bool allowCamRot)
    {
        float rotation = Mathf.SmoothDampAngle(PlayerManager.Instance.transform.eulerAngles.y, NearestClingObject.transform.rotation.eulerAngles.y+180, ref _rotationVelocity, rotationSmoothTime);
        if (allowCamRot) PlayerManager.Instance.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    protected override float GetTargetSpeed(float inputMagnitude)
    {
        if (inputMagnitude > 0.01f)
         return TargetSpeed;
        return 0;
    }
    private bool CheckForStop() => NearestClingObject.MinDistanceToStop(PlayerManager.Instance.transform) < checkDistance;

}
