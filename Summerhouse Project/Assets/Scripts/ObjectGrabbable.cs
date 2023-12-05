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
    private CollisionDetectionMode originalCollisionMode;

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

        // old rotation code place 

        originalCollisionMode = objectRigidbody.collisionDetectionMode; // Store the original collision detection mode

        // Set collision detection to Continuous when grabbed
        objectRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        objectRigidbody.interpolation = RigidbodyInterpolation.Extrapolate;

        objectRigidbody.useGravity = false;

        // Ignore collisions between the object and the player
        Physics.IgnoreCollision(objectCollider, playerCollider, true);

        objectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;

        objectRigidbody.useGravity = true;

        // Restore collision between the object and the player
        if (objectCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, false);
        }

        objectRigidbody.constraints = RigidbodyConstraints.None;

        objectRigidbody.collisionDetectionMode = originalCollisionMode;

        objectRigidbody.interpolation = RigidbodyInterpolation.None;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);

            // Freeze the rotation of the object
            objectRigidbody.rotation = initialRotation; // Set rotation directly
        }
    }
}
