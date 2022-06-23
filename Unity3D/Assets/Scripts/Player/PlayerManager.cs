using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{ 
    // expose
    [HideInInspector] public PlayerStealth playerStealth;
    [HideInInspector] public AbilitiesManager abilitiesManager;

    // private
    private PlayerMovement playerMovementManager;
    private PlayerStats statManager;
    private PlayerStimulus stimulusManager;
    private GameObject effects;
    private StatUIManager uiManager;

    private void Awake()
    {
        abilitiesManager = GetComponentInChildren<AbilitiesManager>();
        playerMovementManager = GetComponent<PlayerMovement>();
        statManager = GetComponent<PlayerStats>();
        stimulusManager = GetComponentInChildren<PlayerStimulus>();
        uiManager = GetComponentInChildren<StatUIManager>();
        effects = GameObject.Find("Effects");
    }

    private void Start()
    {
        playerStealth = statManager.playerStealth;
    }
    private void Update()
    {
        StartCoroutine(PlayerRoutines());
    }
    private IEnumerator PlayerRoutines()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
        statManager.HandleStealthStats();
        stimulusManager.HandleStimuli();
        HandleEndurance();
        HandleAim();

        yield return wait;
    }

    public void HandleZone(Zone zone)
    {
        if (zone == null)
            statManager.currentZone = null;

        else if (zone.name.Contains("Stealth"))
        {
            StealthZone stealthZone = (StealthZone)zone;
            statManager.currentZone = stealthZone;
        }
    }

    private void HandleEndurance()
    {
        if (statManager.rechargingStamina)
        {
            playerMovementManager.canJump = false;
            playerMovementManager.canSprint = false;
        }
        else
        {
            playerMovementManager.canJump = true;
            playerMovementManager.canSprint = true;
        }
    }

    private void HandleAim()
    {
        uiManager.activeReticle = playerMovementManager.isAiming;
    }
}
