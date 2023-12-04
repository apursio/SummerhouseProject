using System.Collections;
using UnityEngine;

public class ElectricBoxController : MonoBehaviour
{
    // Lis�tty 26.11.
    public delegate void LightsToggleEventHandler(bool lightsOn);
    public static event LightsToggleEventHandler OnLightsToggle;
    // lis�tty 26.11.
    public float lidSmoothness = 5f;
    public float knobSmoothness = 5f;
    public float activationDistance = 2.0f;

    private bool isLidOpen = false;
    // private bool isKnobTurned = false;

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
        GlobalVariableStorage.isKnobTurned = false;
        lid = GameObject.Find("Cup");
        knob = GameObject.Find("Main Knob");

        knob.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        playerController = FindObjectOfType<MoveObjectController>();
    }

    void Update()
    {
        // Original raycast direction calculation
        Vector3 originalDirection = (playerController.transform.position - transform.position).normalized;

        // Rotate the original direction by 25 degrees to the left around the Y-axis
        float rotationAngle = 25f;
        Quaternion rotation = Quaternion.Euler(0, -rotationAngle, 0);
        Vector3 rotatedDirection = rotation * originalDirection;

        // Visualize the ray with the rotated direction
        Debug.DrawRay(transform.position, rotatedDirection * activationDistance, Color.green);

        // Use the interactableLayerMask in the Physics.Raycast
        if (Physics.Raycast(transform.position, rotatedDirection, out RaycastHit hit, activationDistance, interactableLayerMask))
        {
            //Debug.Log("Ray hit an object!");

            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("R key pressed");
                if (canToggleLid)
                {
                    canToggleLid = false;
                    StartCoroutine(ToggleLid());
                    FindObjectOfType<AudioManager>().Play("electricbox");
                }
            }
            else if (Input.GetKey(KeyCode.R))
            {
                Debug.Log("T key pressed");
                if (canToggleKnob)
                {
                    canToggleKnob = false;
                    StartCoroutine(ToggleKnob());
                    FindObjectOfType<AudioManager>().Play("electricboxtoggle");
                    //GlobalVariableStorage.actionScore = 600;
                    //GlobalVariableStorage.playerScore = GlobalVariableStorage.playerScore + GlobalVariableStorage.actionScore;
                    isDynamicLightsEnabled = !isDynamicLightsEnabled;
                    ToggleDynamicLights();
                    if (GlobalVariableStorage.scoreElectricityBox)
                    {
                        GlobalVariableStorage.actionScore = 600;
                        GlobalVariableStorage.playerScore = GlobalVariableStorage.playerScore + GlobalVariableStorage.actionScore;
                        GlobalVariableStorage.scoreElectricityBox = false;
                    }
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
        GlobalVariableStorage.isKnobTurned = !GlobalVariableStorage.isKnobTurned;
        Quaternion knobTargetRotation = GlobalVariableStorage.isKnobTurned ? Quaternion.Euler(40, 0, 0) : Quaternion.Euler(-90, 0, 0);
        yield return StartCoroutine(MoveKnob(knobTargetRotation));
        canToggleKnob = true;

        // Lis�tty 26.11.
        // Notify subscribers (other scripts) that lights were toggled
        OnLightsToggle?.Invoke(!GlobalVariableStorage.isKnobTurned);
        // Lis�tty 26.11.
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