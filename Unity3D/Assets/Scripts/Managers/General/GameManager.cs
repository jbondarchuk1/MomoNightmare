using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Start()
    {
        if (Instance == null) Instance = this;
    }
    
    public static IEnumerator SceneChange(int idx)
    {
        yield return new WaitForSeconds(1);
        PlayerManager.Instance.uiManager.TransitionUIManager.Transition(true);
        yield return new WaitForSeconds(1);
        PlayerManager.Instance.uiManager.TransitionUIManager.Transition(false);

        yield return new WaitForSeconds(1);

    }
}
