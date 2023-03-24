using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class SaveSystemManager : MonoBehaviour
{
    public static SaveSystemManager Instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField] private string fileName = "data.momo"; 

    private GameData gameData; // the one gameData object available at any given time read and written from memory
    private List<GeneralData> allGeneralDataObjects;
    private FileDataHandler fileDataHandler;
    public bool idling = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    private void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.allGeneralDataObjects = FindDataObjects();
        LoadGame();
    }
    private void OnApplicationQuit()
    {
        // SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();
        if (gameData == null) NewGame();
        foreach(GeneralData p in allGeneralDataObjects)
        {
            p.LoadData(gameData);
            gameData.loadedKeys.Add(p.Key);
        }
        gameData.PurgeUnusedGameData();
    }

    public void SaveGame()
    {
        foreach(GeneralData p in allGeneralDataObjects)
        {
            p.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
    }

    private List<GeneralData> FindDataObjects()
    {
        IEnumerable<GeneralData> allData = FindObjectsOfType<MonoBehaviour>()
            .OfType<GeneralData>();
        return allData.ToList();
    }
}
