using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerStateMachine : NetworkBehaviour
{
    public PlayerState CurrentState;

    [Header("Animator")]
    public Animator Animator;

    [Header("Datas")]
    public PlayerDatas Datas;

    [Header("Camera"), SerializeField]
    GameObject camHolder;
    [SerializeField]
    Transform camCrouchPos;
    [SerializeField]
    Transform camLyingPos;
    [SerializeField]
    Transform camStandPos;

    [Header("Collider"), SerializeField]
    CapsuleCollider coll;
    [SerializeField]
    float collCrouchHeight; 
    [SerializeField]
    float collStandHeight;

    float t = 0;
    float timeInSprint = 0;

    [SyncVar] float syncHor;
    [SyncVar] float syncVert;

    [SyncVar] float layerWeight;

    #region States
    public PlayerStandingState StandingState;
    public PlayerCrouchingState CrouchingState;

    public PlayerIdleState IdleState;
    public PlayerWalkState WalkState;
    public PlayerRunningState RunningState;

    public PlayerCrouchIdleState CIdleState;
    public PlayerCrouchWalkState CWalkState;
    public PlayerLyingState LyingState;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        CurrentState.PhysicsUpdate();

        if (isLocalPlayer)
        {
            Vector2 moveDir = InputManager.Instance.MoveDirection();
            CmdUpdateAnimator(moveDir.x, moveDir.y);
        }

        Animator.SetFloat("Horizontal", syncHor);
        Animator.SetFloat("Vertical", syncVert);
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
        LyingState = new PlayerLyingState(this, Datas);
    }

    public void ChangeState(PlayerState _newState)
    {
        CurrentState.Exit();
        CurrentState = _newState;
        CurrentState.Enter();
    }

    public IEnumerator CrouchAnim(bool _toWalk)
    {
        if (t != 0)
            yield break;

        Vector3 _basePos = camHolder.transform.localPosition;

        while (t < .2f)
        {
            t += Time.deltaTime;
            camHolder.transform.localPosition = Vector3.Lerp(_basePos, camCrouchPos.transform.localPosition, t/.2f);
            yield return null;
        }

        coll.center = new Vector3(coll.center.x, -.5f, coll.center.z);
        coll.height = collCrouchHeight;
        t = 0;

        if (_toWalk)
        {
            ChangeState(CWalkState);
            yield break;
        }
        
        ChangeState(CIdleState);
    }

    public IEnumerator LyingAnim()
    {
        if (t != 0)
            yield break;

        while (t < .2f)
        {
            t += Time.deltaTime;
            camHolder.transform.localPosition = Vector3.Lerp(camCrouchPos.transform.localPosition, camLyingPos.transform.localPosition, t / .2f);
            yield return null;
        }

        t = 0;
        ChangeState(LyingState);
    }

    public IEnumerator StandAnim()
    {
        if (t != 0)
            yield break;

        if (Physics.Raycast(transform.position, transform.up, 1f))
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

    public IEnumerator SprintLimit()
    {
        while (CurrentState == RunningState)
        {
            timeInSprint += Time.deltaTime;
            yield return null;
            if (timeInSprint >= Datas.SprintTimer)
            {
                timeInSprint = Datas.SprintTimer;
                ChangeState(WalkState);
            } 
        }
        StartCoroutine(ReloadSprint());
        yield break;
    }

    public IEnumerator ReloadSprint()
    {
        yield return new WaitForSeconds(1f);
        while (CurrentState != RunningState)
        {
            float _ratio = CurrentState == IdleState ? Datas.SprintIdleReload : Datas.SprintBaseReload;
            Debug.Log(Time.deltaTime / _ratio);
            timeInSprint -= Time.deltaTime / _ratio;
            yield return null;
            if (timeInSprint <= 0)
            {
                timeInSprint = 0;
                yield break;
            }
        }
        yield break;
    }

    public void UpdateAnimatorLayer(int _index, float _weight)
    {
        CmdUpdateAnimatorLayer(_index, _weight);
    }

    [Command]
    void CmdUpdateAnimator(float _hor, float _vert)
    {
        syncHor = _hor;
        syncVert = _vert;
    }

    [Command]
    void CmdUpdateAnimatorLayer(int _index, float _weight)
    {
        RPCUpdateAnimatorLayer(_index, _weight);
    }

    [ClientRpc]
    void RPCUpdateAnimatorLayer(int _index, float _weight)
    {
        layerWeight = _weight;
        Animator.SetLayerWeight(_index, layerWeight);
    }
}
