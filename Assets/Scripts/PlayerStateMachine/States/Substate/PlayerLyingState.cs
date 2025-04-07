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
    }

    public override void Exit()
    {
       
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
