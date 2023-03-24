using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    StarterAssetsInputs _inputs;
    public static UIManager Instance { get; private set; }
    private bool isWaiting = false;

    #region UIManagers
    public StatUIManager StatUIManager { get; private set; }
    public PauseManager PauseManager { get; private set; }
    public ExternalUIManager ExternalUIManager { get; private set; }
    public SaveUIManager SaveUIManager { get; private set; }
    public InteractableUIManager InteractableUIManager { get; private set; }
    public InventoryUIManager inventoryUIManager { get; private set; }
    public DamagedUIManager DamagedUIManager { get; private set; }
    public TransitionUIManager TransitionUIManager { get; private set; }
    public CinematicUIManager CinematicUIManager { get; private set; }
    #endregion UIManagers

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        StatUIManager = GetComponentInChildren<StatUIManager>();
        PauseManager = GetComponentInChildren<PauseManager>();
        ExternalUIManager = GetComponentInChildren<ExternalUIManager>();
        SaveUIManager = GetComponentInChildren<SaveUIManager>();
        InteractableUIManager = GetComponentInChildren<InteractableUIManager>();
        DamagedUIManager = GetComponentInChildren<DamagedUIManager>();
        inventoryUIManager = GetComponentInChildren<InventoryUIManager>();
        TransitionUIManager = GetComponentInChildren<TransitionUIManager>();
        CinematicUIManager = GetComponentInChildren<CinematicUIManager>();
    }

    private void Start()
    {
        _inputs = StarterAssetsInputs.Instance;
    }

    private void Update()
    {
        HandleUIAvailability();
        StartCoroutine(HandleUIActivation());
    }

    private void HandleUIAvailability()
    {
        int activeIdx = -1;
        IUIDialog[] dialogs = new IUIDialog[]
        {
            SaveUIManager,
            PauseManager,
            inventoryUIManager,
        };
        // find if one is open
        for (int i = 0; i < dialogs.Length; i++)
        {
            IUIDialog dialog = dialogs[i];
            if (dialog.IsOpen()) activeIdx = i;
        }

        // set which ones can be open
        for (int i = 0; i < dialogs.Length; i++)
        {
            if (activeIdx < 0 || i == activeIdx) dialogs[i].SetCanOpen(true);
            else dialogs[i].SetCanOpen(false);
        }
    }
    private IEnumerator HandleUIActivation()
    {
        if (!isWaiting)
        {
            if (_inputs.inventory)
            {
                isWaiting = true;
                inventoryUIManager.Toggle();
                _inputs.inventory = false;
                yield return new WaitForSeconds(.2f);
            }
            else if (_inputs.pause)
            {
                isWaiting = true;
                PauseManager.Toggle();
                _inputs.pause = false;
                yield return new WaitForSeconds(.2f);
            }
        } 
        isWaiting = false;
    }
    public void Damage() => DamagedUIManager.Damage();
}
