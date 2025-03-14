using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHs_Dagger : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * (speed * Time.deltaTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerValues playerValues = collision.GetComponent<PlayerValues>();
            if (playerValues != null)
            {
                playerValues.health -= 1;
            }
            Destroy(gameObject); 
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); 
        }
    }
}
