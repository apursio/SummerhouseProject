using System.Collections;
using UnityEngine;
using TMPro;

public class ElectricBoxController : MonoBehaviour
{
    public delegate void LightsToggleEventHandler(bool lightsOn);
    public static event LightsToggleEventHandler OnLightsToggle;
    public float lidSmoothness = 5f;
    public float knobSmoothness = 5f;
    public float activationDistance = 1.0f;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private TextMeshProUGUI OpenCloseText;
    [SerializeField] private TextMeshProUGUI UseText;
    private string openText = "Press [E] to open";
    private string closeText = "Press [E] to close";

    private bool isLidOpen = false;

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
        OpenCloseText.gameObject.SetActive(false);
        GlobalVariableStorage.isKnobTurned = false;
        lid = GameObject.Find("Cup");
        knob = GameObject.Find("Main Knob");

        knob.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        playerController = FindObjectOfType<MoveObjectController>();
    }

    void Update()
    {
        Vector3 directionToBox = (transform.position - playerController.transform.position).normalized;
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * activationDistance, Color.green);

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, activationDistance, interactableLayerMask))
        {
            if (raycastHit.collider.CompareTag("level2"))
            {
                if (!isLidOpen)
                {
                    OpenCloseText.text = openText;
                    OpenCloseText.gameObject.SetActive(true);
                    UseText.enabled = false;
                } else {
                    OpenCloseText.text = closeText;
                    OpenCloseText.gameObject.SetActive(true);
                    UseText.enabled = true;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    Debug.Log("E key pressed");
                    if (canToggleLid)
                    {
                        canToggleLid = false;
                        StartCoroutine(ToggleLid());
                        FindObjectOfType<AudioManager>().Play("electricbox");
                    }
                }
                else if (Input.GetKey(KeyCode.R))
                {
                    Debug.Log("R key pressed");
                    if (canToggleKnob)
                    {
                        canToggleKnob = false;
                        StartCoroutine(ToggleKnob());
                        FindObjectOfType<AudioManager>().Play("electricboxtoggle");
                        isDynamicLightsEnabled = !isDynamicLightsEnabled;
                        ToggleDynamicLights();
                    }
                }
            }
            else
            {
                OpenCloseText.gameObject.SetActive(false);
                UseText.enabled = false;
            }
        } 
        else
        {
            OpenCloseText.gameObject.SetActive(false);
            UseText.enabled = false;
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

 
        // Notify other scripts that lights were toggled
        OnLightsToggle?.Invoke(!GlobalVariableStorage.isKnobTurned);
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