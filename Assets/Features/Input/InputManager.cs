using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static InputActions;

public class InputManager : Singleton<InputManager>, IUIActions
{
    public InputActions inputActions { get; private set; }

    #region Events
    public event Action Toolbar = delegate { };
    public event Action LeftClick = delegate { };
    public event Action ScrollWheel = delegate { };
    public event Action<Vector2> MousePos = delegate { };
    #endregion

    public Vector2 MouseDirection => inputActions.UI.Point.ReadValue<Vector2>();

    private protected override void Awake()
    {
        base.Awake();
        CreateUIActions();
    }

    void CreateUIActions()
    {
        inputActions = new InputActions();
        inputActions.UI.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftClick.Invoke();
        }
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        MousePos.Invoke(context.ReadValue<Vector2>());
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ScrollWheel.Invoke();
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnMouseNavigate(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnToolbar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Toolbar.Invoke();
        }
    }
}
