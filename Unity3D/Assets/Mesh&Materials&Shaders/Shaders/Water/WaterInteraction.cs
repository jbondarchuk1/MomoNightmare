using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInteraction : MonoBehaviour
{

    [SerializeField]
    RenderTexture rt;
    [SerializeField]
    Transform target;
    // Start is called before the first frame update
    void Awake()
    {
        Shader.SetGlobalTexture("_GlobalEffectRT", rt);
        Shader.SetGlobalFloat("_OrthographicCameraSize", 15f);
    }

    private void Update()
    {
        Vector3 position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        Shader.SetGlobalVector("_Position", position);
    }


}
