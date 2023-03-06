using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LayerManager;

[RequireComponent(typeof(Item))]
public class ItemInteractableObject : InteractableObject, IActivatable
{
    private Item item;
    private ParticleSystem particleSystem;
    private AudioManager audioManager;
    [SerializeField] private string soundName = "Item";
    [SerializeField] private float waitTime = 1f;

    private new void Start()
    {
        base.Start();
        item = GetComponent<Item>();
        TryGetComponent(out particleSystem);
        TryGetComponent(out audioManager);
    }
    public void Activate()
    {
        if (item == null)
        {
            Debug.LogError("Item is null for item interactable object");
            return;
        }

        if (particleSystem != null) particleSystem.Play();
        if (audioManager != null) audioManager.PlaySound(soundName);

        if (particleSystem != null || audioManager != null)
            StartCoroutine(WaitObtain(waitTime));
        else item.Obtain();
    }

    private IEnumerator WaitObtain(float waitTime)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.enabled = false;
        yield return new WaitForSeconds(waitTime);
        mr.enabled = true;
        item.Obtain();
    }

    public void Deactivate() { }
    public bool isActivated() => false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GetLayer(Layers.Target))
            Activate();
    }
    private void OnTriggerExit(Collider other)
    {
        Deactivate();
    }
}