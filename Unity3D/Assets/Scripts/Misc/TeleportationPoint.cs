using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class TeleportationPoint : MonoBehaviour
{
    private TeleporterManager teleporterManager;
    [SerializeField] private Scene toScene;
    private Collider col;
    // Start is called before the first frame update
    void Start()
    {
        teleporterManager = GameObject.Find("GameManager").GetComponent<GameManager>().teleporterManager;
        col = GetComponent<Collider>();
        if (col.isTrigger == false) col.isTrigger = true;
        if (toScene == null) toScene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == LayerMask.GetMask("Player")) teleporterManager.Teleport(toScene.buildIndex);
    }

}
