using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLyingState : PlayerCrouchingState
{
    public PlayerLyingState(PlayerStateMachine _stateMachine, PlayerDatas _datas) : base(_stateMachine, _datas)
    {
    }

    public override void Enter()
    {
        VelocityToNull();
        stateMachine.UpdateAnimatorLayer(1, 0);
        stateMachine.UpdateAnimatorLayer(3, 1);
    }

    public override void Exit()
    {
        stateMachine.UpdateAnimatorLayer(3, 0);
        stateMachine.UpdateAnimatorLayer(1, 1);
    }

    public override void PhysicsUpdate()
    {
        Move(Datas.LyingRatio);
        if (!CheckIfUnderObject())
        {
            stateMachine.StartCoroutine(stateMachine.CrouchAnim(true));
        }
    }
}
