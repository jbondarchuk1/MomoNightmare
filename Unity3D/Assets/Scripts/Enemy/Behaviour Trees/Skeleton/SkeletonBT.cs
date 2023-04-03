using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Cinemachine;
using static BehaviourTree.Exclamation;

public class SkeletonBT : BehaviourTree.Tree
{
    [Header("Behaviour Settings")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] EnemyGroupManager enemyGroupManager;

    #region private
        [Header("Global To Entire Tree")]
        AudioManager enemyAudioMaanger;
        EnemyUIManager enemyUIManager;
        FOV fov;
        EnemyStats enemyStats;
        Transform _transform;
        EnemyNavMesh enemyNavMesh;
        Animator enemyAnimator;
        EnemyAnimationEventHandler enemyAnimationEventHandler;
        EnemySoundListener enemySoundListener;
    #endregion

    [Header("Node Data Settings")]
    [SerializeField] private PatrolValues patrolValues;
    [SerializeField] private PatrolValues alertValues;
    [SerializeField] private CinemachineVirtualCamera faceCam;

    protected virtual void Initialize(Node root)
    {
        enemyNavMesh = enemyManager.enemyNavMesh;
        enemyAnimator = enemyManager.animator;
        enemyStats = enemyManager.enemyStats;
        enemyAudioMaanger = enemyManager.enemyAudioManager;
        enemyUIManager = enemyManager.enemyUIManager;
        enemyAnimationEventHandler = enemyManager.enemyAnimationEventHandler;
        _transform = enemyManager.transform;
        fov = enemyManager.fov;
        enemySoundListener = enemyManager.SoundListener;

        root.SetData("NavMesh", enemyNavMesh);
        root.SetData("EnemyManager", enemyManager);
        root.SetData("Animator", enemyAnimator);
        root.SetData("Stats", enemyStats);
        root.SetData("AudioManager", enemyAudioMaanger);
        root.SetData("UIManager", enemyUIManager);
        root.SetData("AnimationEventHandler", enemyAnimationEventHandler);
        root.SetData("Transform", _transform);
        root.SetData("Tree", this);
        root.SetData("FOV", fov);
        root.SetData("EnemyGroupManager", enemyGroupManager);
        root.SetData("EnemyManager", enemyManager);
        root.SetData("EnemySoundListener", enemySoundListener);

        root.Initialize();
    }
    protected override Node SetupTree()
    {
        Node root = new Selector(
            // Incapacitated
            new Dead(),
            new Falling(),
            new BehaviourTree.TakeDamage(),

            // Attack
            new Sequence(
                new CheckAttack(),
                new Iterator(
                    new Exclamation(faceCam),
                    new RelaySelector(
                        new BehaviourTree.Attack(),
                        new BehaviourTree.Chase()
                    )
                )
            ),

            // Suspicious
                new CheckSus(faceCam,
                    new Iterator(
                        new Curious(),
                        new Search(),
                        new SusWait(5f)
                )),

            // Alert
            new Sequence(
               new CheckAlert(),
               new Exclamation(null),
               new BehaviourTree.Patrol(alertValues)
            ),

            // Patrol
            new BehaviourTree.Patrol(patrolValues)
            );

        Initialize(root);
        return root;
    }
}