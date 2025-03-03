using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
   [SerializeField] private float moveSpeed = 2f;   
    [SerializeField] private float moveHeight = 2f;  
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;  
    }

    void Update()
    {
        // Move the object up and down 
        float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
