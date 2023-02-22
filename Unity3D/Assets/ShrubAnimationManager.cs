using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrubAnimationManager : MonoBehaviour
{
    private Animator shrubAnimator;
    
    void Start()
    {
        shrubAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("something happening");
        shrubAnimator.SetBool("isShaking", true);
        StartCoroutine(resetShake());
    }
    private IEnumerator resetShake()
    {
        yield return new WaitForSeconds(.2f);
        shrubAnimator.SetBool("isShaking", false);
    }

}
