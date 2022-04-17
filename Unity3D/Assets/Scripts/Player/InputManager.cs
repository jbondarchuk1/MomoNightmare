using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public bool crouch = false;
    public bool sprint;
    public bool jump = false;
    public bool l = false;
    public bool r = false;
    public float moveAmount;
    public Vector2 mouseVal;
    public bool paused = false;
    public Vector2 mousePos;


    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Crouch.performed += i => crouch = !crouch;
            playerControls.PlayerMovement.Jump.performed += i => jump = !jump;

            playerControls.PlayerMovement.Sprint.performed += i => sprint = true;
            playerControls.PlayerMovement.Sprint.canceled += i => sprint = false;
            playerControls.Mouse.MouseLook.Enable();
            playerControls.Other.PauseExit.performed += i =>
            {
                paused = !paused;
                Debug.Log("paused = " + paused);
            };

            playerControls.Mouse.MouseLClick.performed += i => l = !l;
            playerControls.Mouse.MouseRClick.performed += i => r = !r;

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }
    private void Update()
    {
        HandleLook();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
    }
    public void HandleLook()
    {
        mouseVal = playerControls.Mouse.MouseLook.ReadValue<Vector2>();
        mousePos = playerControls.Mouse.MousePosition.ReadValue<Vector2>();
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

    }

}
