using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Alien : MonoBehaviour
{
    #region State Machine Variables
    public AlienStateMachine StateMachine {get; set;}
    public PatrolState patrolState {get; set;}

    #endregion

    #region AnimationTriggerEvents
    #endregion

    #region components
    #endregion

    #region other variables
    #endregion

    private void Start()
    {
        StateMachine.Initialize(patrolState);
    }
    private void Awake()
    {
        StateMachine = new AlienStateMachine();
        patrolState = new PatrolState(this, StateMachine);
    }

    private void Update()
    {
        StateMachine._CurrentState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine._CurrentState.PhysicsUpdate();
    }
}
