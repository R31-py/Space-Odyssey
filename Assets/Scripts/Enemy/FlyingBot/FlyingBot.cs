using UnityEngine;

public class FlyingBot : MonoBehaviour
{
    [Header("Movement Settings")]
    public float idleSpeed;
    public Transform startingPoint;
    
    [Header("Hover Settings")]
    public float hoverRadius = 3f;
    public float directionChangeInterval = 2f;
    public float maxDirectionAngle = 45f;
    
    [Header("Combat Settings")]
    public bool isShooting = false;
    
    public float FacingDirection => Mathf.Sign(transform.localScale.x);
    
    private GameObject player;
    private Rigidbody2D body;
    private Vector2 randomDirection;
    private float lastDirectionChangeTime;
    private Vector2 startPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        body = GetComponent<Rigidbody2D>();
        startPosition = startingPoint.position;
        PickNewRandomDirection();
    }

    private void FixedUpdate()
    {
        RandomHover();
        
        if (isShooting)
        {
            FacePlayer();
        }
    }

    public void SetCombatState(bool shouldShoot)
    {
        isShooting = shouldShoot;
    }

    private void FacePlayer()
    {
        if (player == null) return;
        float xScale = Mathf.Sign(player.transform.position.x - transform.position.x);
        transform.localScale = new Vector3(xScale, 1, 1);
    }

    private void RandomHover()
    {
        if (Time.time - lastDirectionChangeTime > directionChangeInterval)
        {
            PickNewRandomDirection();
            lastDirectionChangeTime = Time.time;
        }

        float distanceFromStart = Vector2.Distance(transform.position, startPosition);
        if (distanceFromStart > hoverRadius)
        {
            Vector2 returnDirection = (startPosition - (Vector2)transform.position).normalized;
            randomDirection = Vector2.Lerp(randomDirection, returnDirection, 0.1f).normalized;
        }

        body.velocity = randomDirection * idleSpeed;
    }

    private void PickNewRandomDirection()
    {
        Vector2 toCenter = (startPosition - (Vector2)transform.position).normalized;
        float randomAngle = Random.Range(-maxDirectionAngle, maxDirectionAngle);
        randomDirection = Quaternion.Euler(0, 0, randomAngle) * toCenter;
        randomDirection = randomDirection.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(startingPoint.position, hoverRadius);
    }
}