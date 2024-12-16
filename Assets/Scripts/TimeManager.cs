using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private  int pauseCounter = 0;

    public void  PauseGame()
    {
        pauseCounter++;
        Time.timeScale = 0;
        Debug.Log(pauseCounter);

    }

    public void  ResumeGame()
    {
        pauseCounter = Mathf.Max(0, pauseCounter - 1);
        if (pauseCounter == 0)
        {
            Time.timeScale = 1;
        }
    }
}
