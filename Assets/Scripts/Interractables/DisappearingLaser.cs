using UnityEngine;

public class DisappearingLaser : MonoBehaviour
{
   
    public float onDuration = 2f;
    public float offDuration = 1f;
    public bool startOn = true;
    public GameObject laserObject;

    private float timer;
    private bool isOn;

    private void Awake()
    {
        // If no laser object assigned, use this object
        if (laserObject == null)
        {
            laserObject = this.gameObject;
        }
    }

    private void Start()
    {
        // Initialize the laser state
        isOn = startOn;
        laserObject.SetActive(isOn);
        timer = isOn ? onDuration : offDuration;
    }

    private void Update()
    {
        // Count down the timer
        timer -= Time.deltaTime;

        // When timer reaches zero, toggle the laser
        if (timer <= 0f)
        {
            ToggleLaser();
        }
    }

    private void ToggleLaser()
    {
        // Switch state
        isOn = !isOn;
        laserObject.SetActive(isOn);
        
        // Reset timer based on current state
        timer = isOn ? onDuration : offDuration;
    }

    // For debugging in the editor
    private void OnValidate()
    {
        // Clamp values to prevent negative numbers
        onDuration = Mathf.Max(0.1f, onDuration);
        offDuration = Mathf.Max(0.1f, offDuration);
    }
}