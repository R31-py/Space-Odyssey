using UnityEngine;

public class FakeDeathIntro : MonoBehaviour
{
    public Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            other.transform.position = spawnPoint.position;
            Debug.Log("Player teleported to: " + spawnPoint.position);
        }
    }
}