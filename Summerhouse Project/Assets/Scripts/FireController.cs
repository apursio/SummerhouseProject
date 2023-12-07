using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireController : MonoBehaviour
{
    public float extinguishDistance = 0.5f;
    [SerializeField] private TextMeshProUGUI extinquishFireText;

    public GameObject fire1;
    public GameObject fire2;
    private bool waterUsed = false;

    private void Awake()
    {
        extinquishFireText.enabled = false;
    }

    private void Update()
    {
        IsObjectGrabbableNearFire();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GlobalVariableStorage.level1)
            {
                // Check if any ObjectGrabbable is close enough to extinguish the fire
                if (IsObjectGrabbableNearFire())
                {
                    // Disable the fire
                    if (waterUsed == false)
                    {
                        fire1.SetActive(false);
                        GlobalVariableStorage.fireIsOut = true;
                        GlobalVariableStorage.actionScore = GlobalVariableStorage.availableScore;
                        GlobalVariableStorage.playerScore += GlobalVariableStorage.actionScore;
                        Debug.Log("Fire is out from firecontroller");
                    }
                    else
                    {
                        GlobalVariableStorage.fireIsOutOfControl = true;
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
        Collider fireCollider = GetComponent<Collider>();

        if (fireCollider == null)
        {
            return false;
        }

        // Find all objects with the ObjectGrabbable script
        ObjectGrabbable[] grabbableObjects = FindObjectsOfType<ObjectGrabbable>();
  

        // Check if any of the grabbable objects are close enough to the fire
        foreach (ObjectGrabbable grabbableObject in grabbableObjects)
        {
          
            Collider objectGrabbableCollider = grabbableObject.GetComponent<Collider>();
            if (GlobalVariableStorage.level1)
            {
            
                if (objectGrabbableCollider != null &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    if (grabbableObject.CompareTag("lid") ||
                        grabbableObject.CompareTag("blanket"))
                    {
                        GlobalVariableStorage.availableScore = 500;
                        extinquishFireText.enabled = true;
                        return true; // At least one object is close enough
                    }
                    else if (grabbableObject.CompareTag("extinguisher"))
                    {
                        GlobalVariableStorage.availableScore = 100;
                        extinquishFireText.enabled = true;
                        return true; // At least one object is close enough
                    }

                    else if (grabbableObject.CompareTag("water"))
                    {
                        GlobalVariableStorage.availableScore = 0;
                        extinquishFireText.enabled = true;
                        waterUsed = true;
                        return true; // At least one object is close enough
                    }
                }
            }
            else if (GlobalVariableStorage.level2)
            {
                if (objectGrabbableCollider != null &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    if (grabbableObject.CompareTag("blanket") ||
                        grabbableObject.CompareTag("extinguisher") ||
                        grabbableObject.CompareTag("water"))
                    {
                        GlobalVariableStorage.availableScore = 500;
                        extinquishFireText.enabled = true;
                        return true;
                    }
                }
            }
            else if (GlobalVariableStorage.level3)
            {
                if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("level3") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    extinquishFireText.enabled = true;
                    return true; // At least one object is close enough
                }
            }

        }
      
        extinquishFireText.enabled = false;
        return false; // No object is close enough
    }
}