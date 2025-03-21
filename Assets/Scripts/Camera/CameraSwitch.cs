using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera camLeft; // Assign Left Area's camera
    public CinemachineVirtualCamera camRight; // Assign Right Area's camera
    private CinemachineVirtualCamera _currentCam;
    
    // Store player entry position
    private Vector2 playerEntryPosition;

    void Start()
    {
        // Initialize with camLeft
        _currentCam = camLeft;
        camLeft.enabled = true;
        camRight.enabled = false;
        Debug.Log("Camera system initialized with left camera active");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Store where the player entered the trigger
            playerEntryPosition = other.transform.position;
            Debug.Log($"Player entered trigger at position: X={playerEntryPosition.x:F2}, Y={playerEntryPosition.y:F2}");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Get player exit position
            Vector2 playerExitPosition = other.transform.position;
            
            // Calculate true movement direction based on entry and exit positions
            Vector2 movementDirection = playerExitPosition - playerEntryPosition;
            
            // Debug logs
            Debug.Log($"Player entered at: X={playerEntryPosition.x:F2}, Y={playerEntryPosition.y:F2}");
            Debug.Log($"Player exited at: X={playerExitPosition.x:F2}, Y={playerExitPosition.y:F2}");
            Debug.Log($"Movement vector: X={movementDirection.x:F2}, Y={movementDirection.y:F2}");
            
            // Determine if horizontal movement is the dominant direction
            bool horizontalMovement = Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y);
            
            // Switch cameras based on dominant horizontal movement
            if (horizontalMovement)
            {
                if (movementDirection.x > 0) // Moving right
                {
                    Debug.Log("Player moving RIGHT - Switching to right camera");
                    SwitchCamera(camRight);
                }
                else // Moving left
                {
                    Debug.Log("Player moving LEFT - Switching to left camera");
                    SwitchCamera(camLeft);
                }
            }
            else
            {
                Debug.Log("Vertical movement detected, keeping current camera");
            }
        }
    }

    private void SwitchCamera(CinemachineVirtualCamera newCam)
    {
        Debug.Log($"Switching from {_currentCam.name} to {newCam.name}");
        _currentCam.enabled = false;
        newCam.enabled = true;
        _currentCam = newCam;
        Debug.Log("Camera switch complete");
    }
}