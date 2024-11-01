using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Debug.Log("PauseMenu Awake");
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if pause menu already running unpause
           if (pauseMenu.activeInHierarchy)
            {
                pauseGame(false);
            }
            else
            {
                pauseGame(true);
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
}
