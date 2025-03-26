using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBeep : MonoBehaviour
{
    public Light2D light2D;      // Reference to the Light2D component
    public float interval = 1f;    // Fixed interval time between toggles

    private void Start()
    {
        // If no light2D was set in the Inspector, try to get it from the same GameObject
        if (light2D == null)
            light2D = GetComponent<Light2D>();

        StartCoroutine(Beep());
    }

    private IEnumerator Beep()
    {
        while (true)
        {
            // Toggle the light on/off
            light2D.enabled = !light2D.enabled;
            // Wait for the fixed interval before toggling again
            yield return new WaitForSeconds(interval);
        }
    }
}
