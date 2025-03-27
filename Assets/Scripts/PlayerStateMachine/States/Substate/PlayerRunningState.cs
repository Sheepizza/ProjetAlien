using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerStandingState
{
    public PlayerRunningState(PlayerStateMachine _stateMachine, PlayerDatas _datas) : base(_stateMachine, _datas)
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
        Move(Datas.SprintRatio);
    }

    void Subscribe()
    {
        InputManager.SprintCanceledActions += ToIdle;
    }

    void Unsubscribe()
    {
        InputManager.SprintCanceledActions -= ToIdle;
    }

    void ToIdle() => stateMachine.ChangeState(stateMachine.IdleState);
}
