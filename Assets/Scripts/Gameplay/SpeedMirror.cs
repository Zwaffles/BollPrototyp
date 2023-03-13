using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpeedMirror : MonoBehaviour
{

    private Rigidbody rb, targetRB;
    [SerializeField, Tooltip("How much the speed of the physical ball should affect the graphical ball"), Range(-2f,2f)]
    private float graphicalSpeedFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRB = GameObject.Find("Ball").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // This sorta works, but for slopes I think we need to take the y into account too
        rb.angularVelocity = new Vector3(
            -targetRB.velocity.x * graphicalSpeedFactor,
            -targetRB.velocity.x * graphicalSpeedFactor,
            -targetRB.velocity.x * graphicalSpeedFactor
                );

    }
}
