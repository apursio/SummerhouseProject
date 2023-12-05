using UnityEngine;
using UnityEngine.Rendering;

public class SmokeController : MonoBehaviour
{
    public float startDensity = 0.01f;
    public float endDensity = 0.2f;
    public float duration = 60f;

    private float startTime;
    private bool fogEnabled = false;

    void Start()
    {
        // Record the start time when the script starts
        startTime = Time.time;

        // Set the initial fog state
        RenderSettings.fog = false;
    }

    void Update()
    {
        // Calculate the elapsed time since the script started
        float elapsed = Time.time - startTime;

        // If fog is not enabled and the duration has not passed, enable fog
        if (!fogEnabled && elapsed < duration)
        {
            RenderSettings.fog = true;
            fogEnabled = true;
        }

        // Calculate the lerp value based on the elapsed time and duration
        float lerpValue = Mathf.Clamp01(elapsed / duration);

        // Lerp between startDensity and endDensity to get the current fog density
        float currentDensity = Mathf.Lerp(startDensity, endDensity, lerpValue);

        // Update the RenderSettings fog density
        RenderSettings.fogDensity = currentDensity;

        // Check if the duration has passed, and if so, stop the script
        if (elapsed >= duration)
        {
            enabled = false;
        }
    }
}
