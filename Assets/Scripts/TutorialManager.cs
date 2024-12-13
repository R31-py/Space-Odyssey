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
    public Message message;
    public GameObject messageObject;
    public int currentMessage = 0;

    void Start()
    {
        positions = new Vector3[5];
        positions[0] = new Vector3(-8, 1, 0);
        positions[1] = new Vector3(-16, 1, 0);
        positions[2] = new Vector3(-8, 5, 0);
        positions[3] = new Vector3(8, 1 , 0);
        positions[4] = new Vector3(8, 1, 0);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (currentTutorial >= 4)
            endTutorial();
        else
            checkpoint.transform.position = positions[currentTutorial];
    }

    public void ClearPoint()
    {
        currentTutorial += 1;
        message.show = true;
       
        switch (currentTutorial)
        {
            case 0:
                message.text.text = message.messages[0];
                break;
            case 1:
                message.text.text = message.messages[2];
                break;
            case 2:
                message.text.text = message.messages[3];
                break;
            case 3:
                message.text.text = message.messages[4];
                break;
            case 4:
                message.text.text = message.messages[5];
                break;
        }
        if (currentTutorial == 2)
        {
            player.tutorialStage = 1;
        }

        if (currentTutorial == 4)
        {
            player.tutorialStage = 2;
        }
    }

    public void nextMessage()
    {
        currentMessage += 1;
    }

    void endTutorial()
    {
        Destroy(checkpoint.gameObject);
        Debug.Log("Tutorial Ended");
    }
}
