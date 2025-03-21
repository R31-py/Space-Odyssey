    using UnityEngine;
    using Cinemachine;

    public class CameraSwitchVertical : MonoBehaviour
    {
        public CinemachineVirtualCamera cam1; // Assign Area A's camera
        public CinemachineVirtualCamera cam2; // Assign Area B's camera
        private CinemachineVirtualCamera _currentCam;

        void Start()
        {
            // Initialize with Cam1 (Area A's camera)
            _currentCam = cam1;
            cam1.enabled = true;
            cam2.enabled = false;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Calculate exit direction relative to the trigger
                Vector3 exitDirection = other.transform.position - transform.position;

                // Switch cameras based on vertical exit direction
                if (exitDirection.y > 0) // Moving up (A → B)
                {
                    SwitchCamera(cam2);
                }
                else if (exitDirection.y < 0) // Moving down (B → A)
                {
                    SwitchCamera(cam1);
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