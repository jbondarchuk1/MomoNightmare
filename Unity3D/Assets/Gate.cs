using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Gate : MonoBehaviour, IActivatable
{
    [SerializeField] private float openHeight;
    [SerializeField] private float closeHeight;
    [SerializeField][Range(0, 3)] private float speed = 1;
    [SerializeField] private TriggerSwitch triggerSwitch;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    CinematicUIManager cinematicUIManager;
    [SerializeField] bool open = false;


    private void Start()
    {
        triggerSwitch.OnSwitchActive += Activate;
        triggerSwitch.OnSwitchInactive += Deactivate;
        cinematicUIManager = PlayerManager.Instance.uiManager.CinematicUIManager;
    }
    private void OnDisable()
    {
        triggerSwitch.OnSwitchActive -= Activate;
        triggerSwitch.OnSwitchInactive -= Deactivate;
    }

    private void Update()
    {
        float goalHeight = open ? openHeight : closeHeight;
        Vector3 currPos = transform.position;
        Vector3 newPos = currPos;
        newPos.y = Mathf.Lerp(currPos.y, goalHeight, Time.deltaTime* speed);
        transform.position = newPos;
    }

    public void Activate()
    {
        open = true;
        StartCoroutine(PlayerManager.Instance.SetInvincibility(1));
        StartCoroutine(SetCamera(1));
    }
    public void Deactivate()
    {
        open = false;
        StartCoroutine(PlayerManager.Instance.SetInvincibility(1));
        StartCoroutine(SetCamera(1));
    }
    private IEnumerator SetCamera(float duration)
    {
        this.cinemachineVirtualCamera.enabled = true;
        cinematicUIManager.Activate();
        PlayerManager.Instance.playerMovementManager.canMove = false;
        yield return new WaitForSeconds(duration);
        this.cinemachineVirtualCamera.enabled = false;
        cinematicUIManager.Deactivate();
        PlayerManager.Instance.playerMovementManager.canMove = true;

    }
    public bool isActivated() => open;

}
