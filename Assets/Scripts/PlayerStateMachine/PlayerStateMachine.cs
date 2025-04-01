using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState;
    public PlayerDatas Datas;

    #region States
    public PlayerStandingState StandingState;

    public PlayerIdleState IdleState;
    public PlayerWalkState WalkState;
    public PlayerRunningState RunningState;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        CurrentState.PhysicsUpdate();
        Debug.Log(CurrentState);
    }

    void Init()
    {
        CreateStateMachine();
        CurrentState = IdleState;
        CurrentState.Enter();
    }

    void CreateStateMachine()
    {
        StandingState = new PlayerStandingState(this, Datas);

        IdleState = new PlayerIdleState(this, Datas);
        WalkState = new PlayerWalkState(this, Datas);
        RunningState = new PlayerRunningState(this, Datas);
    }

    public void ChangeState(PlayerState _newState)
    {
        CurrentState.Exit();
        CurrentState = _newState;
        CurrentState.Enter();
    }
}
