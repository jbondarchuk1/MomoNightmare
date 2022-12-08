using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUIManager : MonoBehaviour
{
    StarterAssets.StarterAssetsInputs _inputs;

    public bool canOpenSaveDialog = true;

    SaveSystemManager saveManager;
    [SerializeField] private GameObject SaveDialogUI;
    private void Start()
    {
        _inputs = StarterAssets.StarterAssetsInputs.Instance;
        saveManager = SaveSystemManager.Instance;
    }
    private void Update()
    {
        if (_inputs.pause && isDialogOpen())
        {
            _inputs.pause = false;
            CloseSaveDialog();
        }
    }
    public void Save()
    {
        saveManager.SaveGame();
        CloseSaveDialog();
    }

    public void OpenSaveDialog()
    {
        if (!isDialogOpen())
        {
            Cursor.lockState = Cursor.lockState = CursorLockMode.Confined;
            SaveDialogUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void CloseSaveDialog()
    {
        if (isDialogOpen())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            SaveDialogUI.SetActive(false);
        }
    }
    public bool isDialogOpen() => SaveDialogUI.activeInHierarchy;
}
