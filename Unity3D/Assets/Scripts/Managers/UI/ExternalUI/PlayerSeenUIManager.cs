using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerSeenUIManager : MonoBehaviour
{
    private GameObject arrow;
    private void Start()
    {
        Transform t = transform.GetChild(0);
        arrow = t.gameObject;
        arrow.SetActive(false);
    }
    public void TurnOnArrow(int angle)
    {
        arrow.SetActive(true);
        RectTransform arrowTransform = arrow.GetComponent<RectTransform>();
        Quaternion arrowRot = arrowTransform.rotation;

        arrowRot.eulerAngles = new Vector3(arrowRot.x, arrowRot.y, angle - 90);
        arrowTransform.rotation = arrowRot;
    }
    public void TurnOffArrow()
    {
        arrow.SetActive(false);
    }

}
