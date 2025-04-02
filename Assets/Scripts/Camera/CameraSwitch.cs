using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera camLeft; // Assign Left Area's camera
    public CinemachineVirtualCamera camRight; // Assign Right Area's camera
    public bool startWithRightCamera = false; // Set this in the inspector for right side areas
    private CinemachineVirtualCamera _currentCam;
    
    // Store player entry position
    private Vector2 playerEntryPosition;

    void Start()
    {
        // Initialize with appropriate camera based on area
        if (startWithRightCamera)
        {
            _currentCam = camRight;
            camRight.enabled = true;
            camLeft.enabled = false;
        }
        else
        {
            _currentCam = camLeft;
            camLeft.enabled = true;
            camRight.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Store where the player entered the trigger
            playerEntryPosition = other.transform.position;
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
            
            // Determine if horizontal movement is the dominant direction
            bool horizontalMovement = Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y);
            
            // Switch cameras based on dominant horizontal movement
            if (horizontalMovement)
            {
                if (movementDirection.x > 0) // Moving right
                {
                    SwitchCamera(camRight);
                }
                else // Moving left
                {
                    SwitchCamera(camLeft);
                }
            }
        }
    }

    private void SwitchCamera(CinemachineVirtualCamera newCam)
    {
        _currentCam.enabled = false;
        newCam.enabled = true;
        _currentCam = newCam;
    }
}