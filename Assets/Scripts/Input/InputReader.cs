using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, PlayerInputs.IGameplayActions, PlayerInputs.IUIActions
{
    private PlayerInputs _playerInputs;

    /// <summary>
    /// Event triggered when the player moves.
    /// </summary>
    public event Action<Vector2> MoveEvent;

    /// <summary>
    /// Event triggered when the player has moved.
    /// </summary>
    public event Action HasMovedEvent;

    /// <summary>
    /// Event triggered when the player jumps.
    /// </summary>
    public event Action JumpEvent;

    /// <summary>
    /// Event triggered when the player cancels a jump.
    /// </summary>
    public event Action JumpCancelledEvent;

    /// <summary>
    /// Event triggered when the game is paused.
    /// </summary>
    public event Action PauseEvent;

    /// <summary>
    /// Event triggered when the game is resumed.
    /// </summary>
    public event Action ResumeEvent;

    /// <summary>
    /// Event triggered when the camera cycle action is performed.
    /// </summary>
    public event Action CameraCycleEvent;

    /// <summary>
    /// Event triggered when the submit action is performed.
    /// </summary>
    public event Action SubmitEvent;

    private void OnEnable()
    {
        /// <summary>
        /// Initializes player inputs and sets callbacks.
        /// </summary>
        if (_playerInputs == null)
        {
            _playerInputs = new PlayerInputs();

            _playerInputs.Gameplay.SetCallbacks(instance: this);
            _playerInputs.UI.SetCallbacks(instance: this);

            SetGameplay();
        }
    }

    /// <summary>
    /// Adds an event listener for the MoveEvent.
    /// </summary>
    public void AddMoveEventListener(Action<Vector2> listener)
    {
        MoveEvent += listener;
    }

    /// <summary>
    /// Removes an event listener for the MoveEvent.
    /// </summary>
    public void RemoveMoveEventListener(Action<Vector2> listener)
    {
        MoveEvent -= listener;
    }

    /// <summary>
    /// Adds an event listener for the JumpEvent.
    /// </summary>
    public void AddJumpEventListener(Action listener)
    {
        JumpEvent += listener;
    }

    /// <summary>
    /// Removes an event listener for the JumpEvent.
    /// </summary>
    public void RemoveJumpEventListener(Action listener)
    {
        JumpEvent -= listener;
    }

    /// <summary>
    /// Adds an event listener for the JumpCancelledEvent.
    /// </summary>
    public void AddJumpCancelledEventListener(Action listener)
    {
        JumpCancelledEvent += listener;
    }

    /// <summary>
    /// Removes an event listener for the JumpCancelledEvent.
    /// </summary>
    public void RemoveJumpCancelledEventListener(Action listener)
    {
        JumpCancelledEvent -= listener;
    }

    /// <summary>
    /// Adds an event listener for the SubmitEvent.
    /// </summary>
    public void AddSubmitEventListener(Action listener)
    {
        SubmitEvent += listener;
    }

    /// <summary>
    /// Removes an event listener for the SubmitEvent.
    /// </summary>
    public void RemoveSubmitEventListener(Action listener)
    {
        SubmitEvent -= listener;
    }

    /// <summary>
    /// Callback for the Move input action.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(obj: context.ReadValue<Vector2>());
        HasMovedEvent?.Invoke();
    }

    /// <summary>
    /// Callback for the Jump input action.
    /// </summary>
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

    /// <summary>
    /// Callback for the Pause input action.
    /// </summary>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
        }
    }

    /// <summary>
    /// Callback for the Resume input action.
    /// </summary>
    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
        }
    }

    /// <summary>
    /// Callback for the CycleCamera input action.
    /// </summary>
    public void OnCycleCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CameraCycleEvent?.Invoke();
        }
    }

    /// <summary>
    /// Callback for the Zoom input action.
    /// </summary>
    public void OnZoom(InputAction.CallbackContext context)
    {
        // Empty implementation
    }

    /// <summary>
    /// Callback for the Submit input action.
    /// </summary>
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SubmitEvent?.Invoke();
        }
    }

    /// <summary>
    /// Enables gameplay inputs and disables UI inputs.
    /// </summary>
    public void SetGameplay()
    {
        _playerInputs.Gameplay.Enable();
        _playerInputs.UI.Disable();
    }

    /// <summary>
    /// Enables UI inputs and disables gameplay inputs.
    /// </summary>
    public void SetUI()
    {
        _playerInputs.Gameplay.Disable();
        _playerInputs.UI.Enable();
    }
}
