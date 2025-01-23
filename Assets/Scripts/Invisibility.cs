using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Invisibility : MonoBehaviour
{
    public GameObject invisibilityIndicator;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.tag = "PlayerInvisible";
        invisibilityIndicator.SetActive(true);
    }
}
