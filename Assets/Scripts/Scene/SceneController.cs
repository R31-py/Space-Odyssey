using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private PlayerController player;
    public Vector3 startPosition;
    public float jumpHeight;
    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.gameObject.transform.position = startPosition;
        player.jumpHeight = jumpHeight;
    }

    // Update is called once per frame
    void Update()
    {
    }

}