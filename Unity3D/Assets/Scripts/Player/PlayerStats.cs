using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    [HideInInspector] public Zone currentZone = null;
    [HideInInspector] public PlayerStealth playerStealth = new PlayerStealth();
    
    public int jumpStamina;
    public int sprintStamina;
    public bool rechargingStamina;
    
    [Space][Header("Sound Factors")]
    [Range(0f, 10f)] public float crouchingFactor = 0f;
    [Range(0f, 10f)] public float standingFactor  = 0f;
    [Range(0f, 10f)] public float sprintingFactor = 0f;
    [Range(0f, 10f)] public float jumpingFactor   = 0f;
    [Range(0f, 10f)] public float landingFactor   = 0f;

    private PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMajorStats();
    }

    public void HandleStealthStats()
    {
        playerStealth.Grounded = movement.Grounded;
        playerStealth.Jumping = movement._animator.GetBool("isJumping");
        playerStealth.Crouching = movement.isCrouching;
        playerStealth.Sprinting = movement._speed == movement.SprintSpeed;
        playerStealth.Speed = movement._speed;

        if (currentZone != null)
        {
            if (currentZone.GetType() == typeof(StealthZone))
                playerStealth.StealthZone = (StealthZone)currentZone;
        }


        float[] factors = new float[] { 0f, crouchingFactor, standingFactor, sprintingFactor, jumpingFactor, landingFactor };
        int soundIdx = playerStealth.UpdateValues();
        sound = (int)factors[soundIdx];

        if (soundIdx <= 3)
        {
            float s = (float)sound * movement._speed;
            sound = (int)s;
        }
    }

    private void HandleMajorStats()
    {
        float sprintReduction = Time.deltaTime * sprintStamina;

        if (movement._animator.GetBool("isJumping"))
            stamina = stamina - jumpStamina >= 0 ? stamina - jumpStamina : 0;

        if (movement._speed == movement.SprintSpeed && stamina > 0)
            stamina -= sprintReduction;
        else if (stamina < maxStamina)
            stamina += sprintReduction * 2;
        else
            stamina = maxStamina;


        if (stamina < 1)
            rechargingStamina = true;
        if (rechargingStamina && stamina == maxStamina)
            rechargingStamina = false;
    }
}
