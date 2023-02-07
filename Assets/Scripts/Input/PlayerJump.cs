using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    public Rigidbody Rb;
    private bool IsOnGround;
    private bool ShouldJump = false;


    [SerializeField]
    private float JumpWindow = 1f;
    private InputManager inputs;
    


    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        inputs = GetComponent<InputManager>();
    }

    // Update is called once per frame

    void Update()

    {
        ShouldJump = inputs.jumpInput;

    }

    private void FixedUpdate()
    {
       

        if (inputs.TimeWhenJumpWasPressed + JumpWindow > Time.time && ShouldJump && IsOnGround)
        {
           
            Rb.AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
            inputs.jumpInput = false;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsOnGround = true;

        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsOnGround = false;

        }
    }

}
    
    
