using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]


public class PlayerMovement : MonoBehaviour
{

    private Rigidbody Rb;
    [SerializeField]
    private float moveSpeed = 30f;
 

    [SerializeField]
    private InputManager inputs;


    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        inputs = GetComponent<InputManager>();
        Rb.maxAngularVelocity = moveSpeed;

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Rb.AddTorque(new Vector3(0, 0, -inputs.GetMovement().y * moveSpeed));
    }
}
