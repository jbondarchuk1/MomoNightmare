using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChange : MonoBehaviour
{
    [SerializeField] private float toScale = 1f; 
    [SerializeField] private float scaleSpeed = 1f;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private Vector3 rotateAxis = new Vector3(-1, .3f, -3);
    private Vector3 baseScale;
    private Vector3 rot;
    private bool scaleDown = false; 
    private void Start()
    {
        baseScale = transform.localScale;
    }
    private void Update()
    {
        Vector3 scale = new Vector3(toScale, toScale, toScale);
        scaleDown = (scaleDown && transform.localScale != scale) || (!scaleDown && transform.localScale == baseScale);

        scale = scaleDown ? scale : baseScale;

        transform.localScale = Vector3.Slerp(transform.localScale, scale, scaleSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rot);
        transform.Rotate(rotateAxis, rotateSpeed*Time.deltaTime);
        rot = transform.rotation.eulerAngles;
    }
}
