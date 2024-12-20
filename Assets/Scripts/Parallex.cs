using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallex : MonoBehaviour {

    private float length, startPos;
    public GameObject cam;
    public float parallexEffect;
    
    // Start is called before the first frame update
    void Start () {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update () {
        float temp = (cam.transform.position.x * (1-parallexEffect));
        float dist = (cam.transform.position.x*parallexEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if      (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }

}
