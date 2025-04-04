using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    
    public bool doReset = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void Update()
    {
        if (doReset)
        {
            reset();
            doReset = false;
        }
    }

    private void reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs reset manually!");

    }
}
