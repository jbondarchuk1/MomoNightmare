using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalUIManager : MonoBehaviour
{

    private PlayerSeenUIManager playerSeenUIManager;

    private void Start()
    {
        playerSeenUIManager = GetComponentInChildren<PlayerSeenUIManager>();
    }
    void Update()
    {
        
    }
}
