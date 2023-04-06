using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LayerManager;

public class SpotCone : MonoBehaviour
{
    public bool PlayerSpotted { get; set; } = false;
    [SerializeField] private Color alertColor;
    [SerializeField] SpotBox lightEnd;
    [SerializeField] Transform eyes;
    [SerializeField] Transform rayPoint;
    private Color defaultColor;

    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;    
    }
    private void Update()
    {
        if (lightEnd.Spotted)
        {
            Vector3 pos = PlayerManager.Instance.transform.position;
            pos.y += 1;
            Ray ray = new Ray(eyes.position, pos - eyes.position);
            float distance = Vector3.Distance(pos, eyes.position);
            LayerMask mask = GetMask(Layers.Ground, Layers.Obstruction);

            if (!Physics.Raycast(ray.origin, ray.direction, distance, mask))
                PlayerSpotted = true;
        }
        if (PlayerSpotted)
        {
            meshRenderer.material.color = alertColor;
            StartCoroutine(exit());
        }
        else meshRenderer.material.color = defaultColor;

        if (Physics.Raycast(
            eyes.position, rayPoint.position - eyes.position, out RaycastHit hit, Mathf.Infinity,
            GetMask( Layers.Ground, Layers.Obstruction)))
        {
            lightEnd.transform.position = hit.point;
        }
    }

    private IEnumerator exit()
    {
        yield return new WaitForSeconds(2f);
        PlayerSpotted = true;
    }
}
