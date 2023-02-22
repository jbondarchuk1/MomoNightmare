using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIDialog
{
    void SetCanOpen(bool canOpen);
    bool CanOpen();
    bool IsOpen();
    void Open();
    void Close();
    void Toggle();
}
