using System.Collections;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    private ObjectGrabbable objectGrabbable;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                float pickUpDistance = 5f;

                // Perform a raycast to check for grabbable objects
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        // Check if the player is close enough to the object before grabbing
                        if (IsPlayerCloseEnough(objectGrabbable.transform.position, pickUpDistance))
                        {
                            Debug.Log(objectGrabbable);
                            objectGrabbable.Grab(objectGrabPointTransform);
                        }
                    }
                }
            }
            else
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }

    // Check if the player is close enough to the object
    private bool IsPlayerCloseEnough(Vector3 objectPosition, float requiredDistance)
    {
        float distance = Vector3.Distance(transform.position, objectPosition);
        return distance <= requiredDistance;
    }
}
