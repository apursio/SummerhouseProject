using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireController : MonoBehaviour
{
    public float extinguishDistance = 0.5f;  // Adjust this distance based on your needs
    [SerializeField] private TextMeshProUGUI extinquishFireText;

    public GameObject fire1;
    public GameObject fire2;
    //GameObject fire3 = GlobalFireStorage.Instance.Fire3;
    public GameObject fire4;

    private void Awake()
    {
        extinquishFireText.enabled = false;
    }

    private void Update()
    {
        IsObjectGrabbableNearFire();
        // Check for left mouse click
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GlobalVariableStorage.level1)
            {
                // Check if any ObjectGrabbable is close enough to extinguish the fire
                if (IsObjectGrabbableNearFire())
                {
                    // Disable the fire
                    if (GlobalVariableStorage.fireIsOutOfControl == false)
                    {
                        fire1.SetActive(false);
                        GlobalVariableStorage.fireIsOut = true;
                        GlobalVariableStorage.actionScore = GlobalVariableStorage.availableScore;
                        GlobalVariableStorage.playerScore += GlobalVariableStorage.actionScore;
                        Debug.Log("Fire is out from firecontroller");
                    }
                    else
                    {
                        GlobalVariableStorage.level3 = true;
                        //fire4.SetActive(true);
                        Debug.Log("Fire is out of Control");
                    }
                    extinquishFireText.enabled = false;
                }
            }
            if (GlobalVariableStorage.level2)
            {
                if (GlobalVariableStorage.isKnobTurned)
                {
                    Debug.Log("Knob turned");
                    // Check if any ObjectGrabbable is close enough to extinguish the fire
                    if (IsObjectGrabbableNearFire())
                    {
                        // Disable the fire
                        if (GlobalVariableStorage.fireIsOutOfControl == false)
                        {
                            fire2.SetActive(false);
                            GlobalVariableStorage.fireIsOut = true;
                            GlobalVariableStorage.actionScore = GlobalVariableStorage.availableScore;
                            GlobalVariableStorage.playerScore += GlobalVariableStorage.actionScore;
                            Debug.Log("Fire is out from firecontroller");
                        }
                        else
                        {
                            GlobalVariableStorage.level3 = true;
                            Debug.Log("Fire is out of Control");
                        }
                        extinquishFireText.enabled = false;
                    }
                }
            }
        }
    }

    private bool IsObjectGrabbableNearFire()
    {
        Debug.Log("Is Object Crabbable Near Fire");
        Collider fireCollider = GetComponent<Collider>();

        if (fireCollider == null)
        {

            return false;
        }

        // Find all objects with the ObjectGrabbable script
        ObjectGrabbable[] grabbableObjects = FindObjectsOfType<ObjectGrabbable>();
        //Debug.Log("level 1: " + GlobalVariableStorage.level1);

        // Check if any of the grabbable objects are close enough to the fire
        foreach (ObjectGrabbable grabbableObject in grabbableObjects)
        {
            Debug.Log("in forEach");
            Collider objectGrabbableCollider = grabbableObject.GetComponent<Collider>();
            if (GlobalVariableStorage.level1)
            {
                Debug.Log("in Level1");
                if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("lid") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)     
                {
                    //Debug.Log("If level 1 and crabbables");
                    GlobalVariableStorage.availableScore = 500;
                    extinquishFireText.enabled = true;
                    return true; // At least one object is close enough
                }
                else if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("blanket") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    //Debug.Log("If level 1 and crabbables");
                    GlobalVariableStorage.availableScore = 500;
                    extinquishFireText.enabled = true;
                    return true; // At least one object is close enough
                }
                else if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("extinguisher") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    //Debug.Log("If level 1 and crabbables");
                    GlobalVariableStorage.availableScore = 100;
                    extinquishFireText.enabled = true;
                    return true; // At least one object is close enough
                }
                else if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("water") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    //Debug.Log("If level 1 and water");
                    GlobalVariableStorage.availableScore = 0;
                    GlobalVariableStorage.fireIsOutOfControl = true;
                    //Debug.Log("Fire out of control");
                    extinquishFireText.enabled = true;
                    return true; // At least one object is close enough
                }
            }
            else if (GlobalVariableStorage.level2)
            {
                if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("blanket") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    Debug.Log("If level 2 and crabbables");
                    GlobalVariableStorage.availableScore = 500;
                    extinquishFireText.enabled = true;
                    return true; // At least one object is close enough
                }
                else if (objectGrabbableCollider != null &&
                        grabbableObject.CompareTag("extinguisher") &&
                        Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    Debug.Log("level 2 and crabbables");
                    GlobalVariableStorage.availableScore = 500;
                    extinquishFireText.enabled = true;
                    return true; // At least one object is close enough
                }
                else if (objectGrabbableCollider != null &&
                        grabbableObject.CompareTag("water") &&
                        Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    Debug.Log("level 2 and crabbables");
                    GlobalVariableStorage.availableScore = 500;
                    extinquishFireText.enabled = true;
                    return true; // At least one object is close enough
                }
            }
            else if (GlobalVariableStorage.level3)
            {
                if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("level3") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    Debug.Log("If level 3 and crabbables");
                    extinquishFireText.enabled = true;
                    return true; // At least one object is close enough
                }
            }

        }
        //Debug.Log("Object crabbable false");
        extinquishFireText.enabled = false;
        return false; // No object is close enough
    }
}