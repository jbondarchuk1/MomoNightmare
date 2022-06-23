using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public ParticleSystem pineParticle;
    private Terrain terrain;
    private TreeInstance[] trees;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Trees");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
