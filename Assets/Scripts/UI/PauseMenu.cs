using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private HideCursor hideCursor; 
    public Message message;

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (message != null && message.show)
        {
            // Prevent pausing when messages are active
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        bool isPaused = !pauseMenu.activeInHierarchy;
        pauseGame(isPaused);
    }

    public void pauseGame(bool status)
    {
        pauseMenu.SetActive(status);
        
        if (status)
        {
            timeManager.PauseGame();
            hideCursor.SetCursorState(true, CursorLockMode.None); // Show cursor
        }
        else
        {
            timeManager.ResumeGame();
            hideCursor.SetCursorState(false, CursorLockMode.Locked); // Hide cursor
        }
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}