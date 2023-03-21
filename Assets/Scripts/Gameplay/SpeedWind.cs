using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedWind : MonoBehaviour
{

    private Rigidbody targetRB;
    [SerializeField, Tooltip("Maximum tilt of the hat"), Range(0f, 180f)]
    private float highGraphicalSpeedFactor = 50f;
    [SerializeField, Tooltip("The speed at which the hat achieves max tilt"), Range(0f, 100f)]
    private float highSpeedThreshold = 20f;

    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {

        targetRB = GameObject.Find("Ball").GetComponent<Rigidbody>();

    }

    private void Update()
    {

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1f);

    }

    private void FixedUpdate()
    {

        if (targetRB.velocity.x > 0f)
        {

            targetRotation = Quaternion.Euler(
            0f,
            0f,
            Mathf.SmoothStep(0f, highGraphicalSpeedFactor,
            targetRB.velocity.x / highSpeedThreshold)
            );

        }
        else
        {

            targetRotation = Quaternion.Euler(
            0f,
            0f,
            -Mathf.SmoothStep(0f, highGraphicalSpeedFactor,
            -targetRB.velocity.x / highSpeedThreshold)
            );

        }

    }

}
