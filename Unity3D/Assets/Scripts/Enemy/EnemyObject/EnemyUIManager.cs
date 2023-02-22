using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LayerManager;

public class EnemyUIManager : MonoBehaviour
{
    public Slider enemyHealthBar;
    public Slider enemyAwarenessBar;
    public EnemyStats enemyStats;
    public Animator UIanimator;

    private bool flipQ = false;
    private bool flipE = false;

    public bool isSelected { get; private set; } = false;

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

        HandleReactionAnimations();
    }

    private void HandleReactionAnimations()
    {
        // first check existing bools
        if (flipE) UIanimator.SetBool("Exclamation", false);
        if (flipQ) UIanimator.SetBool("Question", false);

        // reset the bools
        flipE = false;
        flipQ = false;

        // set the bools to trigger on next frame
        if (UIanimator.GetBool("Question")) flipQ = true;
        if (UIanimator.GetBool("Exclamation")) flipE = true;
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

    public void Question()
    {
        UIanimator.SetBool("Question", true);
    }
    public void Exclamation()
    {
        UIanimator.SetBool("Exclamation", true);
    }
    public void SpotEnemy()
    {
        enemyHealthBar.gameObject.SetActive(true);
        enemyAwarenessBar.gameObject.SetActive(true);
        enemyHealthBar.gameObject.layer = GetLayer(Layers.PriorityUI);
        enemyAwarenessBar.gameObject.layer = GetLayer(Layers.PriorityUI);
        isSelected = true;
    }
}
