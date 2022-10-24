using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FootPoofPoolController : MonoBehaviour, IPooledObject
{
    [SerializeField] private VisualEffect visualEffect;

    
    public void OnObjectSpawn()
    {
        visualEffect.Play();
    }
}
