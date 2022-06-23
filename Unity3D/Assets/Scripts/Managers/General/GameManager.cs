using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Scene scene;
    public TeleporterManager teleporterManager;


    /// <summary>
    /// Allow the player to teleport from one door to another.
    /// Depending on where the teleportTo door is we might also swap scenes.
    /// </summary>
    /// <param name="fromIndex"></param>
    public void Teleport(int fromIndex)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Allow the player to teleport from one door to another.
    /// Depending on where the teleportTo door is we might also swap scenes.
    /// </summary>
    /// <param name="fromIndex"></param>
    public void Teleport(string fromName)
    {
        throw new System.NotImplementedException();
    }
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        teleporterManager = TryGetComponent<TeleporterManager>(out teleporterManager) == false ? gameObject.AddComponent<TeleporterManager>() : teleporterManager;
        teleporterManager.scene = scene;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
