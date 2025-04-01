using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add the missing parentheses
            other.gameObject.GetComponent<PlayerValues>().health = 0;
        }
    }
}