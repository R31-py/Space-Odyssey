using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [Tooltip("Scene Name to load when transitioning.")]
    public string sceneToLoad;

    [Tooltip("Check this if the transition requires a key press (e.g., door or spaceship).")]
    public bool requireKeyPress = false;

    private bool playerInRange = false;

    void Update()
    {
        // For transitions that require the player to press T
        if (requireKeyPress && playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TransitionToNextLevel();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player.
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

            // If no key press is required, transition immediately.
            if (!requireKeyPress)
            {
                TransitionToNextLevel();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reset the flag when the player leaves the trigger area.
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void TransitionToNextLevel()
    {
        // Optionally, you can add any transition effects here (fade out, sound, etc.)
        SceneManager.LoadScene(sceneToLoad);
    }
}