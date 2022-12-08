using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton Class:
/// Attributes include all necessary scripts on player GameObject.
/// Runs Child Object Coroutines.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    // expose
    
    #region Public
    [HideInInspector] public Transform camera;
    [HideInInspector] public PlayerStealth playerStealth;
    [HideInInspector] public StatUIManager statUIManager;
    [HideInInspector] public PlayerSeenUIManager playerSeenUIManager;
    [HideInInspector] public PlayerStats statManager;
    [HideInInspector] public AbilitiesManager abilitiesManager;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;
    #endregion Public

    // private
    #region Private
    private PlayerMovement playerMovementManager;
    private PlayerStimulus stimulusManager;
    private GameObject effects;
    private MovementBase currMoveScript;
    #endregion Private

    #region Awake Start Update
    private void Awake()
    {
        abilitiesManager = GetComponentInChildren<AbilitiesManager>();
        playerMovementManager = GetComponent<PlayerMovement>();
        statManager = GetComponent<PlayerStats>();
        stimulusManager = GetComponentInChildren<PlayerStimulus>();
        statUIManager = GetComponentInChildren<StatUIManager>();
        effects = GameObject.Find("Effects");
        currMoveScript = playerMovementManager;
        playerSeenUIManager = GetComponentInChildren<PlayerSeenUIManager>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        camera = GameObject.Find("Main Camera").transform;

        if (Instance == null) Instance = this;
        else GameObject.Destroy(this.gameObject);

    }
    private void Start()
    {
        playerStealth = statManager.playerStealth;
    }
    private void Update()
    {
        StartCoroutine(PlayerRoutines());
    }
    #endregion Awake Start Update

    private IEnumerator PlayerRoutines()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
        stimulusManager.HandleStimuli();
        HandleEndurance();
        HandleAim();
        HandleInteraction();
        HandleAbilities();

        yield return wait;
    }

    private void HandleInteraction()
    {
        playerInteractionManager.HandleRegularInteractions();
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

    public void TeleportTo(Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
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
        statUIManager.ActiveReticle = playerMovementManager.isAiming;
    }
    private void HandleAbilities()
    {
        abilitiesManager.canUseAbility = !playerMovementManager.isCrouching;
    }


}
