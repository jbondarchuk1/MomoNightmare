using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GroundedMovementController;

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
    [HideInInspector] public PlayerStealth playerStealth;
    [HideInInspector] public StatUIManager statUIManager;
    [HideInInspector] public PlayerSeenUIManager playerSeenUIManager;
    [HideInInspector] public PlayerStats statManager;
    [HideInInspector] public AbilitiesManager abilitiesManager;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;
    [HideInInspector] public new Rigidbody rigidbody;
    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public PlayerMovement playerMovementManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] public new Transform camera;
    [HideInInspector] public Transform target;
    public GameObject mesh;
    #endregion Public

    // private
    #region Private
    private PlayerStimulus stimulusManager;
    private GameObject effects;
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
        playerSeenUIManager = GetComponentInChildren<PlayerSeenUIManager>();
        animator = GetComponentInChildren<Animator>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        rigidbody = GetComponent<Rigidbody>();
        audioManager = GetComponent<AudioManager>();
        uiManager = GetComponentInChildren<UIManager>();
        camera = GameObject.Find("Main Camera").transform;

        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    private void Start()
    {
        playerStealth = statManager.playerStealth;
    }
    private void Update()
    {
        StartCoroutine(PlayerRoutines());
    }
    private void LateUpdate()
    {
        if (target != null)
            transform.SetPositionAndRotation(target.position, target.rotation);
        target = null;
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
        this.target = target;
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
        abilitiesManager.canUseAbility = playerMovementManager._groundedMovementController.State == MovementState.Stand;
    }

    public void DamagePlayer(int damage)
    {
        audioManager.PlaySound("Attacked");
        audioManager.PlaySound("Grunt", "Ouch");

        animator.SetBool("isAttacked", true);
        statManager.DamagePlayer(damage);
        uiManager.Damage();
        
    }

}
