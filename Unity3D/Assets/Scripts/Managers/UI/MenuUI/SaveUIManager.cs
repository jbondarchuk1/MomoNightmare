using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class SaveUIManager : MonoBehaviour, IUIDialog
{
    private bool canOpen = true;

    SaveSystemManager saveManager;
    [SerializeField] private GameObject SaveDialogUI;
    private void Start()
    {
        saveManager = SaveSystemManager.Instance;
    }

    public void Save()
    {
        saveManager.SaveGame();
        CloseSaveDialog();
    }

    public void OpenSaveDialog()
    {
        if (!IsDialogOpen())
        {
            Cursor.lockState = Cursor.lockState = CursorLockMode.Confined;
            SaveDialogUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void CloseSaveDialog()
    {
        if (IsDialogOpen())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            SaveDialogUI.SetActive(false);
        }
    }
    public bool CanOpen() => canOpen;
    public bool IsDialogOpen() => SaveDialogUI.activeInHierarchy;
    public void SetCanOpen(bool canOpen) => this.canOpen = canOpen;
    public bool IsOpen() => IsDialogOpen();
    public void Open() => OpenSaveDialog();
    public void Close() => CloseSaveDialog();

    public void Toggle()
    {
        if (IsOpen() || !CanOpen()) Close();
        else Open();
    }
}
