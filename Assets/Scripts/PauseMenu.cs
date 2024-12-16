using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TimeManager timeManager;
    private void Awake()
    {
        Debug.Log("PauseMenu Awake");
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (FindObjectOfType<Message>()?.isMessageActive == true)
        {
            // Verhindere, dass Pause aktiviert wird, wenn Nachrichten aktiv sind
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if pause menu already running unpause
            if (pauseMenu.activeInHierarchy)
            {
                timeManager.PauseGame();
            }
            else
            {
                timeManager.PauseGame();
            }
            Debug.Log("Escape");
            
        }
    }
    
    public void pauseGame(bool status)
    {
        Debug.Log("Pause status: " + status);
        pauseMenu.SetActive(status);

        if (status)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}