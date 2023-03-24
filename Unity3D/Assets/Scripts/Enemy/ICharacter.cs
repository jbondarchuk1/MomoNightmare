using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    bool IsGrounded { get; set; }
    void Move(Vector3 move, bool crouch, bool jump);
}
