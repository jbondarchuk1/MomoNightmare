using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClingObject : MonoBehaviour
{
    public List<Transform> stops = new List<Transform>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("STOP"))
            {
                stops.Add(transform.GetChild(i));
            }
        }
    }

    public float MinDistanceToStop(Transform player)
    {
        Vector2 playerXZ = new Vector2(player.position.x, player.position.z);
        float min = Mathf.Infinity;
        for (int i = 0; i < stops.Count; i++)
        {
            float dist = Vector2.Distance(playerXZ, new Vector2(stops[i].position.x, stops[i].position.z));
            if (dist <= min){
                min = dist;
            }
        }
        return min;
    }
}
