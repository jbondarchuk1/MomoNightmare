using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootEffectsHandler : MonoBehaviour
{
    public PlayerTracks tracks;
    private PlayerMovement movement;

    private void Start()
    {
        movement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (movement._speed == movement.SprintSpeed)
        {
            Transform follow = transform;
            Quaternion followRotation = transform.rotation;
            followRotation.x = tracks.transform.rotation.x;
            tracks.transform.position = follow.position;
            tracks.transform.rotation = followRotation;
            tracks.ToggleTracks();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        tracks.ResetTracks();
    }

}
