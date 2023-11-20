using System.Collections;
using UnityEngine;

public class ElectricBoxController : MonoBehaviour
{
    public float lidSmoothness = 5f;
    public float knobSmoothness = 5f;

    private bool isLidOpen = false;
    private bool isKnobTurned = false;

    private bool canToggleLid = true;
    private bool canToggleKnob = false;

    private GameObject lid;
    private GameObject knob;

    void Start()
    {
        lid = GameObject.Find("Cup"); // Replace with the actual name of your lid GameObject
        knob = GameObject.Find("Main Knob"); // Replace with the actual name of your knob GameObject

        // Ensure the initial rotation of the knob is X-90
        knob.transform.localRotation = Quaternion.Euler(-90, 0, 0);
    }

    void Update()
    {
        // Check if the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (canToggleLid)
            {
                canToggleLid = false; // Prevent starting a new lid toggle until the current one completes

                StartCoroutine(ToggleLid());
            }
        }
        // Check if the "T" key is pressed
        else if (Input.GetKeyDown(KeyCode.T))
        {
            if (canToggleKnob)
            {
                canToggleKnob = false; // Prevent starting a new knob toggle until the current one completes

                StartCoroutine(ToggleKnob());
            }
        }
    }

    IEnumerator ToggleLid()
    {
        // Toggle the lid state
        isLidOpen = !isLidOpen;

        // Decide the target rotation based on whether the lid is open or closed
        Quaternion lidTargetRotation = isLidOpen ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(-90, 90, 0);

        // Start the lid movement coroutine
        yield return StartCoroutine(MoveLid(lidTargetRotation));

        canToggleLid = true; // Allow starting a new lid toggle
        canToggleKnob = isLidOpen; // Allow starting a knob toggle only if the lid is open
    }

    IEnumerator ToggleKnob()
    {
        // Toggle the knob state
        isKnobTurned = !isKnobTurned;

        // Decide the target rotation based on whether the knob is turned or not
        Quaternion knobTargetRotation = isKnobTurned ? Quaternion.Euler(40, 0, 0) : Quaternion.Euler(-90, 0, 0);

        // Start the knob movement coroutine
        yield return StartCoroutine(MoveKnob(knobTargetRotation));

        canToggleKnob = true; // Allow starting a new knob toggle
    }

    IEnumerator MoveLid(Quaternion targetRotation)
    {
        Debug.Log("Moving lid...");

        Quaternion startRotation = lid.transform.localRotation;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            lid.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * lidSmoothness;
            yield return null;
        }

        lid.transform.localRotation = targetRotation;

        Debug.Log("Lid moved.");
    }

    IEnumerator MoveKnob(Quaternion targetRotation)
    {
        Debug.Log("Turning knob...");

        Quaternion startRotation = knob.transform.localRotation;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            knob.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * knobSmoothness;
            yield return null;
        }

        knob.transform.localRotation = targetRotation;

        Debug.Log("Knob turned.");
    }
}
