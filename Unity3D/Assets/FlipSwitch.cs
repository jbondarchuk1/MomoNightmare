using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a monobehaviour for a physical switch. The triggerSwitch parent will automatically grab this component.
/// </summary>
[RequireComponent(typeof(TriggerSwitch))]
public class FlipSwitch : MonoBehaviour, IActivatable
{
    [SerializeField] private float openZRotation;
    [SerializeField] private float closeZRotation;
    [SerializeField][Range(0, 3)] private float speed = 1;

    bool open = false;

    public void Activate() =>  open = true;
    public void Deactivate() => open = false;
    public bool isActivated() => open;

    private void Update()
    {
        float goalRot = open ? openZRotation : closeZRotation;
        Vector3 currRot = transform.localRotation.eulerAngles;
        Vector3 newRot = currRot;
        newRot.z = goalRot;
        transform.localRotation = Quaternion.Euler(Vector3.Lerp(currRot, newRot, Time.deltaTime * speed));
    }
}
