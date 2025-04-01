using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    [SerializeField] private Transform cam;  
    [SerializeField] private float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = cam.position.x * (1 - parallaxEffect);
        float dist = cam.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        // Smooth Looping Fix
        if (temp > startPos + length) 
            startPos += length;
        else if (temp < startPos - length) 
            startPos -= length;
    }
}
