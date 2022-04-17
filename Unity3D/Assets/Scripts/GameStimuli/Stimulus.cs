using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Stimulus : MonoBehaviour
{
    public float intensity = 0f;
    public bool active = true;

    public abstract void Emit();

}
