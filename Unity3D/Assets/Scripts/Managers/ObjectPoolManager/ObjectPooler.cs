using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// **Singleton**
/// Pool of objects set for the game to allow reuse and instantiation of gameobjects. Helpful for spawning lots of objects at once.
/// 
/// Author: Jason Bondarchuk
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }
    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    public List<Pool> pools;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else GameObject.Destroy(this);
    }
    private void Start()
    {

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);

                objectPool.Enqueue(obj);
                
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        try
        {
            if (poolDictionary.ContainsKey(tag))
            {
                GameObject spawnObject = poolDictionary[tag].Dequeue();
                spawnObject.SetActive(true);
                spawnObject.transform.position = position;
                spawnObject.transform.rotation = rotation;
                IPooledObject pooledObj = spawnObject.GetComponent<IPooledObject>();

                if (pooledObj != null)
                {
                    pooledObj.OnObjectSpawn();
                }

                poolDictionary[tag].Enqueue(spawnObject);
                return spawnObject;
            }
            else
            {
                Debug.LogWarning("Unable to get Object pool from Dictionary");
                return null;
            }
        }
        catch(Exception ex)
        {
            Debug.LogWarning("Error getting key: " + tag + " from Object Pool");
            Debug.LogWarning(ex.Message);
            return null;
        }

    }
}
