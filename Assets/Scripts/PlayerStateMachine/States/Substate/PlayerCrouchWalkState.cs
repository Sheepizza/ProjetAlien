using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchWalkState : PlayerStandingState
{
    public PlayerCrouchWalkState(PlayerStateMachine _stateMachine, PlayerDatas _datas) : base(_stateMachine, _datas)
    {
    }

    public override void Enter()
    {
        VelocityToNull();
        Subscribe();
    }

    public override void Exit()
    {
        Unsubscribe();
    }

    public override void PhysicsUpdate()
    {
        Move(Datas.CrouchRatio);
    }

    void Subscribe()
    {
        InputManager.MoveCanceledActions += ToCIdle;
    }

    void Unsubscribe()
    {
        InputManager.MoveCanceledActions -= ToCIdle;
    }

    void ToCIdle() => stateMachine.ChangeState(stateMachine.CIdleState);
}
