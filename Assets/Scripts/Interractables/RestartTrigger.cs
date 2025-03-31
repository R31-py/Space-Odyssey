using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartTrigger : MonoBehaviour
{
    [SerializeField] private Transform restartPosition;  // Assign in Inspector
    [SerializeField] private float restartDelay = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Move player to restart position
            other.transform.position = restartPosition.position;

            // Restart scene after delay
            Invoke(nameof(RestartScene), restartDelay);
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}