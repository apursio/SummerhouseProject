using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private TextMeshProUGUI pickUpPromptText;
    [SerializeField] private float pickUpDistance = 5f;

    private ObjectGrabbable objectGrabbable;
    void Update()
    {
        HandlePickUpInput();

        UpdatePromptTextVisibility();
    }

    private void HandlePickUpInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                TryGrabObject();
            }
            else
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }

    private void TryGrabObject()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out objectGrabbable))
            {
                Debug.Log(objectGrabbable);
                objectGrabbable.Grab(objectGrabPointTransform);
            }
        }
    }

    private void UpdatePromptTextVisibility()
    {
        if (objectGrabbable == null)
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
            {
                float distanceToGrabbable = Vector3.Distance(playerCameraTransform.position, raycastHit.point);

                if (distanceToGrabbable <= pickUpDistance)
                {
                    ObjectGrabbable grabbable = raycastHit.transform.GetComponent<ObjectGrabbable>();

                    if (grabbable != null)
                    {
                        pickUpPromptText.enabled = true;
                    }
                    else
                    {
                        pickUpPromptText.enabled = false;
                    }
                }
                else
                {
                    pickUpPromptText.enabled = false;
                }
            }
            else
            {
                pickUpPromptText.enabled = false;
            }
        }
        else
        {
            pickUpPromptText.enabled = false;
        }
    }

}