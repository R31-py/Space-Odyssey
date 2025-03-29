using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab; // Renamed from laserPrefab for clarity
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 3f;
    
    [Header("Destruction")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explosionSound;
    
    private AudioSource audioSource;
    private bool canShoot = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void SetShootingState(bool state)
    {
        canShoot = state;
        if (canShoot) StartCoroutine(ShootRoutine());
        else StopAllCoroutines();
    }

    private IEnumerator ShootRoutine()
    {
        while (canShoot)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // Instantiate bullet facing LEFT (180° rotation on Y-axis)
            GameObject bullet = Instantiate(
                bulletPrefab, 
                firePoint.position, 
                Quaternion.Euler(0f, 180f, 0f) // Rotate to face left
            );
        }
        else
        {
            Debug.LogWarning("BulletPrefab or FirePoint is not assigned!");
        }
    }

    // Rest of the code remains unchanged
    public void DestroyTurret()
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1.0f);
        }
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}