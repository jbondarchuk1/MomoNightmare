using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool crouch = false;
		public bool sprint;
		public bool mouseL = false;
		public bool mouseR = false;
		public bool pause = false;
		public bool menuFState = false;
		public bool menuBState = false;


		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}
		public void OnCrouch(InputValue value)
		{
			CrouchInput();
		}
		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		public void OnMouseL(InputValue value)
		{
			MouseLInput(value.isPressed);
		}
		public void OnMouseR(InputValue value)
		{
			MouseRInput(value.isPressed);
		}
		public void OnPause(InputValue value)
		{
			PauseInput(value.isPressed);
		}
		public void OnMenuForward(InputValue value)
        {
			MenuForwardInput(value.isPressed);
		}
		public void OnMenuBackward(InputValue value)
		{
			MenuBackwardInput(value.isPressed);
		}
#else

#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}
		public void CrouchInput()
		{
			crouch = !crouch;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void MouseLInput(bool newMouseLState)
		{
			mouseL = newMouseLState;
		}
		public void MouseRInput(bool newMouseRState)
		{
			mouseR = !mouseR;
		}
		public void PauseInput(bool newPauseState)
		{
			pause = !pause;
		}
		public void MenuForwardInput(bool newMenuFState)
		{
			menuFState = newMenuFState;
		}
		public void MenuBackwardInput(bool newMenuBState)
		{
			menuBState = newMenuBState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}