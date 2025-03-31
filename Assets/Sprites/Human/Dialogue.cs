using UnityEngine;
using TMPro;

[RequireComponent(typeof(CircleCollider2D))]
public class Dialogue : MonoBehaviour
{
    [Header("Message Settings")]
    [SerializeField] string message = "Hello, Traveler!";
    [SerializeField] TextMeshProUGUI textDisplay; // Assign the TMP text child
    [SerializeField] GameObject messageParent; // Assign the parent GameObject (e.g., speech bubble)
    [SerializeField] float detectionRadius = 3f;

    private CircleCollider2D triggerZone;

    void Start()
    {
        triggerZone = GetComponent<CircleCollider2D>();
        triggerZone.radius = detectionRadius;
        triggerZone.isTrigger = true;

        // Hide both parent and text initially
        if (messageParent != null) messageParent.SetActive(false);
        if (textDisplay != null) textDisplay.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Activate parent first, then update text
            if (messageParent != null) messageParent.SetActive(true);
            if (textDisplay != null) 
            {
                textDisplay.text = message;
                textDisplay.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Deactivate parent (will hide child text automatically)
            if (messageParent != null) messageParent.SetActive(false);
        }
    }
}