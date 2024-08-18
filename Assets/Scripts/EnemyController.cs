using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float lifePoints = 4f;
    public float knockBack = 0f;
    [SerializeField] private Rigidbody2D body;
    private float spawnX;
    private int movingDirection = 1;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spawnX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(knockBack != 0){
            body.velocity = new Vector2(knockBack, body.velocity.y);
            knockBack = 0;
        }
        if(lifePoints <= 0){
            Destroy(gameObject);
        }
        randomMoving();
    }

    void randomMoving(){
        if(spawnX - transform.position.x > 3 || spawnX - transform.position.x < -3){
            movingDirection *= -1;
        }
        body.velocity = new Vector2(movingDirection, body.velocity.y);
    }
}
