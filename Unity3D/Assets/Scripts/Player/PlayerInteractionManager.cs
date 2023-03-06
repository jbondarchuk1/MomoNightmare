using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

/// <summary>
/// When an interactable object is in range, it will activate the icon for the object
/// If an interactable object is in range and the player presses the interaction button,
/// the Activate() method is called on the object.
/// 
/// Objects out of range are automatically deselected. Deactivation is handled by the object
/// </summary>
public class PlayerInteractionManager : MonoBehaviour
{
    private StarterAssetsInputs _inputs;
    [SerializeField] private float maxCastDistance = 2f;
    [SerializeField] private float castRadius = 3f;

    private InteractableObject activeSelectObj;
    private void Start()
    {
        _inputs = StarterAssetsInputs.Instance;
    }
    
    /// <summary>
    /// Cast a capsule ray, capture any interactable objects within a max distance.
    /// Activate the closest icon for the interactable object
    ///         for example: Press E to open door + door icon etc
    /// </summary>
    public void HandleRegularInteractions()
    {
        // variable initialization
        LayerMask mask = LayerManager.GetMask(LayerManager.Layers.Interactable);
        Transform cam = PlayerManager.Instance.camera;
        Ray ray = new Ray(cam.position, cam.forward);
        Vector3 point2 = ray.GetPoint(maxCastDistance + 20);
        // Debug.DrawLine(ray.origin, point2, Color.red, 1f);



        bool deselect = true;
        // automatically only selects the first seen object
        
        if (Physics.CapsuleCast(ray.origin, ray.origin, castRadius, cam.forward,out RaycastHit hit, maxCastDistance, mask))
        {
            if (hit.collider.gameObject.TryGetComponent(out InteractableObject obj))
            {
                HandleSelectIcon(obj);
                deselect = false;
            }
        }
        if (deselect) HandleDeselection();
    }

    // every frame update to check user input
    private void Update()
    {
        HandleObjectActivation();
    }
    
    private void HandleObjectActivation()
    {
        if (activeSelectObj == null || !_inputs.interact) return;

        if (activeSelectObj is IActivatable activatable)
        {
            if (!activatable.isActivated()) activatable.Activate();
            else activatable.Deactivate();
            _inputs.interact = false;
        }
    }
    
    /// <summary>
    /// Activate the icon associated with the interactable object
    /// </summary>
    /// <param name="closestObj"></param>
    private void HandleSelectIcon(InteractableObject closestObj)
    {
        if (activeSelectObj != null)
            activeSelectObj.Deselect();
        activeSelectObj = closestObj;
        activeSelectObj.Select();
    }

    private void HandleDeselection()
    {
        if (activeSelectObj == null) return;

        activeSelectObj.Deselect();
        activeSelectObj = null;
    }
}
