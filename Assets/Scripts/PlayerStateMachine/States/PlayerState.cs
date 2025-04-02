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
        Vector3 _moveInput = stateMachine.GetComponent<Transform>().right * InputManager.Instance.MoveDirection().x + stateMachine.GetComponent<Transform>().forward * InputManager.Instance.MoveDirection().y;
        _moveInput *= Datas.Speed * _ratio;

        stateMachine.GetComponent<Rigidbody>().velocity = new Vector3(_moveInput.x, stateMachine.GetComponent<Rigidbody>().velocity.y, _moveInput.z);
    }

    public void ApplyGravityForce() => stateMachine.GetComponent<Rigidbody>().AddForce(Vector3.down * Datas.GravityMultiplier, ForceMode.Acceleration);

    public void VelocityToNull() => stateMachine.GetComponent<Rigidbody>().velocity = new Vector3(0, stateMachine.GetComponent<Rigidbody>().velocity.y, 0);
}
