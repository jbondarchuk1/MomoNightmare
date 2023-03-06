using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThirdPersonAimController : AimBase
{
    public override bool AllowCameraRotation { get; protected set; } = true;

    /// <summary>
    /// Handles the right click to 3rd person shooter aiming function.
    /// When the user right clicks, they aim, another right click  will go back to the normal camera view.
    /// </summary>
    public override void Aim() { }

    public override void Enter() 
    {
    }

    public override void Exit() 
    {
    }
}
