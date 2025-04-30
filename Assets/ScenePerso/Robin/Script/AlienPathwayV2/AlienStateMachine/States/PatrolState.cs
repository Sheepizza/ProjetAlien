using UnityEngine;

public class PatrolState : AlienState
{
    public PatrolState(Alien alien, AlienStateMachine stateMachine) : base(alien, stateMachine)
    {

    }

    public override void EnterState()
    {
        Debug.Log("Entre dans l'état de Patrouille");
        alien.rooms.AddRange(GameObject.FindGameObjectsWithTag("Room"));

        alien.FindRoomManager();
    }

    public override void ExitState()
    {
        alien.inPatrol = false;
    }

    public override void FrameUpdate()
    {

        if (alien.isArrived)
        {
            
        }

        if (alien.FOV.canSeePlayer)
        {
            stateMachine.ChangeState(alien.huntState);
        }
    }

    public override void PhysicsUpdate()
    {

    }
}
