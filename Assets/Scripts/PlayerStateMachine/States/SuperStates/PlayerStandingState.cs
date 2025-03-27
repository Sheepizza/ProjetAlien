using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingState : PlayerState
{
    public PlayerStandingState(PlayerStateMachine _stateMachine, PlayerDatas _datas) : base(_stateMachine, _datas)
    {

    }
    public override void Enter()
    {
        Subscribe();
    }

    public override void Exit()
    {
        Unsubscribe();
    }

    public override void PhysicsUpdate()
    {

    }

    void Subscribe()
    {
        InputManager.SprintPerformedActions += ToRunningState;
    }

    void Unsubscribe()
    {
        InputManager.SprintPerformedActions -= ToRunningState;
    }

    void ToRunningState() => stateMachine.ChangeState(stateMachine.RunningState);
}
