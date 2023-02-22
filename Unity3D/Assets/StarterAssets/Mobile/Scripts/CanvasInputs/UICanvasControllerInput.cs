using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        //public void VirtualMoveInput(Vector2 virtualMoveDirection)
        //{
        //    starterAssetsInputs.MoveInput(virtualMoveDirection);
        //}

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.SetLook(virtualLookDirection);
        }

        //public void VirtualJumpInput(bool virtualJumpState)
        //{
        //    starterAssetsInputs.JumpInput(virtualJumpState);
        //}

        //public void VirtualSprintInput(bool virtualSprintState)
        //{
        //    starterAssetsInputs.SprintInput(virtualSprintState);
        //}

    }

}
