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
        Debug.Log("Je chasse");
        if(alien.alienScream.clip == null)
        {
            alien.alienScream.clip = DiegeticSoundManager.Instance.diegeticsSounds["Alien_Scream"].audioClip;
        }
        alien.alienScream.Play();
    }

    public override void ExitState()
    {

    }

    public override void FrameUpdate()
    {
        alien.enemyNavMesh.destination = alien.playerRef.transform.position;

        if(!alien.FOV.canSeePlayer)
        {
            alien.StartCoroutine(alien.StopHunt());
        }

        float distanceToPlayer = Vector3.Distance(alien.transform.position, alien.playerRef.transform.position);
        if(distanceToPlayer < alien.enemyRange)
        {
            
            alien.Killing();
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public void Change()
    {
        stateMachine.ChangeState(alien.searchingState);
    }
}
