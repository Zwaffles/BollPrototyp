using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BounceController : MonoBehaviour
{

    [SerializeField, Header("Bounce Controls"), Tooltip("How strong the impulse needs to be for a bounce to trigger"), Range(0f, 20f)]
    private float impulseThreshold = 1f;
    [SerializeField, Tooltip("How much bias the bounce should have horizontally and vertically")]
    private Vector2 bounceBias = new Vector2(1f, 1f);
    [SerializeField, Tooltip("How weak the bounce should be. DO NOT USE THIS TO STRENGTHEN THE BOUNCE"), Range(0f, 1f)]
    private float bounceStrength = 0.5f;

    private Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {

        // This is where the bounce happens

        //Debug.DrawLine(transform.position, transform.position + collision.impulse * 0.5f, Color.red, 1f);

        if (collision.impulse.magnitude < impulseThreshold) return;

        rb.AddForce(collision.impulse * bounceBias * bounceStrength, ForceMode.Impulse);

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

}
