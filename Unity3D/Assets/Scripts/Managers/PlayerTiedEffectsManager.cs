using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game contains various effects, some are tied to the player, some might not be. All effects here follow the player in the worldspace.
/// Since rotation is not the same as the player, these are kept in a separate game object. This allows zones and different game events to
/// toggle these on and off.
/// </summary>
public class PlayerTiedEffectsManager : MonoBehaviour
{
    // name of child, index of child
    public Dictionary<string, int> effects = new Dictionary<string, int>
    {
        { "Rain", 0         },
        { "Fog", 1          },
        { "Fireflies", 2    },
        { "Particles", 3    },
        { "Tracks", 4       }
    };
    public Transform follow;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 position = follow.position;
        transform.position = position;

    }

    public GameObject ToggleEffect(string name)
    {
        int childIdx = -1;
        effects.TryGetValue(name, out childIdx);

        if (childIdx >= 0)
        {
            GameObject child = transform.GetChild(childIdx).gameObject;
            child.SetActive(!child.activeSelf);
            return child;
        }
        return null;
    }
    public GameObject ToggleEffect(string name, bool isActive)
    {
        int childIdx = -1;
        effects.TryGetValue(name, out childIdx);

        if (childIdx >= 0)
        {
            GameObject child = transform.GetChild(childIdx).gameObject;
            child.SetActive(isActive);
            return child;
        }
        return null;

    }
}
