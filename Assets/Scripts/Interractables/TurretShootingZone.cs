using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShootingZone : MonoBehaviour
{
    public Turret[] turrets; // Assign turrets in the Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (Turret turret in turrets)
            {
                turret.SetShootingState(true); // Enable shooting
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (Turret turret in turrets)
            {
                turret.SetShootingState(false); // Disable shooting
            }
        }
    }
}

