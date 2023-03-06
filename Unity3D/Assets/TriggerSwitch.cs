using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If you want to have a switch flip or button compress on click, add an activatable object to the child.
/// </summary>
public class TriggerSwitch : InteractableObject, IActivatable
{
    [SerializeField] private bool enableMultipleActivation = false;
    [SerializeField] private FlipSwitch physicalTrigger;

    public delegate void SwitchActive();
    public event SwitchActive OnSwitchActive;

    public delegate void SwitchInactive();
    public event SwitchInactive OnSwitchInactive;

    bool active = false;
    private new void Start()
    {
        base.Start();
    }
    public void Activate()
    {
        if (!isActivated())
        {
            OnSwitchActive?.Invoke();
            active = true;
            physicalTrigger.Activate();
        }
    }

    public void Deactivate()
    {
        if (enableMultipleActivation && active)
        {
            active = false;
            OnSwitchInactive?.Invoke();
            physicalTrigger.Deactivate();
        }
    }

    public bool isActivated() => active;
}