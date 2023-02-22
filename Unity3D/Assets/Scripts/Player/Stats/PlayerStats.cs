using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatRangeLevel;

public class PlayerStats : Stats
{
    public enum ConsumableStats { Health, Stamina, Stealth }

    [HideInInspector] public Zone currentZone = null;
    [HideInInspector] public PlayerStealth playerStealth = new PlayerStealth();

    [HideInInspector] public Range StealthRange = Range.High;
    [HideInInspector] public Range NightmareRange = Range.High;

    #region Exposed In Editor
        public int jumpStamina;
        public int sprintStamina;
        public bool rechargingStamina;
    
        [Space][Header("Sound Factors")]
        [Range(0f, 10f)][SerializeField] private float crouchingFactor = 0f;
        [Range(0f, 10f)][SerializeField] private float standingFactor  = 0f;
        [Range(0f, 10f)][SerializeField] private float sprintingFactor = 0f;
        [Range(0f, 10f)][SerializeField] private float jumpingFactor   = 0f;
        [Range(0f, 10f)][SerializeField] private float landingFactor   = 0f;

        [SerializeField]  private StatRangeLevel NightmareRangeLevel = new StatRangeLevel();
    #endregion Exposed In Editor


    private PlayerMovement movement;
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        HandleMovementAndMovementStats();
        HandleStealthStats();
        NightmareRange = GetNightmareLevel();
    }
    public void FoundConsumable(ConsumableStats cs, int value)
    {
        switch (cs)
        {
            case ConsumableStats.Health:
                health += value;
                break;
            case ConsumableStats.Stamina:
                stamina += value;
                break;
            case ConsumableStats.Stealth:
                Debug.Log("idk what to do with this yet");
                break;
        }
    }
    private void HandleStealthStats()
    {
        playerStealth.Grounded = movement._groundedMovementController.isGrounded;
        playerStealth.Jumping = movement._animator.GetBool("isJumping");
        playerStealth.Crouching = movement._groundedMovementController.State == GroundedMovementController.MovementState.Crouch;
        playerStealth.Sprinting = movement._groundedMovementController.TargetSpeed == movement._groundedMovementController.standingController.SprintSpeed;
        playerStealth.Speed = movement._groundedMovementController.TargetSpeed;

        if (currentZone != null)
        {
            if (currentZone.GetType() == typeof(StealthZone))
                playerStealth.StealthZone = (StealthZone)currentZone;
        }

        float[] factors = new float[] { 0f, crouchingFactor, standingFactor, sprintingFactor, jumpingFactor, landingFactor };
        int soundIdx = playerStealth.RefreshValues();
        sound = (int)factors[soundIdx];

        if (soundIdx <= 3)
        {
            float s = (float)sound * movement._groundedMovementController.TargetSpeed;
            sound = (int)s;
        }
        StealthRange = GetStealthLevel();
    }
    private void HandleMovementAndMovementStats()
    {
        float sprintReduction = Time.deltaTime * sprintStamina;

        if (movement._animator.GetBool("isJumping"))
            stamina = stamina - jumpStamina >= 0 ? stamina - jumpStamina : 0;

        if (movement._groundedMovementController._speed == movement._groundedMovementController.standingController.SprintSpeed && stamina > 0)
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
    private Range GetNightmareLevel()
    {
        return NightmareRangeLevel.GetRangeLevel(health);
    }
    private Range GetStealthLevel()
    {
        return playerStealth.GetStealthRangeLevel();
    }

    public void DamagePlayer(int damage)
    {
        health -= damage;
        if (health <= 0) health = 0;
    }
    public void HealPlayer(int health)
    {
        int newHealth = this.health + health;
        if (newHealth >= maxHealth) this.health = maxHealth;
    }

}
