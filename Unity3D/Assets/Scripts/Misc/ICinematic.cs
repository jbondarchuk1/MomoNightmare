using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public interface ICinematic
{
    CinemachineVirtualCamera cinemachineVirtualCamera { get; set; }

    void EnterCinematic();
    void ExitCinematic();
}
