using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState;

    [Header("Datas")]
    public PlayerDatas Datas;

    [Header("Camera"), SerializeField]
    GameObject camHolder;
    [SerializeField]
    Transform camCrouchPos;
    [SerializeField]
    Transform camStandPos;

    [Header("Collider"), SerializeField]
    CapsuleCollider coll;
    [SerializeField]
    float collCrouchHeight; 
    [SerializeField]
    float collStandHeight;

    float t = 0;

    #region States
    public PlayerStandingState StandingState;
    public PlayerCrouchingState CrouchingState;

    public PlayerIdleState IdleState;
    public PlayerWalkState WalkState;
    public PlayerRunningState RunningState;

    public PlayerCrouchIdleState CIdleState;
    public PlayerCrouchWalkState CWalkState;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        CurrentState.PhysicsUpdate();
        Debug.Log(CurrentState);
    }

    void Init()
    {
        CreateStateMachine();
        CurrentState = IdleState;
        CurrentState.Enter();
    }

    void CreateStateMachine()
    {
        StandingState = new PlayerStandingState(this, Datas);
        CrouchingState = new PlayerCrouchingState(this, Datas); 

        IdleState = new PlayerIdleState(this, Datas);
        WalkState = new PlayerWalkState(this, Datas);
        RunningState = new PlayerRunningState(this, Datas);

        CIdleState = new PlayerCrouchIdleState(this, Datas);
        CWalkState = new PlayerCrouchWalkState(this, Datas);
    }

    public void ChangeState(PlayerState _newState)
    {
        CurrentState.Exit();
        CurrentState = _newState;
        CurrentState.Enter();
    }

    public IEnumerator CrouchAnim()
    {
        if (t != 0)
            yield break;

        while (t < .2f)
        {
            t += Time.deltaTime;
            camHolder.transform.localPosition = Vector3.Lerp(camStandPos.transform.localPosition, camCrouchPos.transform.localPosition, t/.2f);
            yield return null;
        }

        coll.center = new Vector3(coll.center.x, -.5f, coll.center.z);
        coll.height = collCrouchHeight;
        t = 0;
        ChangeState(CIdleState);
    }

    public IEnumerator StandAnim()
    {
        if (t != 0)
            yield break;

        if (Physics.Raycast(transform.position, transform.up, 2f))
            yield break;

            while (t < .2f)
        {
            t += Time.deltaTime;
            camHolder.transform.localPosition = Vector3.Lerp(camCrouchPos.transform.localPosition, camStandPos.transform.localPosition, t/.2f);
            yield return null;
        }

        coll.center = new Vector3(coll.center.x, 0, coll.center.z);
        coll.height = collStandHeight;
        t = 0;
        ChangeState(IdleState);
    }
}
