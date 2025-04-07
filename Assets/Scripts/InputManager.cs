using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance = null;
    public static InputManager Instance => instance;

    public InputActionsSystem InputActions;

    #region Actions
    public static Action SprintPerformedActions;
    public static Action SprintCanceledActions;
    public static Action MovePerformedActions;
    public static Action MoveCanceledActions;
    public static Action CrouchPerformedActions;
    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnEnable()
    {
        InputActions = new InputActionsSystem();
        InputActions.Enable();
        InputActions.Player.Sprint.performed += OnSprintPerformed;
        InputActions.Player.Sprint.canceled += OnSprintCanceled;
        InputActions.Player.Move.performed += OnMovePerformed;
        InputActions.Player.Move.canceled += OnMoveCanceled;
        InputActions.Player.Crouch.performed += OnCrouchPerformed;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        SprintPerformedActions?.Invoke();
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        SprintCanceledActions?.Invoke();
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MovePerformedActions?.Invoke();
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        MoveCanceledActions?.Invoke();
    }
    private void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        CrouchPerformedActions?.Invoke();
    }

    public Vector2 MoveDirection() => InputManager.Instance.InputActions.Player.Move.ReadValue<Vector2>();
}
