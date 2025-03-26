using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public Light2D light2D;              // Reference to the Light2D component
    public float minFlickerInterval = 0.1f; // Minimum time between flickers
    public float maxFlickerInterval = 0.5f; // Maximum time between flickers

    private void Start()
    {
        // If no light2D was set in the Inspector, try to get it from the same GameObject
        if (light2D == null)
            light2D = GetComponent<Light2D>();

        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            // Toggle the light on/off
            light2D.enabled = !light2D.enabled;
            // Wait for a random interval before toggling again
            float waitTime = Random.Range(minFlickerInterval, maxFlickerInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
