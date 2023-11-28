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
                gameObject.SetActive(false);
                GlobalVariableStorage.fireIsOut = true;
                Debug.Log("Fire is out from firecontroller");
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
                    grabbableObject.CompareTag("level1") &&
                    Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
                    {
                        Debug.Log("If level 1 and crabbables");
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

