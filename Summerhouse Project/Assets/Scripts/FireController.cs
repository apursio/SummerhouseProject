using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public float extinguishDistance = 0.5f;  // Adjust this distance based on your needs

    private void Update()
    {
        // Check for left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Check if any ObjectGrabbable is close enough to extinguish the fire
            if (IsObjectGrabbableNearFire())
            {
                // Disable the fire
                if(GlobalVariableStorage.fireIsOutofControl == false)
                {
                    gameObject.SetActive(false);
                    GlobalVariableStorage.fireIsOut = true;
                    GlobalVariableStorage.actionScore = GlobalVariableStorage.availableScore;
                    Debug.Log("Fire is out from firecontroller");
                }else
                {
                    GlobalVariableStorage.taskTimeLeft = 0;
                    GlobalVariableStorage.timeLeft = 60;
                    Debug.Log("Fire is out of Control");
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


        // Check if any of the grabbable objects are close enough to the fire
        foreach (ObjectGrabbable grabbableObject in grabbableObjects)
        {
            Collider objectGrabbableCollider = grabbableObject.GetComponent<Collider>();
            if (GlobalVariableStorage.level1)
            {
                if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("lid") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                    {
                        Debug.Log("If level 1 and crabbables");
                        GlobalVariableStorage.availableScore = 500;
                        return true; // At least one object is close enough
                    }
                else if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("blanket") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    Debug.Log("If level 1 and crabbables");
                    GlobalVariableStorage.availableScore = 500;
                    return true; // At least one object is close enough
                }
                if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("extinguisher") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    Debug.Log("If level 1 and crabbables");
                    GlobalVariableStorage.availableScore = 100;
                    return true; // At least one object is close enough
                }
                else if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("water") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                    {
                        Debug.Log("If level 1 and water");
                        GlobalVariableStorage.availableScore = 0;
                        GlobalVariableStorage.fireIsOutofControl = true;
                        Debug.Log("Fire out of control");
                        return true; // At least one object is close enough
                    }
            }
            if (GlobalVariableStorage.level2)
            {
                if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("level2") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    Debug.Log("If level 2 and crabbables"); 
                    return true; // At least one object is close enough
                }
            }
            if (GlobalVariableStorage.level3)
            {
                if (objectGrabbableCollider != null &&
                    grabbableObject.CompareTag("level3") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                {
                    Debug.Log("If level 3 and crabbables"); 
                    return true; // At least one object is close enough
                }
            }

        }
        Debug.Log("Object crabbable false");
        return false; // No object is close enough
    }
}

