using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Invisibility : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public float lifetime = 3f;
    private string originalTag;

    void Start()
    {
        if (player != null)
        {
            originalTag = player.tag;
            player.tag = "PlayerInvisible"; 
            StartCoroutine(RevertInvisibility());
        }
    }

    private IEnumerator RevertInvisibility()
    {
        yield return new WaitForSeconds(lifetime);
        if (player != null) player.tag = originalTag; 
        Destroy(gameObject);
    }
}
