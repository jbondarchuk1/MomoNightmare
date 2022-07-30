using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class Scan : ProjectileAbility, IPoolUser
{
    public LayerMask targetMask; // Enemy Layer
    public LayerMask obstructionMask; // obstruction and ground layer
    public float radius = 1f;
    public float distance = Mathf.Infinity;
    public Transform castOrigin; // Camera

    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "EnemyHighlight";
    protected Select spawnedSelect;

    protected void Start()
    {
        ObjectPooler = GameObject.Find("-- Pooler --").GetComponent<ObjectPooler>();
    }

    public void Select(Transform t)
    {
        if (spawnedSelect == null)
        {
            GameObject spawnedObject = ObjectPooler.SpawnFromPool(Tag, t.position, Quaternion.identity);
            spawnedSelect = spawnedObject.GetComponent<Select>();
            spawnedSelect.follow = t;
        }
    }
    public void Deselect()
    {
        if (spawnedSelect != null)
        {
            spawnedSelect.selected = false;
            spawnedSelect = null;

        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(HandleScan());
    }

    private GameObject CheckForEnemy()
    {
        Transform castCam = castOrigin;
        Ray ray = new Ray(castCam.transform.position, castCam.transform.forward);

        GameObject enemy = CapsuleRayShoot(20f,2f,ray, targetMask, obstructionMask);

        if (enemy != null)
        {
            GameObject canvas = enemy.GetComponent<EnemyManager>().canvas;
            if (canvas.layer != LayerMask.NameToLayer("PriorityUI"))
                Select(enemy.transform);
            else Deselect();
        }
        else Deselect();
        return enemy;
    }

    private IEnumerator HandleScan()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        GameObject enemy = CheckForEnemy();

        if (_inputs.mouseL && _inputs.mouseR)
        {
            _inputs.mouseL = false;
            if (GetWaitComplete(this.endTime))
            {
                this.endTime = Time.time + this.coolDownTimer;

                if (enemy != null)
                {
                    Debug.Log("Scanned Enemy");
                    GameObject canvas = enemy.GetComponent<EnemyManager>().canvas;
                    canvas.SetActive(true);
                    canvas.layer = LayerMask.NameToLayer("PriorityUI");
                    enemy.GetComponentInChildren<EnemyUIManager>().SetLayer("PriorityUI");
                }
            }
        }

        yield return wait;
    }

}
