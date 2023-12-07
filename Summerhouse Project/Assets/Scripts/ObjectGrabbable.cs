using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private Collider objectCollider; // Store the object's collider
    private Collider playerCollider; // Assuming the player has a collider
    private Quaternion initialRotation; // Store the initial rotation of the object

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();

        // Store the initial rotation of the object
        initialRotation = objectRigidbody.rotation;
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectCollider = GetComponent<Collider>(); // Store the object's collider

       
        objectRigidbody.useGravity = false;
        objectRigidbody.isKinematic = true;
        objectRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        objectCollider.isTrigger = true;

        // Ignore collisions between the object and the player
        Physics.IgnoreCollision(objectCollider, playerCollider, true);

        objectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;

        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
        objectRigidbody.interpolation = RigidbodyInterpolation.None;
        objectCollider.isTrigger = false;

        // Restore collision between the object and the player
        if (objectCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, false);
        }

        objectRigidbody.constraints = RigidbodyConstraints.None;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);

            objectRigidbody.rotation = initialRotation; // Set rotation directly
        }
    }
}
