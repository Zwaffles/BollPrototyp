using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Bouncing script
 * Copied from earlier version by Johan Rosenberg
 * 2023-02-20
 */

public class BounceSquish : MonoBehaviour
{

    private bool isBouncing = false;
    private bool shouldBounce = false;

    private float bouncingTimer = 0f;

    [SerializeField, Header("Bouncing"), Tooltip("How quickly the animation plays out; higher = quicker")]
    private float bouncingTimeFactor = 7f;
    [SerializeField, Tooltip("How much time in seconds the ball stays squeezed, and the player can do the skill")]
    private float bouncingHoldTime = 1f;
    [SerializeField, Tooltip("How much the ball will stretch on the XZ-plane; 2 = double")]
    private float maxStretch = 2f;
    [SerializeField, Tooltip("How much the ball will squeeze on the Y-axis; 0.5 = half")]
    private float maxSqueeze = 0.5f;
    [SerializeField, Tooltip("How strongly the ball bounces physically; higher = more")]
    private float baseBounceFactor = 400f;

    [SerializeField]
    private GameObject ballHolder, holderHolder;

    private Rigidbody rb;

    private Vector3 forceToBeAdded, bounceDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        if (!shouldBounce)
        {
            return;
        }

        forceToBeAdded = bounceDirection * baseBounceFactor;
        shouldBounce = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ;
        rb.AddForce(forceToBeAdded);

    }

    private void Update()
    {

        if (isBouncing)
        {

            bouncingTimer += Time.deltaTime * bouncingTimeFactor;
            if (bouncingTimer > 1f + bouncingHoldTime) // Squish finished
            {
                isBouncing = false;
            }

        }
        else if (bouncingTimer > 0f)
        {

            bouncingTimer -= Time.deltaTime * bouncingTimeFactor;
            if (bouncingTimer < 0f) // Bounce-up finished
            {
                bouncingTimer = 0f;
                shouldBounce = true;
            }

        }

        ballHolder.transform.localScale = new Vector3(
            Mathf.Lerp(1f, maxStretch, bouncingTimer > 1f ? 1f : bouncingTimer),
            Mathf.Lerp(1f, maxSqueeze, bouncingTimer > 1f ? 1f : bouncingTimer),
            Mathf.Lerp(1f, maxStretch, bouncingTimer > 1f ? 1f : bouncingTimer)
            );

    }

    public void triggerBounce(Vector3 _bounceDirection)
    {
        bounceDirection = _bounceDirection;

        float angle = Mathf.Atan2(
            bounceDirection.y,
            bounceDirection.x
            ) * Mathf.Rad2Deg - 90f;

        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = new Vector3(0f, 0f, 0f);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;

        holderHolder.transform.eulerAngles = new Vector3(0, 0, angle);

        isBouncing = true;

    }

}
