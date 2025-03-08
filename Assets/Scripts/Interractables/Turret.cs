using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab; // Laser projectile
    [SerializeField] private Transform firePoint; // Where the laser spawns
    [SerializeField] private float fireRate = 3f; // Firing interval

    [SerializeField] private GameObject explosionPrefab; // Explosion effect
    [SerializeField] private AudioClip explosionSound; // Explosion sound effect
    private AudioSource audioSource;

    private void Start()
    {
        // Start shooting continuously
        StartCoroutine(ShootRoutine());

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Shoot()
    {
        if (laserPrefab != null && firePoint != null)
        {
            Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("LaserPrefab or FirePoint is not assigned!");
        }
    }

    public void DestroyTurret()
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Play explosion sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1.0f);
        }
        else
        {
            Debug.LogWarning("ExplosionSound is not assigned!");
        }

        // Instantiate explosion effect
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("ExplosionPrefab is not assigned!");
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }
}