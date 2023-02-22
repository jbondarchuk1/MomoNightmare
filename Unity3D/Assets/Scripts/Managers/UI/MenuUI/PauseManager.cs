using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour, IUIDialog
{
    StarterAssetsInputs _inputs;

    public bool canOpen = true;

    [SerializeField] private GameObject statUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject saveUI;

    void Start()
    {
        _inputs = StarterAssetsInputs.Instance;

        pauseUI.SetActive(false);
        Resume();
    }

    #region MenuButtonMethods
    public void Pause()
    {
        Cursor.lockState = Cursor.lockState = CursorLockMode.Confined;
        statUI.SetActive(false);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        _inputs.pause = true;
    }
    public void Resume()
    {
        if (!IsOpen()) return;
        Cursor.lockState = CursorLockMode.Locked;
        statUI.SetActive(true);
        pauseUI.SetActive(false);

        Time.timeScale = 1f;
        
        _inputs.pause = false;
    }
    public void Quit() => SceneManager.LoadScene(0);
    #endregion MenuButtonMethods

    public void Toggle()
    {
        if (canOpen && !IsOpen()) Pause();
        else Resume();
    }
    public bool CanOpen() => this.canOpen;
    public bool IsOpen() => pauseUI.activeInHierarchy;
    public void Open() => Pause();
    public void Close() => Resume();
    public void SetCanOpen(bool canOpen) => this.canOpen = canOpen;
}
