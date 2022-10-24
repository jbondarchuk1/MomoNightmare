using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyManager : MonoBehaviour
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
    [HideInInspector] public PlayerStealth PlayerStealth;
    [HideInInspector] public EnemySoundListener SoundListener { get; set; }
    #endregion Hidden

    #region Private
    private bool alive = true;
    private EnemyStateManager esm;
    private EnemyController enemyController;
    private FOV fov;
    private ThirdPersonCharacter thirdPersonCharacter;
    private EnemyNavMesh enemyNavMesh;
    private EnemyStats enemyStats;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Collider capsuleCollider;
    private Dictionary<string, int> FovHash { get; set; } = new Dictionary<string, int>();

    #endregion Private

    #region Start and Update

    protected void Start()
    {
        PlayerStealth = GameObject.Find("Player").GetComponent<PlayerStats>().playerStealth;
        esm = GetComponentInChildren<EnemyStateManager>();
        SoundListener = new EnemySoundListener(esm.Overrides, esm);
        enemyController = GetComponentInChildren<EnemyController>();
        fov = GetComponentInChildren<FOV>();
        thirdPersonCharacter = GetComponentInChildren<ThirdPersonCharacter>();
        enemyNavMesh = GetComponentInChildren<EnemyNavMesh>();
        enemyStats = GetComponentInChildren<EnemyStats>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();

        if (FovHash.Count == 0)
        {
            for (int i = 0; i < fov.fovValues.Count; i++)
                FovHash.Add(fov.fovValues[i].name, i);
        }
    }
    protected void Update()
    {
        HandlePlayerVisibility();
        if (enemyStats.health <= 0 && alive)
            Die();
    }

    #endregion Start and Update

    public GameObject Die()
    {
        alive = false;
        MakeBloodEffect();
        ToggleRagdoll(true);
        return ragdollModel;
    }
    public void DamageEnemy(int damage)
    {
        Debug.Log("Damaging Enemy");
        enemyStats.Damage(damage);
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
    }
    private void MakeBloodEffect()
    {
        if (hips.position != Vector3.zero)
        {
            GameObject blood = Instantiate(bloodEffect, ragdollModel.transform);
            BloodHandler bloodHandler = blood.GetComponent<BloodHandler>();
            bloodHandler.followTarget = hips;
        }
    }

}
