using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRipple : MonoBehaviour
{
    public MeshRenderer waterMesh;
    public Transform objectTransform;
    public float lifeSpan = 5f;
    private float deathTime = Mathf.Infinity;
    public bool stopEmitting = true;
    public Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waterMesh != null && objectTransform != null)
        {
            Bounds boxBounds = waterMesh.bounds;
            Vector3 checkposition = objectTransform.position;
            if (this.name.Contains("Splash") && initialPosition != null)
            {
                checkposition = initialPosition;
            }
            checkposition.y = boxBounds.center.y + boxBounds.size.y / 2;
            transform.position = boxBounds.ClosestPoint(checkposition);

            if (Time.time >= deathTime)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    public void TriggerDeath()
    {
        if (stopEmitting)
        {
            GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        deathTime = Time.time + lifeSpan;
    }
}
