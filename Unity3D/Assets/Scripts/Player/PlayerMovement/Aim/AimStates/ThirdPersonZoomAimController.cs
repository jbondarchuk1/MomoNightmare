using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThirdPersonZoomAimController: AimBase
{
    [SerializeField] protected LayerMask aimColliderLayerMask;
    public override bool AllowCameraRotation { get; protected set; } = false;

    /// <summary>
    /// Handles the right click to 3rd person shooter aiming function.
    /// When the user right clicks, they aim, another right click  will go back to the normal camera view.
    /// </summary>
    public override void Aim()
    {

        Vector3 mouseWorldPos = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
            mouseWorldPos = hit.point;

        aimCam.gameObject.SetActive(true);
        Vector3 worldAimTarget = mouseWorldPos;
        worldAimTarget.y = PlayerManager.Instance.transform.position.y;

        bool lastTab = _input.tab;
        float toVal = lastTab ? 1f : 0f;

        Cinemachine3rdPersonFollow follow = aimCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        float currVal = follow.CameraSide;
        follow.CameraSide = Mathf.Lerp(currVal, toVal, .3f);

        Vector3 aimDirection = (worldAimTarget - PlayerManager.Instance.transform.position);
        PlayerManager.Instance.transform.forward = Vector3.Lerp(PlayerManager.Instance.transform.forward, aimDirection.normalized, Time.deltaTime * 20);
    }

    public override void Enter()
    {
        Debug.Log("Entering Zoom");
        _animator.SetBool(animationIDs._animIDADS, true);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Zoom");
        _animator.SetBool(animationIDs._animIDADS, false);
    }
}