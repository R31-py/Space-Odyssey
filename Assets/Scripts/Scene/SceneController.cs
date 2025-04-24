using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public PlayerController player;
    public Vector3 startPosition;
    public float jumpHeight;
    public TextMeshProUGUI moneyText;
    private bool setUp = false;
    public GameObject boss;
    public DeathScreen deathScreen;
    public CameraController cameraController;

    void Start()
    {
        StartCoroutine(InitializePlayer());
    }

    private IEnumerator InitializePlayer()
    {
        yield return null;

        player = FindObjectOfType<PlayerController>();
        
        

        if (player == null)
        {
            Debug.LogError("PlayerController not found!");
            yield break;
        }

        PlayerValues playerValues = player.GetComponent<PlayerValues>();
        PlayerSaveManager saveManager = player.GetComponent<PlayerSaveManager>();
        cameraController.player = player.gameObject.transform;

        if (playerValues == null || saveManager == null)
        {
            Debug.LogError("Missing PlayerValues or PlayerSaveManager on Player!");
            yield break;
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        if (PlayerSaveManager.SaveLoadState.loadingFromSave)
        {
            float x = PlayerPrefs.GetFloat("PlayerX", startPosition.x);
            float y = PlayerPrefs.GetFloat("PlayerY", startPosition.y);
            float z = PlayerPrefs.GetFloat("PlayerZ", startPosition.z);
            player.transform.position = new Vector3(x, y, z);
            Debug.Log("Loaded saved position: " + player.transform.position);

            PlayerSaveManager.SaveLoadState.loadingFromSave = false;
        }
        else
        {
            player.transform.position = startPosition;
            Debug.Log("Using start position for scene.");
        }

        player.maxJumpHeight = jumpHeight;
        playerValues.moneyText = moneyText;
        saveManager.SavePlayerData(SceneManager.GetActiveScene().name);

        if (rb != null) rb.simulated = true;
        if(saveManager.IsBossDefeated("Robotut")) Destroy(boss);
        setUp = true;
    }
}
