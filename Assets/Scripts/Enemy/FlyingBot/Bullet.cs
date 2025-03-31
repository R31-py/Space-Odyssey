using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 10f;
    public int damage = 1;
    public GameObject impactEffect;

    void Update()
    {
        // Move forward
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Only destroy if hitting player
        if (collision.CompareTag("Player"))
        {
            // Damage player
            PlayerValues player = collision.GetComponent<PlayerValues>();
            if (player != null)
            {
                player.health -= damage;
            }

            // Show hit effect
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        // Destroy when off-screen
        Destroy(gameObject);
    }
}