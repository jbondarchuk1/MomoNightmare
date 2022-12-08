using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    StarterAssetsInputs _inputs;

    public bool canPause = true;

    [SerializeField] private GameObject statUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject saveUI;

    void Start()
    {
        _inputs = StarterAssetsInputs.Instance;

        pauseUI.SetActive(false);
        Resume();
    }

    void Update()
    {
        if (!canPause)
        {
            if (isPaused()) Resume();
            return;
        }

        if (_inputs.pause && !isPaused())
        {
            Pause();
        }
        else if (isPaused() && !_inputs.pause)
        {
            Resume();
        }
    }
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
        Cursor.lockState = CursorLockMode.Locked;
        statUI.SetActive(true);
        pauseUI.SetActive(false);

        Time.timeScale = 1f;
        
        _inputs.pause = false;
    }
    public bool isPaused() =>  pauseUI.activeInHierarchy;

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
