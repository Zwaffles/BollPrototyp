using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class SlopePush : MonoBehaviour
{

    Rigidbody rb;

    [SerializeField, Range(0f, 30f), Tooltip("How slow the ball is allowed to get before the push is activated")]
    private float minimumVelocity = 6f;
    [SerializeField, Tooltip("How quickly the effect wears off, higher = faster")]
    private float dropOffRate = 0.5f;
    [SerializeField, Tooltip("How low the velocity is allowed to get before it zeroes out")]
    private float absoluteMinimumThreshold = 0.1f;
    private float temporaryMinimumVelocity;
    [SerializeField, Range(0f, 30f), Tooltip("How fast the ball needs to spin to trigger the effect")]
    private float minimumAngularVelocity = 25f;

    private int currentCollisions = 0;

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();

        temporaryMinimumVelocity = minimumVelocity;

    }

    void Update()
    {

        if ( // These conditions block the push
            rb.angularVelocity.magnitude <= minimumAngularVelocity ||   // Has to have input (this is a scuffed way of getting input)
            rb.angularVelocity.z > 0f && rb.velocity.x > 0f ||          // Has to be going in the same direction as the input
            rb.angularVelocity.z < 0f && rb.velocity.x < 0f ||          // Same as above
            currentCollisions < 1                                       // Has to be grounded
            ) return;

        if ( // These conditions block & reset the push
            rb.velocity.magnitude >= minimumVelocity                    // If you reach the necessary speed
            )
        {
            temporaryMinimumVelocity = minimumVelocity;
            return;
        }

        rb.velocity = Vector3.Lerp(
            rb.velocity,
            rb.velocity.normalized * temporaryMinimumVelocity,
            Time.deltaTime
            );

        temporaryMinimumVelocity -= Time.deltaTime * dropOffRate;
        temporaryMinimumVelocity = (temporaryMinimumVelocity + absoluteMinimumThreshold) < 0f ? 0f : temporaryMinimumVelocity;

    }

    private void OnCollisionEnter(Collision collision) => currentCollisions++;

    private void OnCollisionExit(Collision collision) => currentCollisions--;

}
