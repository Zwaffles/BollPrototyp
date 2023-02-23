using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  PLACEHOLDER
 *  Plays SFX and makes you go weee
 *  Copied from SFX_Play_Bounce_Sound
 *  2023-02-20
 */
[RequireComponent(typeof(BounceSquish))]
public class BounceTrigger : MonoBehaviour
{

    //Rigidbody
    private Rigidbody ballCollisionRb;

    private BounceSquish bounce;

    private int currentCollisions = 0;

    [SerializeField, Tooltip("How strong the impulse from a collision has to be to trigger the SFX")]
    [Range(0f, 40f)]
    private float minimumImpulse = 20f;

    [SerializeField, Tooltip("How strong the impulse from a collision has to be to trigger the SFX")]
    [Range(0f, 180f)]
    private float minimumAngle = 60.86f;

    private bool shouldBounce = false;
    private Vector3 bounceDirection;

    //On Event Start
    void Start()
    {

        ballCollisionRb = GetComponent<Rigidbody>();
        bounce = GetComponent<BounceSquish>();

    }

    void Update()
    {

        if (!shouldBounce) return;

        bounce.triggerBounce(bounceDirection);

        shouldBounce = false;

    }


    //On Event Colliding With Collision
    private void OnCollisionEnter(Collision collision)
    {

        if (currentCollisions++ != 0) return;

        if (collision.impulse.magnitude < minimumImpulse) return;

        if (Vector3.Angle(collision.impulse, ballCollisionRb.velocity) < minimumAngle) return; // This line triggers a null ref for some reason?

        shouldBounce = true;
        bounceDirection = collision.contacts[0].normal;

    }

    private void OnCollisionExit(Collision collision)
    {
        currentCollisions--;
    }

    private void OnEnable()
    {
        shouldBounce = false;
    }

}
