using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Vector2 movementInput;

   public void OnMovementInput(InputAction.CallbackContext context)
   {
        movementInput = context.ReadValue<Vector2>();
   }
   
    public Vector2 GetMovement()
    {
        return movementInput;
    }
   

}
