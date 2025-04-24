using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    private PlayerValues playerValues;
    public SceneController sceneController;
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerValues = sceneController.player.gameObject.GetComponent<PlayerValues>();
    }

    public void RestartGame()
    {
        StartCoroutine(playerValues.ReloadLastSave());
    }
    
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
