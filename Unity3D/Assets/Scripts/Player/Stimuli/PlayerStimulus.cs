using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStimulus : StimulusController
{
    public List<Stimulus> stimuli;
    // Start is called before the first frame update
    void Start()
    {
        stimuli = new List<Stimulus>();
        GameObject stims = GameObject.Find("PlayerStimuli");
        for (int i = 0; i < stims.transform.childCount; i++)
        {
            stimuli.Add(stims.transform.GetChild(i).GetComponent<Stimulus>());
        }
    }

    public void HandleStimuli()
    {
        foreach (Stimulus stim in stimuli)
        {
            if (stim.active)
            {
                stim.Emit();
            }
        }
    }

    public Stimulus GetStimByName(string type)
    {
        foreach (Stimulus s in stimuli)
        {
            if (s.name.Contains(type))
            {
                return s;
            }
        }
        return null;
    }
}
