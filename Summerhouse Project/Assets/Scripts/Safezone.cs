using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safezone : MonoBehaviour
{
    public Transform targetArea;
    public float detectionRadius = 6.0f;

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetArea.position);
        if (distanceToTarget <= detectionRadius)
        {
            Debug.Log("Object is in the target area!");
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "safezone")
    //        Debug.Log("Mummo turvassa");
    //}
}
