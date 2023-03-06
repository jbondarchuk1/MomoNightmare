using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;

public class Pickup : AbilityBase, IPoolUser
{
    public override Abilities Ability { get; } = Abilities.Pickup;
    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "PickupOrb";

    #region Exposed in Editor
    [Header("Initialization References")]
    [SerializeField] private Transform holdParent;
    [SerializeField] private Transform cam;

    [Header("Settings")]
    [SerializeField] private float pickupRange = 5f;
    [SerializeField] private float castRadius = 1f;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float shootForce = 150f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private Animator animator;

    #endregion Exposed in Editor

    #region Private
    private StarterAssetsInputs _inputs;
    private GameObject heldObj;
    private int heldObjLayer = 0;
    private Vector3 originalParentPos;
    private GameObject orb;
    private SelectHandler SelectHandler { get; set; }
    #endregion Private
    protected void Start()
    {
        ObjectPooler = ObjectPooler.Instance;
        originalParentPos = new Vector3(holdParent.localPosition.x, holdParent.localPosition.y, holdParent.localPosition.z);
        _inputs = StarterAssetsInputs.Instance;
        SelectHandler = new SelectHandler();
    }
    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        GameObject lookObj = CheckForInteractableObject();
        if (lookObj == null) yield return wait;
        

        // aim and click - pickup or shoot
        if (_inputs.actionPressed)
        {
            if (heldObj == null && lookObj != null)
                PickupObject(lookObj);
            else if (heldObj != null)
                ShootObject(heldObj);
            _inputs.actionPressed = false;
            yield return wait;
        }
        // not aim - drop
        else if (!_inputs.mouseR && heldObj != null)
            DropObject();
        else if (lookObj != null && !SelectHandler.isSelected())
            SelectHandler.Select(lookObj.transform);



        if ((lookObj == null || heldObj != null) && SelectHandler.isSelected()) 
            SelectHandler.Deselect();

        // handle move always if we have an object
        if (heldObj != null)
        {
            MoveObject();
            HandleHoldPointDistance();
        }
        else ResetHoldPointDistance();
    }
    public override void EnterAbility()
    {
        DropObject();
    }
    public override void ExitAbility()
    {
        DropObject();
    }
    private GameObject CheckForInteractableObject()
    {
        if (heldObj == null)
        {
            Vector3 point1 = cam.transform.position;
            Vector3 forward = cam.forward;
            Ray ray = new Ray(point1, forward);
            Vector3 point2 = ray.GetPoint(pickupRange);
            RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, castRadius, forward, pickupRange, LayerManager.GetMask(LayerManager.Layers.Interactable));
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.name != "Player")
                {
                    if (hit.transform.gameObject.TryGetComponent(out InteractableObject interactableObj))
                    {
                        if (interactableObj.CanPickUp())
                            return hit.transform.gameObject;
                    }
                }
            }
        }
        return null;
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
        animator.SetBool("isHoldingTorch", true);

        if (obj.TryGetComponent(out Rigidbody _))
        {
            heldObj = obj;
            heldObjLayer = obj.layer;
            heldObj.layer = LayerManager.GetLayer(LayerManager.Layers.Target);
            orb = ObjectPooler.SpawnFromPool(Tag, heldObj.transform.position, Quaternion.identity);
            orb.transform.parent = heldObj.transform;
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
    private void DropObject()
    {
        animator.SetBool("isHoldingTorch", false);

        if (heldObj == null)
        {
            heldObjLayer = 0;
            return;
        }
        else if (heldObj.TryGetComponent(out Rigidbody rb))
        {
            heldObj.layer = heldObjLayer;
            Vector3 normalizedVelocity = rb.velocity.normalized;
            heldObj = null;
            rb.velocity = normalizedVelocity;
        }
        if (orb != null)
        {
            orb.SetActive(false);
            orb.transform.parent = null;
            orb = null;
        }

    }
    private void ShootObject(GameObject obj)
    {
        Rigidbody rb;
        if (obj.TryGetComponent(out rb))
        {
            rb.AddForce(cam.forward * shootForce);
        }
        DropObject();
    }
}
