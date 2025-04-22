using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerStandingState
{
    public PlayerIdleState(PlayerStateMachine _stateMachine, PlayerDatas _datas) : base(_stateMachine, _datas)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Subscribe();
        stateMachine.UpdateAnimatorLayer(0, 1);
        stateMachine.UpdateAnimatorLayer(1, 0);
    }

    public override void Exit()
    {
        base.Exit();
        Unsubscribe();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    void Subscribe()
    {
        InputManager.MovePerformedActions += ToWalk;
        InputManager.CrouchPerformedActions += ToCIdle;
    }
    void Unsubscribe()
    {
        InputManager.MovePerformedActions -= ToWalk;
        InputManager.CrouchPerformedActions -= ToCIdle;
    }

    void ToWalk() => stateMachine.ChangeState(stateMachine.WalkState);
    void ToCIdle() => stateMachine.StartCoroutine(stateMachine.CrouchAnim(false));
}
