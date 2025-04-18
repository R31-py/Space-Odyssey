using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Ability : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision is BoxCollider2D)
        {
            SoundManager.Instance.PlaySound2D("shield-down_ability");
            Destroy(gameObject); 
        }
    }
}
