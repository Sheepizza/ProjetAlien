using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerStandingState
{
    public PlayerWalkState(PlayerStateMachine _stateMachine, PlayerDatas _datas) : base(_stateMachine, _datas)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Subscribe();
    }

    public override void Exit()
    {
        base.Exit();
        VelocityToNull();
        Unsubscribe();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Move(Datas.InitRatio);
    }

    void Subscribe()
    {
        InputManager.MoveCanceledActions += ToIdle;
    }

    void Unsubscribe()
    {
        InputManager.MoveCanceledActions -= ToIdle;
    }

    void ToIdle() => stateMachine.ChangeState(stateMachine.IdleState);
}
