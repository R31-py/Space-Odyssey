using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] private int maxHealth;
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject message;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            message.SetActive(true);
            player.SetActive(false);
        }
    }
}
