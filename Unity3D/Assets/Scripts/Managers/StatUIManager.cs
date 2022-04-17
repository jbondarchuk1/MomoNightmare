using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUIManager : MonoBehaviour
{
    PlayerStats stats;
    Slider healthBar;
    Slider staminaBar;

    void Start()
    {
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        staminaBar = GameObject.Find("StaminaBar").GetComponent<Slider>();
    }

    void Update()
    {
        healthBar.value = (float)stats.health/stats.maxHealth;
        staminaBar.value = stats.stamina / stats.maxStamina;
    }

}
