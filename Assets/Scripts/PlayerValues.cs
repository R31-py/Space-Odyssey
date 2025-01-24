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
    public ParticleSystem damageParticle;
    
    public KeyCode RIGHT = KeyCode.RightArrow;
    public KeyCode LEFT = KeyCode.LeftArrow;
    public KeyCode DASH = KeyCode.E;
    public KeyCode JUMP = KeyCode.Space;
    public KeyCode FIGHT = KeyCode.Z;
    
    // Tutorial Variables
    [SerializeField] public int tutorialStage = 0;
    
    void Start()
    {
        oldHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(JUMP))
        {
            SoundManager.Instance.PlaySound2D("player_Jump");
        }
        
        if (Input.GetKeyDown(FIGHT))
        {
            SoundManager.Instance.PlaySound2D("player_Slash");
        }
        
        if (oldHealth > health)
        {
            Debug.Log("Play damageParticle");
            damageParticle.Play();
            SoundManager.Instance.PlaySound2D("player_Hurt");
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
