using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInteractableObject : InteractableObject, IActivatable
{
    private SaveUIManager _saveUIManager;
    private bool isActivated = false;

    private new void Start()
    {
        _saveUIManager = UIManager.Instance.SaveUIManager;
        base.Start();
    }
    private void Update()
    {
        // force deselect so that the ui doesnt conflict with the save dialog
        if (isActivated)
        {
            Deselect();
            if (!_saveUIManager.isDialogOpen())
                Deactivate();
        }
    }
    public void Activate()
    {
        Deselect();
        isActivated = true;
        _saveUIManager.OpenSaveDialog();
    }
    public void Deactivate()
    {
        isActivated = false;
        _saveUIManager.CloseSaveDialog();
    }
}
