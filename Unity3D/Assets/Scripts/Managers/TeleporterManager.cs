using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handler for swapping scenes both async and one at a time. Async is not implemented, class is untested,
/// written on shinkansen
/// </summary>
public class TeleporterManager : MonoBehaviour
{
    [HideInInspector] public Scene scene;

    #region Teleport Methods
        /// <summary>
        /// Non Asynchronously load a new scene and set to a new scene.
        /// </summary>
        /// <param name="sceneName"></param>
        public void Teleport(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            Teleport(SceneManager.GetSceneByName(sceneName));
        }
        public void Teleport(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
            Teleport(SceneManager.GetSceneAt(sceneIndex));
        }
        /// <summary>
        /// Assuming scene has already been loaded, swap the scenes.
        /// </summary>
        /// <param name="s"></param>
        private void Teleport(Scene s)
        {
            SceneManager.SetActiveScene(s);
            scene = s;
        }
    #endregion Teleport Methods
}
