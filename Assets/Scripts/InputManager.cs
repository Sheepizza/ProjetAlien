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

    public Vector2 MoveDirection() => InputManager.Instance.InputActions.Player.Move.ReadValue<Vector2>();
}
