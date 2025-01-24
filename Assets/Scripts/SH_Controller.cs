using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Controller : MonoBehaviour, IEnemy
{
    [SerializeField] public float lifePoints = 2f;
    public float deathTimer = 3f;
    
    [SerializeField] private GameObject SHs_Dagger;
    [SerializeField] private float attackCooldown = 5f;
    private float attackTimer = 1f;

    [SerializeField]public Animator animator;
    
    private bool playerDetected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                Attack(); 
                attackTimer = attackCooldown;
            }
        }
    }

    public void Attack()
    {
        Debug.Log($"{gameObject.name} attacked!");
        animator.SetTrigger("attack");
        Instantiate(SHs_Dagger, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public void Trigger()
    {
        Debug.Log($"{gameObject.name} player detected!");
        playerDetected = true;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Trigger(); 
        }
    }
}
