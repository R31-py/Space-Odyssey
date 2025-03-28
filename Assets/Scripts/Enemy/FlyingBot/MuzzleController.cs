using UnityEngine;

public class MuzzleController : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Transform player;
    public FlyingBot flyingBot;

    [Header("Settings")]
    public float fireRate = 0.5f;
    public float bulletSpeed = 10f;
    public float rotationSpeed = 5f;

    private float nextFireTime;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (flyingBot == null)
            flyingBot = GetComponentInParent<FlyingBot>();
    }

    void Update()
    {
        if (player == null || !flyingBot.isShooting) return;

        FacePlayer();
        HandleShooting();
    }

    void FacePlayer()
    {
        Vector2 direction = player.position - transform.position;
        direction.x *= flyingBot.FacingDirection; // Account for flip
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
    }

    void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 shootDirection = firePoint.right * flyingBot.FacingDirection;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        if (bullet.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = shootDirection * bulletSpeed;
        }
    }
}