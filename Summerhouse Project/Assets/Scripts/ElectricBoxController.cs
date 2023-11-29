using System.Collections;
using UnityEngine;

public class ElectricBoxController : MonoBehaviour
{
    // Lisätty 26.11.
    public delegate void LightsToggleEventHandler(bool lightsOn);
    public static event LightsToggleEventHandler OnLightsToggle;
    // lisätty 26.11.
    public float lidSmoothness = 5f;
    public float knobSmoothness = 5f;
    public float activationDistance = 2.0f;

    private bool isLidOpen = false;
    private bool isKnobTurned = false;

    private bool canToggleLid = true;
    private bool canToggleKnob = false;

    private GameObject lid;
    private GameObject knob;

    public GameObject dynamicLights;
    private bool isDynamicLightsEnabled = true;

    private MoveObjectController playerController;

    // Declare a LayerMask variable
    public LayerMask interactableLayerMask;

    void Start()
    {
        lid = GameObject.Find("Cup");
        knob = GameObject.Find("Main Knob");

        knob.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        playerController = FindObjectOfType<MoveObjectController>();
    }

    void Update()
    {
        //Debug.Log("Update method called");

        Debug.DrawRay(transform.position, playerController.transform.position - transform.position, Color.green); // Visualize the ray
        // Use the interactableLayerMask in the Physics.Raycast
        if (Physics.Raycast(transform.position, playerController.transform.position - transform.position, out RaycastHit hit, activationDistance, interactableLayerMask))
        {
            //Debug.Log("Player is within activation distance");

            if (Input.GetKey(KeyCode.R))
            {
                Debug.Log("R key pressed");
                if (canToggleLid)
                {
                    canToggleLid = false;
                    StartCoroutine(ToggleLid());
                }
            }
            else if (Input.GetKey(KeyCode.T))
            {
                Debug.Log("T key pressed");
                if (canToggleKnob)
                {
                    canToggleKnob = false;
                    StartCoroutine(ToggleKnob());
                    GlobalVariableStorage.actionScore = 600;
                    GlobalVariableStorage.playerScore = GlobalVariableStorage.playerScore + GlobalVariableStorage.actionScore;
                    isDynamicLightsEnabled = !isDynamicLightsEnabled;
                    ToggleDynamicLights();

                }
            }
        }
    }

    void ToggleDynamicLights()
    {
        if (dynamicLights != null)
        {
            dynamicLights.SetActive(isDynamicLightsEnabled);
            Debug.Log("DynamicLights are now " + (isDynamicLightsEnabled ? "enabled" : "disabled"));
        }
        else
        {
            Debug.LogWarning("DynamicLights reference is not set!");
        }
    }

    IEnumerator ToggleLid()
    {
        isLidOpen = !isLidOpen;
        Quaternion lidTargetRotation = isLidOpen ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(-90, 90, 0);
        yield return StartCoroutine(MoveLid(lidTargetRotation));
        canToggleLid = true;
        canToggleKnob = isLidOpen;
    }

    IEnumerator ToggleKnob()
    {
        isKnobTurned = !isKnobTurned;
        Quaternion knobTargetRotation = isKnobTurned ? Quaternion.Euler(40, 0, 0) : Quaternion.Euler(-90, 0, 0);
        yield return StartCoroutine(MoveKnob(knobTargetRotation));
        canToggleKnob = true;

        // Lisätty 26.11.
        // Notify subscribers (other scripts) that lights were toggled
        OnLightsToggle?.Invoke(!isKnobTurned);
        // Lisätty 26.11.
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