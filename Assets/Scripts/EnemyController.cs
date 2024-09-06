using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float lifePoints = 4f;
    public Rigidbody2D body;
    private float spawnX;
    private int movingDirection = 1;
    private float directionChange = 0f;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spawnX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(body.velocity.x == 0){
            randomMoving();
        }
        if(lifePoints <= 0){
            Destroy(gameObject);
        }
        
    }

    void randomMoving(){
        directionChange += Time.deltaTime;
        if(directionChange > 1.5){
            directionChange = 0;
            movingDirection *= -1;
        }
        body.velocity = new Vector2(movingDirection, body.velocity.y);
    }
}
