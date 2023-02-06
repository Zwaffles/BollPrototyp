using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Vector3 movementInput;

   public void OnMovementInput(InputAction.CallbackContext context)
   {
        movementInput = context.ReadValue<Vector3>();
   }
   
    public Vector3 GetMovement()
    {
        return movementInput;
    }
   

}
