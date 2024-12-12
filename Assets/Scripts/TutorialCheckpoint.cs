using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TutorialCheckpoint : MonoBehaviour
{
    
    public UnityEvent clearPoint;
    private bool canClear = true;
    private float cooldown = 0f;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && canClear)
            clearPoint.Invoke();
            canClear = false;
    }

    private void Update()
    {
        cooldown += Time.deltaTime;
        if (cooldown >= 1f)
        {
            canClear = true;
            cooldown = 0f;
        }
    }
}
