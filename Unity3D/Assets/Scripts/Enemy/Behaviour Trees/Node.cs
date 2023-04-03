using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    [System.Serializable]
    public class Node
    {
        [HideInInspector] public NodeState state = NodeState.FAILURE;
        [HideInInspector] public Node parent = null;
        [SerializeField] protected List<Node> children = new List<Node>();
        private Dictionary<string, object> data = new Dictionary<string, object>();

        protected AudioManager audioManager;
        protected EnemyUIManager enemyUIManager;
        protected FOV fov;
        protected PlayerSeenUIManager playerSeenUIManager;
        protected EnemyStats enemyStats;
        protected EnemyNavMesh enemyNavMesh;
        protected Transform transform;
        protected Animator animator;
        protected EnemyAnimationEventHandler enemyAnimationEventHandler;
        protected Tree tree;
        protected EnemySoundListener enemySoundListener;
        protected EnemyGroupManager enemyGroupManager;
        protected EnemyManager enemyManager;

        protected bool isWaiting = false;
        public Node() { }
        public virtual void Initialize()
        {
            audioManager = (AudioManager)GetData("AudioManager");
            enemyNavMesh = (EnemyNavMesh)GetData("NavMesh");
            enemyUIManager = (EnemyUIManager)GetData("UIManager");
            fov = (FOV)GetData("FOV");
            playerSeenUIManager = PlayerManager.Instance.playerSeenUIManager;
            enemyStats = (EnemyStats)GetData("Stats");
            transform = (Transform)GetData("Transform");
            tree = (Tree)GetData("Tree");
            animator = (Animator)GetData("Animator");
            enemyAnimationEventHandler = (EnemyAnimationEventHandler)GetData("AnimationEventHandler");
            enemyGroupManager = (EnemyGroupManager)GetData("EnemyGroupManager");
            enemySoundListener = (EnemySoundListener)GetData("EnemySoundListener");
            enemyManager = (EnemyManager)GetData("EnemyManager");

            foreach (Node child in children)
            {
                child.Initialize();
            }
        }
        public Node(params Node[] children): this()
        {
            foreach (Node child in children) Attach(child);
        }
        public Node(List<Node> children): this()
        {
            foreach (Node child in children) Attach(child);
        }
        private void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }
        public void SetData(string key, object value)
        {
            data[key] = value;
        }
        public object GetData(string key)
        {
            if (data.ContainsKey(key)) return data[key];
            else if (parent == null)
            {
                Debug.LogWarning("Key: " + key + " not in tree. \nCaller: " + this.GetType().ToString());
                return null;
            }
            else return parent.GetData(key);
        }
        public bool ClearData(string key)
        {
            if (data.ContainsKey(key)) return data.Remove(key);
            else if (parent == null) return false;
            else return parent.ClearData(key);
        }
        public virtual NodeState Evaluate() { return NodeState.FAILURE; }

        public void Wait(float delay)
        {
            tree.StartCoroutine(HandleWait(delay));
        }
        private IEnumerator HandleWait(float delay)
        {
            isWaiting = true;
            yield return new WaitForSeconds(delay);
            Debug.Log("Wait over");
            isWaiting = false;
        }

    }
}

