using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LayerManager;

public class ExplosionStimulus : Stimulus
{
    [SerializeField] private bool useDamageByDistance = false;
    [SerializeField] private int maxDamage = 60;
    [SerializeField] private int force = 150;

    public override void Emit()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, intensity, GetMask(Layers.Target, Layers.Enemy, Layers.Interactable));
        foreach (Collider collider in colliders)
        {
            Layers layer = GetLayerEnum(collider.gameObject.layer);
            switch (layer)
            {
                case Layers.Target:         // player
                    ExplodePlayer(collider.gameObject);
                    break;
                case Layers.Enemy:          // enemies
                    ExplodeEn(collider.gameObject);
                    break;
                case Layers.Interactable:   // interactable
                    ExplodeObject(collider.gameObject);
                    break;
                default: break;
            }
        }
    }

    private void ExplodeEn(GameObject enemy)
    {
        if (enemy.TryGetComponent(out ExplodeEnemy explode))
            explode.Explode(transform.position, intensity, force, maxDamage);
    }
    private void ExplodeObject(GameObject obj)
    {
        if (obj.TryGetComponent(out IDestructable destructable))
            destructable.ExplodeObj(transform.position, force);
        else if (obj.TryGetComponent(out Rigidbody rb))
            rb.AddExplosionForce(force*3, transform.position, intensity, 2);
    }

    // TODO
    private void ExplodePlayer(GameObject player)
    {
        if (player.transform.position == PlayerManager.Instance.transform.position)
        {
            Debug.Log("Exploding player");
        }
    }
}
