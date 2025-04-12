using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{

    public Transform player;
    [SerializeField] private float tolerance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;
    SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            SceneController sceneController = FindObjectOfType<SceneController>();
            if (sceneController != null)
            {
                player = sceneController.player.transform;
            }
            else
            {
                Debug.LogError("SceneController or Player not found!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x + lookAhead, player.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (tolerance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }
}
