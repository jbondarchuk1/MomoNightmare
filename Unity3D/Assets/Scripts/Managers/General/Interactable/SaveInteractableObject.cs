using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInteractableObject : InteractableObject, IActivatable
{
    private SaveUIManager _saveUIManager;
    private bool isActive = false;

    private new void Start()
    {
        _saveUIManager = UIManager.Instance.SaveUIManager;
        base.Start();
    }
    private void Update()
    {
        // force deselect so that the ui doesnt conflict with the save dialog
        if (isActive)
        {
            Deselect();
            if (!_saveUIManager.IsDialogOpen())
                Deactivate();
        }
    }
    public void Activate()
    {
        if (isActive) return;

        Deselect();
        _saveUIManager.OpenSaveDialog();
        StartCoroutine(WaitSetActive(true));

    }
    public IEnumerator WaitSetActive(bool active)
    {
        yield return new WaitForSeconds(.1f);
        isActive = active;

    }
    public void Deactivate()
    {
        if (!isActive) return;

        isActive = false;
        _saveUIManager.CloseSaveDialog();
    }
    public bool isActivated() => isActive;
}
