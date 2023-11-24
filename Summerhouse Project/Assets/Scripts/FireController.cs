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

            if (objectGrabbableCollider != null &&
                Vector3.Distance(transform.position, objectGrabbableCollider.transform.position) <= extinguishDistance)
            {
                return true; // At least one object is close enough
            }
        }

        return false; // No object is close enough
    }
}

