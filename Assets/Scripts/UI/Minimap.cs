using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Minimap : MonoBehaviour
{
    public Transform player;
    // LateUpdate is called after the player has moved
    //https://www.youtube.com/watch?v=28JTTXqMvOU
    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }
}
