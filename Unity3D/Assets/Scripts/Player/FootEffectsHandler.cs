using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootEffectsHandler : MonoBehaviour, IPoolUser
{
    private PlayerMovement movement;
    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "Tracks";

    private void Start()
    {
        movement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        ObjectPooler = GameObject.Find("-- Pooler --").GetComponent<ObjectPooler>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (movement._speed == movement.SprintSpeed)
        {
            Vector3 position;
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, LayerMask.GetMask("Ground"), 5))
            {
                position = hit.point;
            }
            else position = transform.position;
            
            ObjectPooler.SpawnFromPool(Tag, position, Quaternion.identity);
            
        }

    }


}
