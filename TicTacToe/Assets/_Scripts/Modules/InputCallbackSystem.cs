using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCallbackSystem
{
    #region Component Configs

    private SystemState state;
    private InputSettings _inputSettings;

    public event Action<Vector2> OnClick;

    #endregion

    #region Properties

    public SystemState State
    {
        get => state;
        set
        {
            state = value;
            if (state == SystemState.Disable)
            {
                Disable();
                return;
            }
            Enable();
        }
    }

    #endregion

    public InputCallbackSystem()
    {
        _inputSettings = new InputSettings();
        Enable();
    }

    private void Click(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Pointer.current.position.ReadValue();
        OnClick?.Invoke(mousePosition);
    }

    private void Enable()
    {
        _inputSettings.Player.Click.performed += Click;
        _inputSettings.Enable();
    }

    private void Disable()
    {
        _inputSettings.Player.Click.performed -= Click;
        _inputSettings.Disable();
    }
}
