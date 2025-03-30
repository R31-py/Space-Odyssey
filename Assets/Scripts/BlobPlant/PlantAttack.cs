using UnityEngine;

public class PlantAttack : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 3f;
    
    private Vector3 targetDirection;

    public void SetTarget(Vector3 targetPosition)
    {
        targetDirection = (targetPosition - transform.position).normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += targetDirection * (speed * Time.deltaTime);
        Debug.Log($"Projectile moving in direction: {targetDirection}");
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
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}