using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SplashHandler : MonoBehaviour
{
    [Header("Required Fields")]
    public GameObject rippleParticlePrefab;
    public GameObject splashParticlePrefab;
    public MeshRenderer waterMesh;
    public LayerMask generationLayer;
    public float bigSplashThreshold = 0f;

    public Dictionary<Collider,GameObject> ripples = new Dictionary<Collider,GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        if ((generationLayer.value & other.gameObject.layer) == 0)
        {
            if (other.TryGetComponent(out Rigidbody rb))
            {
                float velocity = 0f;
                if (Mathf.Abs(rb.velocity.y) > bigSplashThreshold)
                    velocity = rb.velocity.y;
                else if (other.TryGetComponent(out NavMeshAgent nma))
                    velocity = nma.velocity.y;
                else if (other.TryGetComponent(out CharacterController controller))
                    velocity = controller.velocity.y;
                else if (other.gameObject.layer == Mathf.Pow(2,6))
                {
                    if (other.TryGetComponent(out PlayerMovement pm)) 
                    {
                        velocity = !pm._groundedMovementController.isGrounded ? bigSplashThreshold + 1 : 0f;
                        if (velocity == 0f)
                            Debug.Log(pm._groundedMovementController.isGrounded);
                    }
                }

                if (Mathf.Abs(velocity) > bigSplashThreshold)
                    MakeBigSplash(other);

            }
            MakeRipple(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GameObject ripple;
        if (ripples.TryGetValue(other, out ripple))
        {
            ripples.Remove(other);
            ripple.GetComponent<WaterRipple>().TriggerDeath();
        }
    }

    private void MakeBigSplash(Collider other)
    {
        WaterRipple splashRipple = splashParticlePrefab.GetComponent<WaterRipple>();
        splashRipple.objectTransform = other.transform;
        splashRipple.waterMesh = waterMesh;
        splashRipple.initialPosition = other.transform.position;

        GameObject splash = Instantiate(splashParticlePrefab);
        splashRipple = splash.GetComponent<WaterRipple>();
        splashRipple.stopEmitting = false;
        splashRipple.TriggerDeath();
    }

    private void MakeRipple(Collider other)
    {
        WaterRipple ripple = rippleParticlePrefab.GetComponent<WaterRipple>();
        ripple.waterMesh = waterMesh;
        ripple.objectTransform = other.transform;

        GameObject splashParticle = Instantiate(rippleParticlePrefab);
        ripples.Add(other, splashParticle);
    }
}
