using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public float tolerance;
    [SerializeField] private float helperSpeed;
    private float lookAhead;
    private float offsetUp = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if(Math.Abs(transform.position.x - player.position.x) < 2)
            offsetUp = Mathf.Abs(player.position.x - transform.position.x);
        transform.position = new Vector3(player.position.x + lookAhead, player.position.y + offsetUp, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (tolerance * player.localScale.x), Time.deltaTime * helperSpeed);
        
    }
}
