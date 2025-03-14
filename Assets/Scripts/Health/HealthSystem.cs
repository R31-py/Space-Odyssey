using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject h1;
    [SerializeField] public GameObject h2;
    [SerializeField] public GameObject h3;
    [SerializeField] public GameObject h4;
    [SerializeField] public GameObject h5;
    [SerializeField] public GameObject h6;
    [SerializeField] public GameObject h7;
    private GameObject[] healthPoints = new GameObject[7];
    [SerializeField] private PlayerValues playerValues;
    void Start()
    {
        healthPoints[0] = h1;
        healthPoints[1] = h2;
        healthPoints[2] = h3;
        healthPoints[3] = h4;
        healthPoints[4] = h5;
        healthPoints[5] = h6;
        healthPoints[6] = h7;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 7; i++)
        {
            if (i < playerValues.health)
            {
                healthPoints[i].SetActive(true);
            }   
            else
            {
                healthPoints[i].SetActive(false);
           }
        }
    }
}
