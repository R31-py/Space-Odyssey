using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Message : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool show = true;
    public GameObject canvas;
    public GameObject helper;
    public String[] messages =
    {
        "Hello hero! I am THE HELPER! \n  I am going to assist you and teach you the true universal warrior ways! \n Right Click to continue",
        "First you need to learn how to move! Use the right arrow key to get to the star!",
        "Great! Now use the left arrow key to get to the star!",
        "You sure learn fast! \n Use the space key to jump! If you press it twice you will perform a double jump",
        "You can also climb walls! Get the star and try to go to the other side of the wall. There you will find another star!",
        "Here you will see an health item! You will need it against your enemies!",
        "If you go near enemies they detect you. You can fight them off with your sword. Press z to attack!"
    };
    // Start is called before the first frame update
    void Start()
    {
        text.text = messages[0];
        show = true;
    }

    // Update is called once per frame
    void Update()
    {
        helper.SetActive(false);
            if (show)
            {
                Time.timeScale = 0;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (text.text == messages[0])
                    {
                        text.text = messages[1];
                    }else if (text.text == messages[1] || text.text == messages[2] || text.text == messages[3] || text.text == messages[4] || text.text == messages[6])
                    {
                        Time.timeScale = 1;
                        show = false;
                    }
                    else if(text.text == messages[5])
                    {
                        text.text = messages[6];
                    }
                }
            }else
            {
                Time.timeScale = 1;
            }
            canvas.SetActive(show);
    }
}
