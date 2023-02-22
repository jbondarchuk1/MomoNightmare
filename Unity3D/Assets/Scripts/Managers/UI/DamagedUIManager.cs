using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class DamagedUIManager : MonoBehaviour
{
    private RawImage damagedImage;
    private PlayerStats playerStats;
    [SerializeField] private float animationSpeed = 1f;
    private Animator animator;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        damagedImage = GetComponentInChildren<RawImage>();
        playerStats = PlayerManager.Instance.statManager;
        PlayerAnimationEventHandler.OnReceiveAttack += DamageInvoke;
    }
    private void OnDisable()
    {
        PlayerAnimationEventHandler.OnReceiveAttack -= DamageInvoke;
    }

    private void DamageInvoke()
    {
        animator.SetBool("Damage", false);
    }




    public void Damage()
    {
        animator.SetBool("Damage", true);
    }
    private void Update()
    {
        Color color = damagedImage.color;
        float newOpacity = 0.05f * (1f - ((float)playerStats.health / (float)playerStats.maxHealth));
        if (playerStats.health > 25) newOpacity = 0f;
        Color newColor = new Color(color.r, color.g, color.b, newOpacity);
        damagedImage.color = newColor;
    }
}