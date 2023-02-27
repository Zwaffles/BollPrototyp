using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f; // The starting speed of the ball
    public float acceleration = 0.1f; // The rate at which the speed of the ball increases
    public float maxSpeed = 30f; // The maximum speed the ball can reach
    public float maxSpeedInAir = 20f; // The maximum speed the ball can reach while not grounded
    public float bounceForce = 20f; // The force applied when bouncing back
    public float bounceThreshold = 5f; // The minimum speed required to bounce back
    public float airControlFactor = 0.1f; // The factor to reduce air control by

    private Rigidbody rb; // The Rigidbody2D component of the ball
    private bool isGrounded = false; // A flag to keep course if the ball is on the ground
    private bool isAirControlApplied = false; // A flag to keep course if air control has been applied

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Apply a force to the ball in the right or left direction based on player input
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Epsilon)
        {
            if (speed < (isGrounded ? maxSpeed : maxSpeedInAir))
                speed += acceleration;

            if (!isGrounded && !isAirControlApplied)
            {
                speed *= airControlFactor;
                isAirControlApplied = true;
            }

            if (Input.GetAxis("Horizontal") > 0)
                rb.AddForce(Vector2.right * speed);
            else
                rb.AddForce(-Vector2.right * speed);
        }
        else
        {
            // Reset the speed increase when the player stops
            speed = 10f;
            acceleration = 0.1f;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Check if the ball is on the ground
        if (col.contacts[0].normal.y > 0.5)
        {
            isGrounded = true;
            isAirControlApplied = false;
        }

        // Check if the ball is bouncing back
        if (!isGrounded && col.relativeVelocity.magnitude > bounceThreshold)
        {
            // Apply the bounce force in the opposite direction of the collision
            Vector2 bounceDirection = -col.contacts[0].normal * bounceForce;
            rb.AddForce(bounceDirection, ForceMode.Impulse);

            // Reduce the speed after bouncing back
            speed = Mathf.Max(speed - 10f, 10f);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // Reset the flag when the ball is not on the ground anymore
        isGrounded = false;
    }
}

