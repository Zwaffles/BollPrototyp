using System;
using System.Collections;
using System.Collections.Generic;
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

    private void OnEnable()
    {
        if(_playerInputs == null)
        {
            _playerInputs = new PlayerInputs();

            _playerInputs.Gameplay.SetCallbacks(instance: this);
            _playerInputs.UI.SetCallbacks(instance: this);

            SetGameplay();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(obj: context.ReadValue<Vector2>());
        HasMovedEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
            HasMovedEvent?.Invoke();
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            JumpCancelledEvent?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
            SetUI();
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
            SetGameplay();
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
