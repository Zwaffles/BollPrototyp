using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script for having limited aerial control
 * Johan Rosenberg
 * 2023-02-08
 */

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
public class AirControl : MonoBehaviour
{
    // Components
    private Rigidbody rb;
    private InputManager inputs;

    [SerializeField, Header("Aerial control variables"), Tooltip("How strong the aerial control is to begin with.")]
    private float aerialControlFactor = 10f;
    [SerializeField, Tooltip("How much the aerial control drops off per second, higher = faster.")]
    private float aerialControlDropoff = 1f;
    private float tempAerialControlFactor = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputs = GetComponent<InputManager>();
    }

    private void FixedUpdate()
    {

        float zInput = inputs.GetMovement().y;

        if (zInput != 0f)
        {
            rb.AddForce(Vector3.up * zInput * tempAerialControlFactor);
            tempAerialControlFactor -= aerialControlDropoff * Time.fixedDeltaTime;

            tempAerialControlFactor = tempAerialControlFactor < 0f ? 0f : tempAerialControlFactor;
        }

    }

    // Copied from PlayerJump with modifications
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            tempAerialControlFactor = aerialControlFactor;

        }
    }

}
