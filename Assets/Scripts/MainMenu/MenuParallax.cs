using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Tutorial: https://www.youtube.com/watch?v=B40xBPXK97A&t=65s
public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = 0.3f;
    
    private Vector2 startPos;
    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);//bekommt die Position von Mouse
        transform.position = Vector3.SmoothDamp(transform.position, startPos + (offset * offsetMultiplier), ref velocity, smoothTime);// wir bewegen wo der Mouse geht
    }
}
