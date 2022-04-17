using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracks : MonoBehaviour
{
    private ParticleSystem tracks;
    // Start is called before the first frame update
    void Start()
    {
        tracks = GetComponent<ParticleSystem>();
    }

    public void ToggleTracks()
    {
        if (transform.rotation.x > -10f)
            tracks.Play();
    }

    public void ResetTracks()
    {
        tracks.Stop();
    }
}
