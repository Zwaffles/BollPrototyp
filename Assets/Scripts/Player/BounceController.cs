using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
public class BounceController : MonoBehaviour
{



    /*
    private PlayerController playerController; // Used to access gravity
    private Rigidbody rb;

    [SerializeField, Header("Trigger"), Tooltip("Minimum gravity (read: airtime) for bounce to trigger"), Range(0f, 40f)]
    private float minimumGravity = 10f;
    [SerializeField, Tooltip("Maximum angle for bounce to trigger"), Range(0f, 180f)]
    private float maximumAngle = 100f;
    [SerializeField, Tooltip("Minimum impulse for bounce to trigger"), Range(0f, 20f)]
    private float minimumImpulse = 2f;

    [SerializeField, Header("Bounce"), Tooltip("How strong the bounce should be overall")]
    private float bounceStrength = 0.1f;
    [SerializeField, Tooltip("How much the gravity (read: airtime) should affect the bounce")]
    private float gravityFactor = 0.1f;

    private void Bounce(Vector3 impulse, float gravity)
    {

        rb.AddForce(bounceStrength * gravity * gravityFactor * impulse, ForceMode.Impulse);

    }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (playerController.Gravity < minimumGravity) return;
        if (Vector3.Angle(rb.velocity, collision.impulse) > maximumAngle) return;
        if (collision.impulse.magnitude < minimumImpulse) return;

        Debug.DrawLine(
            collision.GetContact(0).point,
            collision.GetContact(0).point + collision.impulse,
            Color.red,
            10f
            );

        Bounce(collision.impulse, playerController.Gravity);

    }

    */

}
