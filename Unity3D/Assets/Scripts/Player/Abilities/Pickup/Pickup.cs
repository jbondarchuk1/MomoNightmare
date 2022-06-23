using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
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

    private void Start()
    {
        originalParentPos = new Vector3(holdParent.localPosition.x, holdParent.localPosition.y, holdParent.localPosition.z);
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(HandlePickup());
    }
    private IEnumerator HandlePickup()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);

        // aim and click - pickup or shoot
        if (_inputs.mouseL && _inputs.mouseR)
        {
            _inputs.mouseL = false;

            if (heldObj == null)
            {
                if (Physics.Raycast(cam.transform.position, cam.TransformDirection(Vector3.forward), out RaycastHit hit, pickupRange))
                {
                    if (hit.transform.gameObject.name != "Player")
                        PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                ShootObject(heldObj);
            }
        }
        // not aim - drop
        else if (!_inputs.mouseR && heldObj != null)
        {
            DropObject(heldObj);
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
        }
    }

    private void OnScroll()
    {
        //    if ()
        //    holdParent.localPosition.z += 1;
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
        if (!rb.gameObject.TryGetComponent(out BodyPartGore gore))
        {
            heldObj.layer = heldObjLayer;
        }
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
