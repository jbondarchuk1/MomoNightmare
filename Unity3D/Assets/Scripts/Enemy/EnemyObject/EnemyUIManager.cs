using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIManager : MonoBehaviour
{
    public Slider enemyHealthBar;
    public Slider enemyAwarenessBar;
    public EnemyStats enemyStats;

    private void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
    }
    private void Update()
    {
        if (enemyHealthBar == null || enemyAwarenessBar == null) return;

        float awareness = enemyStats.awareness;
        float maxAwareness = enemyStats.maxAwareness;
        float health = enemyStats.health;
        float maxHealth = enemyStats.maxHealth;

        enemyHealthBar.value = health / maxHealth;
        enemyAwarenessBar.value = awareness / maxAwareness;
    }
    
    public void SetLayer(string layerName)
    {
        Debug.Log("Changing " + this.gameObject.name + " to " + layerName);
        LayerMask layer = LayerMask.NameToLayer(layerName);
        enemyHealthBar.gameObject.layer = layer;
        enemyAwarenessBar.gameObject.layer = layer;
        Debug.Log(LayerMask.LayerToName(layer));
        this.gameObject.layer = layer;
    }
}
