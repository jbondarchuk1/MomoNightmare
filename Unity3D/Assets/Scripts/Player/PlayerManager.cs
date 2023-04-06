using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GroundedMovementController;

/// <summary>
/// Singleton Class:
/// Attributes include all necessary scripts on player GameObject.
/// Runs Child Object Coroutines.
/// </summary>
public class PlayerManager : MonoBehaviour, IDamageable
{
    public event Action OnDie;
    public static PlayerManager Instance { get; private set; }
    [field: SerializeField] public bool Invincible { get; private set; } = false;
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

    public GameObject ragdoll;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public GameObject mesh;
    private Transform lookAt;
    private GameObject ragdollCopy;
    #endregion Public

    // private
    #region Private
    private PlayerStimulus stimulusManager;
    #endregion Private

    #region Awake Start Update
    private void Awake()
    {
        abilitiesManager = GetComponentInChildren<AbilitiesManager>();
        playerMovementManager = GetComponent<PlayerMovement>();
        statManager = GetComponent<PlayerStats>();
        stimulusManager = GetComponentInChildren<PlayerStimulus>();
        statUIManager = GetComponentInChildren<StatUIManager>();
        playerSeenUIManager = GetComponentInChildren<PlayerSeenUIManager>();
        animator = GetComponentInChildren<Animator>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        rigidbody = GetComponent<Rigidbody>();
        audioManager = GetComponent<AudioManager>();
        uiManager = GetComponentInChildren<UIManager>();
        camera = GameObject.Find("Main Camera").transform;
        ragdoll.transform.parent = null;

        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    private GameObject SpawnRagdollCopy()
    {
        GameObject doll = GameObject.Instantiate(ragdoll, ragdoll.transform.position, ragdoll.transform.rotation);
        doll.SetActive(false);
        Transform t = doll.transform.Find("DEF-head");
        return doll;
    }
    private void Start()
    {
        playerStealth = statManager.playerStealth;
        OnDie += Die;
        lookAt = cinemachineVirtualCamera.LookAt;
    }
    private void Update()
    {
        StartCoroutine(PlayerRoutines());
    }
    private void LateUpdate()
    {
        if (target != null)
        {
            transform.SetPositionAndRotation(target.position, target.rotation);
            Debug.Log("Teleporting to " + target.ToString());
            StartCoroutine(ResetTarget(.2f));
        }
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
    private IEnumerator ResetTarget(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        target = null;
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

    public IEnumerator SetInvincibility(float seconds)
    {
        Invincible = true;
        yield return new WaitForSeconds(seconds);
        Invincible = false;
    }

    public void TeleportTo(Vector3 pos, Vector3 rot)
    {
        GameObject go = new GameObject();
        go.transform.position = pos;
        go.transform.rotation = Quaternion.Euler(rot);
        target = go.transform;
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

    public void Damage(int damage)
    {
        if (Invincible) return;

        audioManager.PlaySound("Attacked");
        audioManager.PlaySound("Grunt", "Ouch");

        animator.SetBool("isAttacked", true);
        statManager.DamagePlayer(damage);
        uiManager.Damage();

        if (statManager.health == 0)
        {
            statManager.health = statManager.maxHealth;
            OnDie?.Invoke();
        }
    }
    private void Die()
    {
        Ragdoll(true);
        StartCoroutine(ResetLife());
    }
    private void Ragdoll(bool isRagdoll = false)
    {
        if (!isRagdoll || ragdollCopy == null)
        {
            if (ragdollCopy != null) Destroy(ragdollCopy);
            ragdollCopy = SpawnRagdollCopy();
        }
        Vector3 toTransform = transform.position;
        // toTransform.y += .5f;
        ragdollCopy.transform.position = toTransform;

        playerMovementManager._groundedMovementController.standingController.Stand();
        playerMovementManager.canMove = !isRagdoll;
        cinemachineVirtualCamera.LookAt = isRagdoll ? ragdollCopy.transform : lookAt;
        cinemachineVirtualCamera.Follow = isRagdoll ? null : lookAt;

        mesh.SetActive(!isRagdoll);
        ragdollCopy.SetActive(isRagdoll);
    }

    private IEnumerator ResetLife()
    {
        Invincible = true;
        yield return new WaitForSeconds(1f);
        uiManager.CinematicUIManager.Activate();
        uiManager.TransitionUIManager.Transition(true);
        yield return new WaitForSeconds(1f);
        Ragdoll(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SaveSystemManager.Instance.LoadGame();
        yield return new WaitForSeconds(2f);

        uiManager.TransitionUIManager.Transition(false);
        uiManager.CinematicUIManager.Deactivate();
        Invincible = false;
    }
}
