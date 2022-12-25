using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootEffectsHandler : MonoBehaviour, IPoolUser
{
    private PlayerMovement movement;
    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "Tracks";
    [SerializeField] private Transform bottom;
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        movement = PlayerManager.Instance.gameObject.GetComponent<PlayerMovement>();
        ObjectPooler = ObjectPooler.Instance;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (movement._speed == movement.SprintSpeed)
        {
            Vector3 position = new Vector3(transform.position.x, bottom.position.y, transform.position.z);

            ObjectPooler.SpawnFromPool(Tag, position, Quaternion.identity);
            StartCoroutine(TraumaInducer.Instance.InduceStress(45, 0.04f));
            audioManager.Play("Footsteps");
        }
    }
}
