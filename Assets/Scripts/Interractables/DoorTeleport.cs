using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    public Transform teleportTarget; 
    private bool playerIsNear = false;
    public GameObject openGamePanel;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            playerIsNear = true;    
            openGamePanel.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            openGamePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.T)) // Press 'T' near the door
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player
            if (player != null && teleportTarget != null)
            {
                player.transform.position = teleportTarget.position; // Teleport the player
            }
        }
    }
}
