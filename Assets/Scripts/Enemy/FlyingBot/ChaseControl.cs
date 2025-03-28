using UnityEngine;

public class ChaseControl : MonoBehaviour
{
    public FlyingBot[] enemyArray;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (FlyingBot enemy in enemyArray)
            {
                enemy.SetCombatState(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (FlyingBot enemy in enemyArray)
            {
                enemy.SetCombatState(false);
            }
        }
    }
}