using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : SelectHandler
{
    public StarterAssetsInputs _inputs;
    public float pickupRange = 5f;
    public Transform holdParent;
    private Vector3 originalParentPos;
    public float moveForce = 10f;
    public float shootForce = 150f;
    public Transform cam;

    public float maxDistance = 5f;
    public GameObject heldObj;
    public int heldObjLayer = 0;
    public float radius = 1f;

    protected new void Start()
    {
        originalParentPos = new Vector3(holdParent.localPosition.x, holdParent.localPosition.y, holdParent.localPosition.z);
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(HandlePickup());
    }

    private GameObject CheckForInteractableObject()
    {
        if (heldObj == null)
        {
            Vector3 point1 = cam.transform.position;
            Vector3 forward = cam.forward;
            Ray ray = new Ray(point1, forward);
            Vector3 point2 = ray.GetPoint(pickupRange);
            RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, radius, forward, pickupRange, LayerMask.GetMask("Interactable"));
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.name != "Player")
                {
                    return hit.transform.gameObject;
                }
            }
        }
        return null;
    }

    private IEnumerator HandlePickup()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        GameObject lookObj = CheckForInteractableObject();


        // aim and click - pickup or shoot
        if (_inputs.mouseL && _inputs.mouseR)
        {
            _inputs.mouseL = false;

            if (heldObj == null && lookObj != null)
                PickupObject(lookObj);
            else if (heldObj != null)
                ShootObject(heldObj);
        }
        // not aim - drop
        else if (!_inputs.mouseR && heldObj != null)
        {
            DropObject(heldObj);
        }
        else if (lookObj != null && spawnedSelect == null)
        {
            Select(lookObj.transform);
        }

        if (
                (lookObj == null || heldObj != null)
                && spawnedSelect != null
           )
        {
            Deselect();
        }

        // handle move always if we have an object
        if (heldObj != null)
        {
            MoveObject();
            HandleHoldPointDistance();
        }
        else ResetHoldPointDistance();
        
        
        yield return wait;
    }
    private void ResetHoldPointDistance()
    {
        holdParent.localPosition = originalParentPos;
    }

    private void HandleHoldPointDistance()
    {
        Vector3 holdPos = holdParent.localPosition;
        if (holdPos.z + (float)_inputs.scrollVal * 2f / 3f <= 3f
            && holdPos.z + (float)_inputs.scrollVal * 2f / 3f >= .2f)
                holdPos.z += (float)_inputs.scrollVal * 2f/3f;
        
        holdParent.localPosition = holdPos;
        _inputs.ResetScroll();
    }

    private void PickupObject(GameObject obj)
    {
        Rigidbody rb;
        if (obj.TryGetComponent<Rigidbody>(out rb))
        {
            heldObj = obj;
            heldObjLayer = obj.layer;
            heldObj.layer = LayerMask.NameToLayer("Target");
            Debug.Log("Picking up object");
        }
    }

    private void MoveObject()
    {
        if (heldObj != null)
        {
            float distanceToObj = Vector3.Distance(holdParent.position, heldObj.transform.position);
            float currentSpeed = Mathf.SmoothStep(0f, 20f, distanceToObj / maxDistance);
            currentSpeed += Time.fixedDeltaTime;
            Vector3 direction = holdParent.position - heldObj.transform.position;
            heldObj.GetComponent<Rigidbody>().velocity = direction.normalized * currentSpeed;
        }
    }
    private void DropObject(GameObject obj)
    {
        Rigidbody rb;
        if (obj.TryGetComponent<Rigidbody>(out rb))
        {
            DropObject(rb);
        }
    }
    private void DropObject(Rigidbody rb)
    {
        heldObj.layer = heldObjLayer;
        heldObjLayer = 0;
        heldObj = null;
    }
    private void ShootObject(GameObject obj)
    {
        Rigidbody rb;
        if (obj.TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(cam.forward * shootForce);
            DropObject(rb);
        }
    }
}
