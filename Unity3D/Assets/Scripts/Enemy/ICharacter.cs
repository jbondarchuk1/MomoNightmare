using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void Move(Vector3 move, bool crouch, bool jump);
}
