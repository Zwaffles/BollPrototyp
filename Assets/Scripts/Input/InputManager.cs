using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Vector2 movementInput;
    public bool jumpInput = false;
    public float TimeWhenJumpWasPressed = 0;

    private bool cameraInput = false;

    public void OnMovementInput(InputAction.CallbackContext context)
   {
        movementInput = context.ReadValue<Vector2>();
   }
   
    public Vector2 GetMovement()
    {
        return movementInput;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {

        if (context.action.triggered == true)
        {
            TimeWhenJumpWasPressed = Time.time;
            jumpInput = true;
        }


    }

    public void OnCameraCycleInput(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            cameraInput = true;
        }

    }

    public bool getCameraInput()
    {
        if (cameraInput)
        {
            cameraInput = false;
            return true;
        }
        else
        {
            return false;
        }
    }

}
