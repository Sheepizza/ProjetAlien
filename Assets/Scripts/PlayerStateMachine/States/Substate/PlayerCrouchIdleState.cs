using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerCrouchingState
{
    public PlayerCrouchIdleState(PlayerStateMachine _stateMachine, PlayerDatas _datas) : base(_stateMachine, _datas)
    {
    }

    public override void Enter()
    {
        VelocityToNull();
        Subscribe();
        stateMachine.UpdateAnimatorLayer(0, 0);
        stateMachine.UpdateAnimatorLayer(1, 1);
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
        InputManager.MovePerformedActions += ToCWalk;
        InputManager.CrouchPerformedActions += ToIdle;
    }
    void Unsubscribe()
    {
        InputManager.MovePerformedActions -= ToCWalk;
        InputManager.CrouchPerformedActions -= ToIdle;
    }

    void ToCWalk() => stateMachine.ChangeState(stateMachine.CWalkState);
    void ToIdle() => stateMachine.StartCoroutine(stateMachine.StandAnim());
}
