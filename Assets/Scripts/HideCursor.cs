using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetCursorState(false, CursorLockMode.Locked);
    }
    
    // Ausblenden und Sperren des Cursors zu Beginn des Spiels
    public void SetCursorState(bool visible, CursorLockMode lockState)
    {
        Cursor.visible = visible;
        Cursor.lockState = lockState;
    }

    
}
