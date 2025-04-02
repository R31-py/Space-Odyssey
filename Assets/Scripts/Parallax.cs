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
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
            length = renderer.bounds.size.x;
        else
            Debug.LogError("No SpriteRenderer found in this object or its children");
    }

    void Update()
    {
        float dist = cam.position.x * parallaxEffect;
        float newPos = startPos + dist;
    
        transform.position = new Vector3(newPos, transform.position.y, transform.position.z);

        // Better looping check
        if (cam.position.x - transform.position.x > length)
            startPos += length;
        else if (cam.position.x - transform.position.x < -length)
            startPos -= length;
    }
}
