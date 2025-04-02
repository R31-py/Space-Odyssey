using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public Animator buttonAnimator;
    public GameObject objectToDestroy;
    private bool playerInRange = false;
    public GameObject openGamePanel;

    private void Start()
    {
        // Set initial animation state
        if (buttonAnimator != null)
        {
            buttonAnimator.Play("redButton");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PlaySwitchAnimation();
            DestroyTargetObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            openGamePanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            openGamePanel.SetActive(false);
        }
    }

    void PlaySwitchAnimation()
    {
        if (buttonAnimator != null)
        {
            buttonAnimator.Play("switch");
        }
    }

    void DestroyTargetObject()
    {
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
    }
}