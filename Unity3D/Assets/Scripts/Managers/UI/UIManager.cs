using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    StarterAssetsInputs _inputs;
    public static UIManager Instance { get; private set; }
    public StatUIManager StatUIManager { get; private set; }
    public PauseManager PauseManager { get; private set; }
    public ExternalUIManager ExternalUIManager { get; private set; }
    public SaveUIManager SaveUIManager { get; private set; }
    public InteractableUIManager InteractableUIManager { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        StatUIManager = GetComponentInChildren<StatUIManager>();
        PauseManager = GetComponentInChildren<PauseManager>();
        ExternalUIManager = GetComponentInChildren<ExternalUIManager>();
        SaveUIManager = GetComponentInChildren<SaveUIManager>();
        InteractableUIManager = GetComponentInChildren<InteractableUIManager>();
    }

    private void Start()
    {
        _inputs = StarterAssetsInputs.Instance;
    }

    private void Update()
    {
        HandleMenuUI();
    }

    private void HandleMenuUI()
    {
        if (PauseManager.isPaused() && SaveUIManager.canOpenSaveDialog)
            SaveUIManager.canOpenSaveDialog = false;
        else if (SaveUIManager.isDialogOpen() && PauseManager.canPause)
            PauseManager.canPause = false;
        else
        {
            SaveUIManager.canOpenSaveDialog = true;
            PauseManager.canPause = true;
        }
    }
}
