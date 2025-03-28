using System.Collections;
using UnityEngine;

public class SpikesController : MonoBehaviour
{
    public float upPosition = 1f; // Adjust how high the spikes rise
    public float downPosition = 0f; // Adjust how low the spikes go
    public float riseSpeed = 0.3f; // Time to rise (faster)
    public float retractSpeed = 1f; // Time to retract (slower)
    public float stayUpTime = 0.5f; // Time to stay up

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isUp = false;

    void Start()
    {
        startPos = transform.position;
        targetPos = new Vector3(startPos.x, startPos.y + upPosition, startPos.z);
        StartCoroutine(SpikeCycle());
    }

    IEnumerator SpikeCycle()
    {
        while (true)
        {
            // Spikes rise quickly
            yield return MoveSpike(targetPos, riseSpeed);
            isUp = true;

            // Stay up for a moment
            yield return new WaitForSeconds(stayUpTime);

            // Spikes retract slowly
            yield return MoveSpike(startPos, retractSpeed);
            isUp = false;

            // Wait before repeating
            yield return new WaitForSeconds(2f - riseSpeed - stayUpTime - retractSpeed);
        }
    }

    IEnumerator MoveSpike(Vector3 target, float duration)
    {
        float elapsed = 0f;
        Vector3 initialPos = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(initialPos, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target; // Ensure exact position
    }
}