using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class SlopePush : MonoBehaviour
{

    Rigidbody rb;

    [SerializeField, Range(0f, 30f), Tooltip("How slow the ball is allowed to get before the push is activated")]
    private float minimumVelocity = 3f;
    [SerializeField, Range(0f, 30f), Tooltip("How fast the ball needs to spin to trigger the effect")]
    private float minimumAngularVelocity = 25f;

    private int currentCollisions = 0;

    void Start() => rb = GetComponent<Rigidbody>();

    void Update()
    {

        if (
            rb.velocity.magnitude >= minimumVelocity ||
            rb.angularVelocity.magnitude <= minimumAngularVelocity ||
            currentCollisions < 1 ||
            rb.angularVelocity.z > 0f && rb.velocity.x > 0f ||
            rb.angularVelocity.z < 0f && rb.velocity.x < 0f
            ) return;

        rb.velocity = Vector3.Lerp(
            rb.velocity,
            rb.velocity.normalized * minimumVelocity,
            Time.deltaTime
            );

    }

    private void OnCollisionEnter(Collision collision) => currentCollisions++;

    private void OnCollisionExit(Collision collision) => currentCollisions--;

}
