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
    public ParticleSystem damageParticle;
    
    public int money = 100;
    
    public KeyCode RIGHT = KeyCode.RightArrow;
    public KeyCode LEFT = KeyCode.LeftArrow;
    public KeyCode DASH = KeyCode.E;
    public KeyCode JUMP = KeyCode.Space;
    public KeyCode FIGHT = KeyCode.Z;

    public AbilityItem[] Inventory = new AbilityItem[3];
    
    // Tutorial Variables
    [SerializeField] public int tutorialStage = 0;
    
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
            //damageParticle.Play();
//            SoundManager.Instance.PlaySound2D("player_Hurt");
            oldHealth = health;
        }
        else
        {
            // damageParticle.Stop();
        }
        if (health <= 0)
        {
            player.SetActive(false);    
        }
    }
}
