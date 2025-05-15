public abstract class AlienState
{
    public AlienState(Alien alien, AlienStateMachine stateMachine)
    {
        this.alien = alien;
        this.stateMachine = stateMachine;
    }

    protected Alien alien;
    protected AlienStateMachine stateMachine;

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();

}
