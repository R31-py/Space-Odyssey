using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject checkpoint;
    private Vector3[] positions;
    public int currentTutorial = 0;
    public PlayerValues player;
    void Start()
    {
        positions = new Vector3[5];
        positions[0] = new Vector3(-8, 1, 0);
        positions[1] = new Vector3(-16, 1, 0);
        positions[2] = new Vector3(-8, 5, 0);
        positions[3] = new Vector3(4.5f, 10, 0);
        positions[4] = new Vector3(8, 1, 0);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (currentTutorial >= 5)
            endTutorial();
        else
            checkpoint.transform.position = positions[currentTutorial];
    }

    public void ClearPoint()
    {
        currentTutorial += 1;
        if (currentTutorial == 2)
        {
            player.tutorialStage = 1;
        }

        if (currentTutorial == 4)
        {
            player.tutorialStage = 2;
        }
    }

    void endTutorial()
    {
        Debug.Log("Tutorial Ended");
    }
}
