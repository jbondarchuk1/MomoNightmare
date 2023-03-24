using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyController))]
public class EnemyManager : MonoBehaviour, IDamageable
{
    #region Exposed In Inspector
        [Header("Prefab Properties")]
        public bool canDetonate = true;
        public bool canZombify = true;
        public GameObject activeModel;
        public GameObject ragdollModel;
        public GameObject canvas;
        public GameObject bloodEffect;
        public Transform hips;
    #endregion Exposed In Inspector
    #region Hidden
        [HideInInspector] private bool alive = true;
        [HideInInspector] public EnemySoundListener SoundListener { get; set; }
        [HideInInspector] public EnemyStats enemyStats;
        [HideInInspector] public PlayerStealth PlayerStealth;
    #endregion Hidden
    #region Private
        protected EnemyController enemyController;
        protected ThirdPersonCharacter thirdPersonCharacter;
        protected EnemyNavMesh enemyNavMesh;
        protected NavMeshAgent navMeshAgent;
        protected Collider capsuleCollider;
        protected Dictionary<string, int> FovHash { get; set; } = new Dictionary<string, int>();
    #endregion Private
    public EnemyUIManager enemyUIManager { get; private set; }
    public EnemyStateManager esm { get; private set; }
    public FOV fov;
    public Animator animator;
    public Rigidbody[] childrenRigidbodies { get; set; } = null;
    public EnemyAnimationEventHandler enemyAnimationEventHandler;
    [SerializeField] private FallDamage fallDamage;
    private bool isFalling = false;

    #region Start and Update

    protected void Start()
    {
        PlayerStealth = PlayerManager.Instance.statManager.playerStealth;
        esm = GetComponentInChildren<EnemyStateManager>();
        SoundListener = new EnemySoundListener(esm.Overrides, esm);
        enemyController = GetComponentInChildren<EnemyController>();
        fov = GetComponent<FOV>();
        thirdPersonCharacter = GetComponentInChildren<ThirdPersonCharacter>();
        enemyNavMesh = GetComponentInChildren<EnemyNavMesh>();
        enemyStats = GetComponentInChildren<EnemyStats>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        enemyUIManager = GetComponentInChildren<EnemyUIManager>();
        enemyAnimationEventHandler = GetComponentInChildren<EnemyAnimationEventHandler>();
        findChildrenRigidbodies();

        if (FovHash.Count == 0)
        {
            for (int i = 0; i < fov.fovValues.Count; i++)
                FovHash.Add(fov.fovValues[i].name, i);
        }
    }
    protected void Update()
    {
        HandlePlayerVisibility();
        if (enemyStats.health <= 0)
        {
            Die();
            Debug.Log("End dead");
        }
        animator.SetBool("isDead", !alive);
        HandleFallDamage();
    }
    private void HandleFallDamage()
    {
        if (isFalling && thirdPersonCharacter.IsGrounded)
        {
            Damage(fallDamage.EndFall(transform.position.y));
            isFalling = false;
        }

        if (!isFalling && !thirdPersonCharacter.IsGrounded)
        {
            fallDamage.StartFall(transform.position.y);
            isFalling = true;
        }
    }
    #endregion Start and Update
    public GameObject Die()
    {
        alive = false;
        ToggleRagdoll(true);
        return ragdollModel;
    }
    public void Damage(int damage)
    {
        Damage(damage, false);
    }
    public void Damage(int damage, bool fallBack)
    {
        enemyStats.Damage(damage);
        if (!fallBack)
        {
            animator.SetBool("isDamaged", true);
            enemyAnimationEventHandler.OnEndDamage += EndDamage;
        }
    }

    private void EndDamage()
    {
        animator.SetBool("isDamaged", false);
        enemyAnimationEventHandler.OnEndDamage -= EndDamage;
    }

    private void HandlePlayerVisibility()
    {
        int fovIndex;
        if (PlayerStealth.Crouching && PlayerStealth.StealthZone != null) FovHash.TryGetValue("CrouchAndStealthZone", out fovIndex); // Crouch in Stealth Zone
        else if (PlayerStealth.Crouching) FovHash.TryGetValue("Crouch", out fovIndex); // Crouch
        else if (PlayerStealth.Jumping) FovHash.TryGetValue("Jump", out fovIndex); // Jump
        else if (PlayerStealth.StealthZone != null) FovHash.TryGetValue("Stealth", out fovIndex); // Stealth Zone
        else FovHash.TryGetValue("Default", out fovIndex); // default
        fov.currFOVIdx = fovIndex;
    }
    private void ToggleRagdoll(bool isRagdoll)
    {
        Behaviour[] behaviours = new Behaviour[]
        {
            esm, enemyController, fov, thirdPersonCharacter,
            enemyNavMesh, enemyStats, animator, navMeshAgent
        };
        foreach (Behaviour b in behaviours)
            b.enabled = !isRagdoll;

        ragdollModel.SetActive(isRagdoll);
        activeModel.SetActive(!isRagdoll);
        capsuleCollider.enabled = !isRagdoll;
        canvas.SetActive(!isRagdoll);

        List<GameObject> toDespawn = new List<GameObject>();
        Transform ragdollParent = ragdollModel.transform.GetChild(0);
        for (int i = 0; i < ragdollParent.childCount; i++)
        {
            Transform t = ragdollParent.GetChild(i);
            if (!t.gameObject.name.Contains("Skull") && !t.gameObject.name.Contains("Body"))
                toDespawn.Add(t.gameObject);
        }

        StartCoroutine(Delete(5f, toDespawn));
    }
    private IEnumerator Delete(float wait, List<GameObject> objects)
    {
        yield return new WaitForSeconds(wait);
        foreach (GameObject obj in objects) obj.SetActive(false);
    }
    private void findChildrenRigidbodies()
    {
        this.childrenRigidbodies = ragdollModel.GetComponentsInChildren<Rigidbody>();
    }
}
