using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IActivatable
{
    [SerializeField] private float openHeight;
    [SerializeField] private float closeHeight;
    [SerializeField][Range(0, 3)] private float speed = 1;
    [SerializeField] private TriggerSwitch triggerSwitch;
    bool open = false;
    private void Start()
    {
        triggerSwitch.OnSwitchActive += Activate;
        triggerSwitch.OnSwitchInactive += Deactivate;
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
    }
    public void Deactivate()
    {
        open = false;
    }
    public bool isActivated() => open;


}
