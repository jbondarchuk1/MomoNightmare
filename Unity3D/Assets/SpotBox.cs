using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotBox : MonoBehaviour
{
    public bool Spotted { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerManager manager))
            Spotted = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerManager manager))
            Spotted = false;
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) > 10) Spotted = false;
    }
}