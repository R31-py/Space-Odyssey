using UnityEngine;

public class FinalBossArrow : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the arrow
    [SerializeField] private float lifetime = 5f;
    private Vector2 direction;
    private bool directionSet = false;

    // Call this method to set the arrow's movement direction externally.
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        directionSet = true;
        // Rotate the arrow to face the new direction.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Start()
    {
        // If the direction hasn't been set externally, aim toward the player.
        if (!directionSet)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        // Destroy the arrow after a set lifetime.
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the arrow in the set direction.
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerValues playerValues = collision.GetComponent<PlayerValues>();
            if (playerValues != null)
            {
                playerValues.health -= 1;
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}