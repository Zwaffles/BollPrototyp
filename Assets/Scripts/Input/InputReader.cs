using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, PlayerInputs.IGameplayActions, PlayerInputs.IUIActions
{
    private PlayerInputs _playerInputs;

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
        if(_playerInputs == null)
        {
            _playerInputs = new PlayerInputs();

            _playerInputs.Gameplay.SetCallbacks(instance: this);
            _playerInputs.UI.SetCallbacks(instance: this);

            SetUI();
        }
    }

    public void AddMoveEventListener(Action<Vector2> listener)
    {
        MoveEvent += listener;
    }    
    
    public void RemoveMoveEventListener(Action<Vector2> listener)
    {
        MoveEvent -= listener;
    }    
    
    public void AddJumpEventListener(Action listener)
    {
        JumpEvent += listener;
    }    
    
    public void RemoveJumpEventListener(Action listener)
    {
        JumpEvent -= listener;
    }    
    
    public void AddJumpCancelledEventListener(Action listener)
    {
        JumpCancelledEvent += listener;
    }    
    
    public void RemoveJumpCancelledEventListener(Action listener)
    {
        JumpCancelledEvent -= listener;
    }

    public void AddSubmitEventListener(Action listener)
    {
        SubmitEvent += listener;
    }

    public void RemoveSubmitEventListener(Action listener)
    {
        SubmitEvent -= listener;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(obj: context.ReadValue<Vector2>());
        HasMovedEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        var phase = context.phase;
        if(phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
            HasMovedEvent?.Invoke();
        }

        else if(phase == InputActionPhase.Canceled)
        {
            JumpCancelledEvent?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
        }
    }

    public void OnCycleCamera(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            CameraCycleEvent?.Invoke();
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {

    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        SubmitEvent?.Invoke();
    }

    public void SetGameplay()
    {
        _playerInputs.Gameplay.Enable();
        _playerInputs.UI.Disable();
    }

    public void SetUI()
    {
        _playerInputs.Gameplay.Disable();
        _playerInputs.UI.Enable();
    }
}
