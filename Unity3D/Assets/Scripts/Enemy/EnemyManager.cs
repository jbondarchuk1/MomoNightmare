using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyManager : MonoBehaviour
{
    private bool alive = true;

    [HideInInspector] public EnemyStateManager esm;
    private EnemyController enemyController;
    private FOV fov;
    private ThirdPersonCharacter thirdPersonCharacter;
    private EnemyNavMesh enemyNavMesh;
    private EnemyStats enemyStats;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Collider capsuleCollider;
    public GameObject activeModel;
    public GameObject ragdollModel;
    public GameObject canvas;

    public GameObject bloodEffect;
    public Transform hips;
    [HideInInspector] public PlayerStealth playerStealth;
    private Dictionary<string, int> fovHash = new Dictionary<string, int>();

    protected void Start()
    {
        playerStealth = GameObject.Find("Player").GetComponent<PlayerStats>().playerStealth;
        esm = GetComponentInChildren<EnemyStateManager>();
        enemyController = GetComponentInChildren<EnemyController>();
        fov = GetComponentInChildren<FOV>();
        thirdPersonCharacter = GetComponentInChildren<ThirdPersonCharacter>();
        enemyNavMesh = GetComponentInChildren<EnemyNavMesh>();
        enemyStats = GetComponentInChildren<EnemyStats>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        if (fovHash.Count == 0)
        {
            for (int i = 0; i < fov.fovValues.Count; i++)
            {
                fovHash.Add(fov.fovValues[i].name, i);
            }
        }
    }
    protected void Update()
    {
        ImplementPlayerStealth();
        if (enemyStats.health <= 0 && alive)
            Die();
    }

    public void ToggleRagdoll(bool isRagdoll)
    {

        Behaviour[] behaviours = new Behaviour[]
        {
            esm, enemyController, fov, thirdPersonCharacter,
            enemyNavMesh, enemyStats, animator, navMeshAgent
        };
        foreach (Behaviour b in behaviours)
        {
            b.enabled = !isRagdoll;
        }

        ragdollModel.SetActive(isRagdoll);
        activeModel.SetActive(!isRagdoll);
        capsuleCollider.enabled = !isRagdoll;
        canvas.SetActive(!isRagdoll);
    }

    public GameObject Die()
    {
        alive = false;
        MakeBloodEffect();
        ToggleRagdoll(true);
        return ragdollModel;
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

    public void DamageEnemy(int damage)
    {
        Debug.Log("Damaging Enemy");
        enemyStats.Damage(damage);
    }

    public void ImplementPlayerStealth()
    {
        int fovIndex = 0;
        if (playerStealth == null)
        {
            // Debug.Log("No Player Stealth Stats");
            return;
        }
        if (playerStealth.Crouching)
        {
            if (playerStealth.StealthZone != null)
            {
                fovHash.TryGetValue("CrouchAndStealthZone", out fovIndex);
            }
            // crouching
            fovHash.TryGetValue("Crouch", out fovIndex);
        }
        else if (playerStealth.Jumping)
        {
            // jumping
            fovHash.TryGetValue("Jump", out fovIndex);
        }
        //else if (playerStealth.Landing) { }
        else if (playerStealth.StealthZone != null)
        {
            int level = playerStealth.StealthZone.StealthLevel;
            // do something
            fovHash.TryGetValue("Stealth", out fovIndex);
        }
        else
        {
            // standing
            fovHash.TryGetValue("Default", out fovIndex);
        }
        fov.currFOVIdx = fovIndex;
    }
}
