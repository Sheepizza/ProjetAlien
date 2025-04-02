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
        VelocityToNull();
        Unsubscribe();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Move(Datas.SprintRatio);
    }

    void Subscribe()
    {
        InputManager.SprintCanceledActions += EndSprint;
    }

    void Unsubscribe()
    {
        InputManager.SprintCanceledActions -= EndSprint;
    }

    void EndSprint()
    {
        if (InputManager.Instance.InputActions.Player.Move.inProgress)
            stateMachine.ChangeState(stateMachine.WalkState);
        else
            stateMachine.ChangeState(stateMachine.IdleState);
    }
}
