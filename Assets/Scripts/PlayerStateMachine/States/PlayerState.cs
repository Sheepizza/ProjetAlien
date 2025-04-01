using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerDatas Datas;
    public PlayerState(PlayerStateMachine _stateMachine, PlayerDatas _datas)
    {
        stateMachine = _stateMachine;
        Datas = _datas;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void PhysicsUpdate();

    public void Move(float _ratio)
    {
        Vector3 _newVel = new Vector3(InputManager.Instance.MoveDirection().x,
            0,
            InputManager.Instance.MoveDirection().y);

        Vector3 _moveDir = stateMachine.GetComponent<Transform>().TransformDirection(_newVel) * Datas.Speed * _ratio;

        stateMachine.GetComponent<Rigidbody>().velocity = new Vector3(_moveDir.x,
            stateMachine.GetComponent<Rigidbody>().velocity.y,
            _moveDir.z);
    }
}
