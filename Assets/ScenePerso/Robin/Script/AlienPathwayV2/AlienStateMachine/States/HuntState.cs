using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntState : AlienState
{
    public HuntState(Alien alien, AlienStateMachine stateMachine) : base(alien, stateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Je chasse");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        alien.enemyNavMesh.destination = alien.playerRef.transform.position;

        if(alien.FOV.canKill)
        {
            alien.Killing();
        }
        if(!alien.FOV.canSeePlayer)
        {
            alien.StartCoroutine(alien.StopHunt());
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void Change()
    {
        stateMachine.ChangeState(alien.patrolState);
    }
}
