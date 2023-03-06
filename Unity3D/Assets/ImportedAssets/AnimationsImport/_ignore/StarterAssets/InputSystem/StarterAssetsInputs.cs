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
			else Destroy(this);
		}
        private void Update()
        {
			StartCoroutine(GarbageCollectInputs());
			if (mouseL && mouseR) actionPressed = true;
			mouseL = false;
		}
		private IEnumerator GarbageCollectInputs()
        {
			WaitForSeconds wait = new WaitForSeconds(.2f);

			if (actionPressed && !mouseL)
				actionPressed = false;

			yield return wait;
        }
		public float scrollWaitTime = 0f;

		#region Input values
			[HideInInspector] public Vector2 move;
			[HideInInspector] public Vector2 look;
			[HideInInspector] public bool actionPressed = false;

			[HideInInspector] public bool jump = false;
			[HideInInspector] public bool crouch = false;
			[HideInInspector] public bool sprint = false;
			[HideInInspector] public bool mouseL = false;
			[HideInInspector] public bool mouseR = false;
			[HideInInspector] public bool pause = false;
			[HideInInspector] public bool menuFState = false;
			[HideInInspector] public bool menuBState = false;
			[HideInInspector] public bool tab = false;
			[HideInInspector] public bool interact = false;
			[HideInInspector] public bool inventory = false;
			[HideInInspector] public bool zoom = false;

		[HideInInspector] public int scrollVal = 0;
		#endregion Input values

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

		// **every value needs an onpress event**
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		#region OnPress Events

		public void OnMove(InputValue value) => move = value.Get<Vector2>();
		public void OnLook(InputValue value) => look = cursorInputForLook ? value.Get<Vector2>() : look;
		public void OnJump(InputValue value) => jump = (value.isPressed);
		public void OnTab(InputValue value) => tab = !tab;
		public void OnCrouch(InputValue value) => crouch = !crouch;
		public void OnSprint(InputValue value) => sprint = (value.isPressed);
		public void OnMouseL(InputValue value) => mouseL = (value.isPressed);
		public void OnMouseR(InputValue value) => mouseR = !mouseR;
		public void OnPause(InputValue value) => pause = !pause;
		public void OnMenuForward(InputValue value) => menuFState = (value.isPressed);
		public void OnMenuBackward(InputValue value) => menuBState = (value.isPressed);
		public void OnInteract(InputValue value) => interact = !interact;
		public void OnInventory(InputValue value) => this.inventory = (value.isPressed);
		public void OnZoom(InputValue value) => this.zoom = !this.zoom;

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
		public void SetLook(Vector2 look)
        {
			this.look = look;
        }
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