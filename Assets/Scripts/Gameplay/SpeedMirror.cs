using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpeedMirror : MonoBehaviour
{

    private Rigidbody rb, targetRB;
    private PlayerController playerController;
    [SerializeField, Tooltip("How much the speed of the physical ball should affect the graphical ball at low speeds"), Range(0f, 100f)]
    private float lowGraphicalSpeedFactor = 10f;
    [SerializeField, Tooltip("How much the speed of the physical ball should affect the graphical ball at high speeds"), Range(0f, 100f)]
    private float highGraphicalSpeedFactor = 50f;
    [SerializeField, Tooltip("The speed from which the ball will only use the highGraphicalSpeedFactor"), Range(0f, 100f)]
    private float highSpeedThreshold = 20f;
    [SerializeField, Tooltip("How much the rotation of the physical ball should affect the graphical ball up in the air"), Range(0f, 1f)]
    private float aerialSpeedFactor = 0.5f;

    [SerializeField, Tooltip("The maximum speed the graphical ball may attain"), Range(0f, 100f)]
    private float maxSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRB = GameObject.Find("Ball").GetComponent<Rigidbody>();
        playerController = GameObject.Find("Ball").GetComponent<PlayerController>();

        rb.maxAngularVelocity = maxSpeed;
    }

    private void FixedUpdate()
    {

        rb.maxAngularVelocity = maxSpeed * (playerController.isOnIce ? 2f : 1f);

        float graphicalSpeedFactor = CalculateGraphicalSpeedFactor();

        if (playerController.isGrounded)
        {

            if (targetRB.angularVelocity.z < 0f)
            {

                rb.angularVelocity = new Vector3(
                0f,
                0f,
                -targetRB.angularVelocity.magnitude * graphicalSpeedFactor
                    );

            }
            else
            {

                rb.angularVelocity = new Vector3(
                0f,
                0f,
                targetRB.angularVelocity.magnitude * graphicalSpeedFactor
                    );

            }

        }
        else
        {

            rb.angularVelocity = targetRB.angularVelocity * aerialSpeedFactor;

        }

    }

    private float CalculateGraphicalSpeedFactor()
    {

        return Mathf.SmoothStep(lowGraphicalSpeedFactor, highGraphicalSpeedFactor,
            targetRB.angularVelocity.magnitude/highSpeedThreshold // This value might need tweaking
            );

    }

}
