using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public StarterAssetsInputs inputManager;
    private bool pause;
    public GameObject statUI;
    public GameObject pauseUI;


    // Start is called before the first frame update
    void Start()
    {
        pause = inputManager.pause;

        pauseUI.SetActive(false);
        initializeUI();
    }


    // Update is called once per frame
    void Update()
    {

        if (inputManager.pause && !pause)
        {
            Pause();
        }
        else if (pause && !inputManager.pause)
        {
            Resume();
        }
    }
    private void initializeUI()
    {
        Resume();
    }
    public void Pause()
    {
        Cursor.lockState = Cursor.lockState = CursorLockMode.Confined;
        statUI.SetActive(false);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        pause = true;
        inputManager.pause = true;
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        statUI.SetActive(true);
        pauseUI.SetActive(false);

        Time.timeScale = 1f;
        
        pause = false;
        inputManager.pause = false;
    }
    public void Quit()
    {
        // TODO: implement save

        // return to start scene
        SceneManager.LoadScene(0);
    }
}
