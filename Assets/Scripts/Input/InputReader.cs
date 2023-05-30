using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, PlayerInputs.IGameplayActions, PlayerInputs.IUIActions
{
    private PlayerInputs _playerInputs;

    // Events for gameplay actions
    public event Action<Vector2> MoveEvent;
    public event Action HasMovedEvent;
    public event Action JumpEvent;
    public event Action JumpCancelledEvent;
    public event Action PauseEvent;
    public event Action ResumeEvent;
    public event Action CameraCycleEvent;
    public event Action SubmitEvent;

    private void OnEnable()
    {
        // Initialize player inputs and set callbacks
        if (_playerInputs == null)
        {
            _playerInputs = new PlayerInputs();

            _playerInputs.Gameplay.SetCallbacks(instance: this);
            _playerInputs.UI.SetCallbacks(instance: this);

            SetGameplay();
        }
    }

    // Methods to add and remove event listeners for MoveEvent
    public void AddMoveEventListener(Action<Vector2> listener)
    {
        MoveEvent += listener;
    }

    public void RemoveMoveEventListener(Action<Vector2> listener)
    {
        MoveEvent -= listener;
    }

    // Methods to add and remove event listeners for JumpEvent
    public void AddJumpEventListener(Action listener)
    {
        JumpEvent += listener;
    }

    public void RemoveJumpEventListener(Action listener)
    {
        JumpEvent -= listener;
    }

    // Methods to add and remove event listeners for JumpCancelledEvent
    public void AddJumpCancelledEventListener(Action listener)
    {
        JumpCancelledEvent += listener;
    }

    public void RemoveJumpCancelledEventListener(Action listener)
    {
        JumpCancelledEvent -= listener;
    }

    // Methods to add and remove event listeners for SubmitEvent
    public void AddSubmitEventListener(Action listener)
    {
        SubmitEvent += listener;
    }

    public void RemoveSubmitEventListener(Action listener)
    {
        SubmitEvent -= listener;
    }

    // Callback for Move input action
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(obj: context.ReadValue<Vector2>());
        HasMovedEvent?.Invoke();
    }

    // Callback for Jump input action
    public void OnJump(InputAction.CallbackContext context)
    {
        var phase = context.phase;
        if (phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
            HasMovedEvent?.Invoke();
        }
        else if (phase == InputActionPhase.Canceled)
        {
            JumpCancelledEvent?.Invoke();
        }
    }

    // Callback for Pause input action
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
        }
    }

    // Callback for Resume input action
    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
        }
    }

    // Callback for CycleCamera input action
    public void OnCycleCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CameraCycleEvent?.Invoke();
        }
    }

    // Callback for Zoom input action
    public void OnZoom(InputAction.CallbackContext context)
    {
        // Empty implementation
    }

    // Callback for Submit input action
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SubmitEvent?.Invoke();
        }
    }

    // Enable gameplay inputs and disable UI inputs
    public void SetGameplay()
    {
        _playerInputs.Gameplay.Enable();
        _playerInputs.UI.Disable();
    }

    // Enable UI inputs and disable gameplay inputs
    public void SetUI()
    {
        _playerInputs.Gameplay.Disable();
        _playerInputs.UI.Enable();
    }
}
