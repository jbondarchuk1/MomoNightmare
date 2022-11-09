using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatRangeLevel;

public abstract class RangeUIManagerBase: MonoBehaviour
{
    public abstract string TrackedValueName { get; set; }
    [SerializeField] protected GameObject LowUI;
    [SerializeField] protected GameObject MidUI;
    [SerializeField] protected GameObject HighUI;

    public void SetUI(Range range)
    {
        bool low = false;
        bool mid = false;
        bool high = false;

        switch (range)
        {
            case Range.Low:
                low = true;
                break;
            case Range.Middle:
                mid = true;
                break;
            case Range.High:
                high = true;
                break;
        }
        SetUI(low, mid, high);
    }
    private void SetUI(bool low, bool mid, bool high)
    {
        LowUI.SetActive(low);
        MidUI.SetActive(mid);
        HighUI.SetActive(high);
    }
}
