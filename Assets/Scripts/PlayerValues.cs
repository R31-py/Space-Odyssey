using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    [SerializeField] public int health = 3;
    private int oldHealth = 3;
    [SerializeField] public int maxHealth;
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject message;
    /*[SerializeField] private GameObject damageParticles;
     Provo me e ba si prefab e jo particle system*/
    
    public ParticleSystem damageParticle;
    void Start()
    {
        oldHealth = health;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oldHealth > health)
        {
            Debug.Log("Play damageParticle");
            damageParticle.Play();
            oldHealth = health;
        }
        else
        {
            damageParticle.Stop();
        }
        if (health <= 0)
        {
            message.SetActive(true);
            player.SetActive(false);
        }
    }
}
