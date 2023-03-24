using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicUIManager : MonoBehaviour, IActivatable
{
    RectTransform rect;
    public float invisibleScale = 3f;
    public float visibleScale = 1f;
    [SerializeField] private float speed = 1f;
    public bool activated = false;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }

    public bool isActivated() => activated;

    private void Update()
    {
        float toScale = activated ? visibleScale : invisibleScale;

        Vector3 scale = rect.localScale;
        scale.y = Mathf.Lerp(scale.y, toScale, speed * Time.deltaTime);
        rect.localScale = scale;
    }
}
