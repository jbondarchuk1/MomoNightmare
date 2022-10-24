using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats : MonoBehaviour
{
    [Header("Maximums")]
    public int maxHealth = 100;
    public int maxStamina = 100;

    [Header("Actual Values")]
    public int health;
    public float stamina;
    public int sound;

    public void Die()
    {
        Debug.Log("Player Died");
    }

}
