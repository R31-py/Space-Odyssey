using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class wallCheck : MonoBehaviour
{
    public String wallTag;
    public Boolean check;
    void Start()
    {
        wallTag = "Wall";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        check = collision.CompareTag(wallTag);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        check = !collision.CompareTag(wallTag);
    }
    
}
