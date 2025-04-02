using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchingState : PlayerState
{
    public PlayerCrouchingState(PlayerStateMachine _stateMachine, PlayerDatas _datas) : base(_stateMachine, _datas)
    {

    }
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void PhysicsUpdate()
    {
        ApplyGravityForce();
        
    }
}
