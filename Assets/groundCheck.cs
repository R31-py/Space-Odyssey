using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public String groundTag;
    public Boolean check;
    void Start()
    {
        groundTag = "Ground";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        check = collision.CompareTag(groundTag);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        check = !collision.CompareTag(groundTag);
    }
}
