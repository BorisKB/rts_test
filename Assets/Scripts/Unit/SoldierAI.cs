using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SoldierAI : MonoBehaviour
{
    protected Unit _Unit;

    protected State _CurrentState;

    protected State _IdleState = new IdleState();
    protected State _MoveState = new MoveState();
    protected State _ChasingState = new ChasingState();
    protected State _AttackState = new AttackState();

    protected virtual void Start()
    {
        _Unit = GetComponent<Unit>();
        _IdleState.Init(_Unit, transform);
        _MoveState.Init(_Unit, transform);
        _ChasingState.Init(_Unit, transform);
        _AttackState.Init(_Unit, transform);
        SetState(_IdleState);
    }
    protected void Update()
    {
        _CurrentState.StateUpdate();
    }
    protected void SetState(State newState)
    {
        Debug.Log(newState);
        _CurrentState = newState;
        _CurrentState.StateStart();

    }

    public void SetIdleState()
    {
        SetState(_IdleState);
    }
    public void SetMoveState()
    {
        SetState(_MoveState);
    }
    public void SetChasingState()
    {
        SetState(_ChasingState);
    }
    public void SetAttackState()
    {
        SetState(_AttackState);
    }


}
