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
    public Transform camera;
    [HideInInspector] public PlayerStealth playerStealth;
    [HideInInspector] public StatUIManager statUIManager;
    [HideInInspector] public PlayerSeenUIManager playerSeenUIManager;
    [HideInInspector] public PlayerStats statManager;
    [HideInInspector] public AbilitiesManager abilitiesManager;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;
    [HideInInspector] public new Rigidbody rigidbody;
    [SerializeField] public Animator animator;
    [HideInInspector] public AudioManager audioManager;
    public PlayerMovement playerMovementManager;
    #endregion Public

    // private
    #region Private
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
        animator = GetComponentInChildren<Animator>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        rigidbody = GetComponent<Rigidbody>();
        audioManager = GetComponent<AudioManager>();

        if (Instance == null) Instance = this;
        else GameObject.Destroy(this.gameObject);

    }
    private void Start()
    {
        camera = GameObject.Find("Main Camera").transform;
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

    public void DamagePlayer(int damage)
    {
        audioManager.PlaySound("Attacked");
        audioManager.PlaySound("Grunt", "Ouch");

        animator.SetBool("isAttacked", true);
        statManager.DamagePlayer(damage);
    }



}
