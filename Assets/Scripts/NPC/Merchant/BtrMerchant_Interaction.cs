using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtrMerchant_Interaction : MonoBehaviour
{
    [SerializeField] private GameObject Shop;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            Shop.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Shop.SetActive(false);
        }
    }
}
