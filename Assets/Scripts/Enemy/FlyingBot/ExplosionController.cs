using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.5f;
    
    private void Start()
    {
        // Automatically destroy after animation
        Destroy(gameObject, destroyDelay);
    }

    // Called via animation event (if using animation events)
    public void OnExplosionFinished()
    {
        Destroy(gameObject);
    }
}