using UnityEngine;

// TODO: Change to ComponentSystem
public class SubsceneManager : MonoBehaviour
{
    // private SceneSystem sceneSystem;
    //protected override void OnCreate()
    //{
    //    sceneSystem = World.GetOrCreateSystem<SceneSystem>();
    //}

    //void Update() // void OnUpdate()
    //{
    //    Transform playerTransform = PlayerManager.Instance.transform;
    //    foreach (SubScene scene in SubsceneReferences.Instance.Subscenes)
    //    {
    //        if (Vector3.Distance(transform.position, scene.transfrom.position) <= scene.loadDistance) 
    //        {
    //            LoadSubScene(scene);
    //        }
    //        else
    //        {
    //            UnloadSubScene(scene);
    //        }
    //    }
    //}
    //private void LoadSubScene(SubScene subScene)
    //{
    //    sceneSystem.LoadSceneAsync(subScene.SceneGUID);
    //}
    //private void UnloadSubScene(SubScene subScene)
    //{
    //    sceneSystem.UnloadScene(subScene.SceneGUID);
    //}
}
