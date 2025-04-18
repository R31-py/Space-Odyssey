using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockController : Enemy
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float height = 1f;
    [SerializeField] public PlayerValues player;
    [SerializeField] private GameObject Pfb_WarlocksLaser;
    private bool playerDetected = false;
    
    private bool isFiring = false;
    public float fireRate = 2f;
    public float minFireRate = 0.3f;
    public float decreaseFireRate = 0.1f;
    
    private Vector3 startPos;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Move(0); 
        }

        if (playerDetected && !isFiring)
        {
            StartCoroutine(FireRate());
        }

        if (lifepoints <= 0)
        {
            canMove = false;
            Destroy(gameObject);
            MusicManager.Instance.PlayMusic("background");
        }
    }
    
    public override void Move(float direction)
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z); 
    }

    public override void Attack()
    {
        Debug.Log($"{gameObject.name} attacked!");
        Instantiate(Pfb_WarlocksLaser, transform.position + new Vector3(Random.Range(-5,5), 0, 0), Quaternion.identity);
    }
    
    IEnumerator FireRate()
    {
        isFiring = true; //verhindert mehrere coroutines
        while (playerDetected)
        {
            Attack();
            yield return new WaitForSeconds(fireRate);
            
            fireRate = Mathf.Max(fireRate - decreaseFireRate, minFireRate);
        }
        isFiring = false; //verhindert mehrere coroutines
    }
    
    public override void Trigger()
    {
        MusicManager.Instance.PlayMusic("boss_fight");
        playerDetected = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Trigger();
            Debug.Log($"{gameObject.name} detected the player.");
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.health -= 1;
        }
    }
}
