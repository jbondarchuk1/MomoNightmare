using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject, IActivatable
{
    [SerializeField] private int openZRotation;
    [SerializeField] private int closeZRotation;

    private AudioManager audioManager;
    [SerializeField] private string soundName = "Open";
    [SerializeField] private PhysicalItem itemChild;
    private float rot = 0;
    [SerializeField][Range(0, 3)] private float speed = 1;
    [SerializeField][Range(0, 3)] private float scaleSpeed = 1;

    public bool open = false;

    public void Activate() => open = true;
    public void Deactivate() => open = false;
    public bool isActivated() => open;
    private new void Start()
    {
        base.Start();
        TryGetComponent(out audioManager);
    }
    private void Update()
    {
        float goalRot = open ? openZRotation : closeZRotation;
        rot = Mathf.Lerp(rot, goalRot, Time.deltaTime * speed);
        Vector3 rotation = Vector3.zero;
        rotation.x = rot;
        transform.localRotation = Quaternion.Euler(rotation);

        Debug.Log(transform.localRotation.eulerAngles.x);
        Debug.Log(openZRotation);
        if (open) OnOpen();
    }

    private void OnOpen()
    {
        if (audioManager != null) audioManager.PlaySound(soundName);
        itemChild.gameObject.SetActive(true);
        ScaleUp(itemChild.transform, 1, scaleSpeed);
    }
    private void ScaleUp(Transform t, float scale, float speed)
    {
        Vector3 newScale = new Vector3(scale, scale, scale);
        t.localScale = Vector3.Lerp(t.localScale, newScale, speed * Time.deltaTime);
    }
}
