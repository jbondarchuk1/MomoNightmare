using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeDoor : Door
{
    public int newSceneIndex = 0;
    public override void Activate()
    {
        StartCoroutine(load());
        base.Activate();
    }
    private IEnumerator load()
    {
        PlayerManager.Instance.uiManager.TransitionUIManager.Transition(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(newSceneIndex);
        yield return new WaitForSeconds(1);
        PlayerManager.Instance.uiManager.TransitionUIManager.Transition(false);
    }
}
