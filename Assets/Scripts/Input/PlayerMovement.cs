using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]


public class PlayerMovement : MonoBehaviour
{

    private Rigidbody Rb;
    private float moveSpeed = 30;
 

    [SerializeField]
    private InputManager inputs;


    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Rb.maxAngularVelocity = moveSpeed;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        Rb.AddTorque(new Vector3(0, 0, inputs.GetMovement().y * moveSpeed));

    }
}
