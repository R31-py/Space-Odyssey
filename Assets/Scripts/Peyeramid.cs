using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peyeramid : MonoBehaviour, IEnemy
{
    private float speed = 3f;
    public float height = 3f; 
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(0f, newY, 0f);
    }

    public void Trigger()
    {
        throw new System.NotImplementedException();
    }
}
