using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		public static StarterAssetsInputs Instance { get; private set; }

		public bool isActionAndAiming()
        {
			return mouseR && mouseL;
        }
		public void ResetActionInput()
        {
			mouseL = false;
        }

        private void Awake()
        {
			if (Instance == null) Instance = this;
			else GameObject.Destroy(this);
		}
        private void Update()
        {
			StartCoroutine(GarbageCollectInputs());

			if (mouseL && mouseR)
			{
				actionPressed = true;
			}
			mouseL = false;

		}
		private IEnumerator GarbageCollectInputs()
        {
			WaitForSeconds wait = new WaitForSeconds(.2f);

			if (actionPressed && !mouseL)
				actionPressed = false;

			yield return wait;
        }

        #region Input values
        [HideInInspector] public bool actionPressed = false;
		[HideInInspector] public Vector2 move;
		[HideInInspector] public Vector2 look;
		[HideInInspector] public bool jump;
		[HideInInspector] public bool crouch = false;
		[HideInInspector] public bool sprint;
		[HideInInspector] public bool mouseL = false;
		[HideInInspector] public bool mouseR = false;
		[HideInInspector] public bool pause = false;
		[HideInInspector] public bool menuFState = false;
		[HideInInspector] public bool menuBState = false;
		[HideInInspector] public bool tab = false;
		[HideInInspector] public int scrollVal = 0;
		[HideInInspector] public float scrollWaitTime = 0f;
		[HideInInspector] public bool interact = false;
		#endregion Input values

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

		// **every value needs an onpress event and a toggle value**
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		#region OnPress Events

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
		public void OnTab(InputValue value)
		{
			TabInput();
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
		public void OnInteract(InputValue value)
        {
			InteractInput(value.isPressed);
        }



		public void OnScroll(InputValue value)
		{
			int getVal = (int)value.Get<float>();
			
			if (TimeMethods.GetWaitComplete(scrollWaitTime))
            {
				if (getVal > 0) scrollVal = 1;
				else scrollVal = -1;

				scrollWaitTime = (getVal != 0) ? TimeMethods.GetWaitEndTime(.2f): 0;
			}
        }
		public void ResetScroll()
        {
			scrollVal = 0;
        }
        #endregion OnPress Events
#else

#endif
        #region Toggle Value Methods
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
		public void TabInput()
		{
			tab = !tab;
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
		public void InteractInput(bool newInteract)
        {
			interact = !interact;
        }
        #endregion Toggle Value Methods


#if !UNITY_IOS || !UNITY_ANDROID
        #region Misc
        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
		#endregion Misc
#endif

	}

}