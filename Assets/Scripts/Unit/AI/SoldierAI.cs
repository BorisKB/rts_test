using UnityEngine;
using UnityEngine.AI;

public class SoldierAI : MonoBehaviour
{
    protected Unit _Unit;

    protected State _CurrentState;

    protected DamagableObject _Target;

    protected State _IdleState = new IdleState();
    protected State _MoveState = new MoveState();
    protected State _ChasingState = new ChasingState();
    protected State _ChasingCurrentTargetState = new ChasingCurrentTargetState();
    protected State _AttackState = new AttackState();

    public DamagableObject GetTarget() { return _Target; }
    public void SetTarget( DamagableObject target) { _Target = target; }
    protected virtual void Start()
    {
        _Unit = GetComponent<Unit>();
        Initialization();
        _CurrentState = _IdleState;
        _CurrentState.StateStart();
    }

    public virtual void Initialization()
    {
        _IdleState.Init(_Unit, transform);
        _MoveState.Init(_Unit, transform);
        _ChasingState.Init(_Unit, transform);
        _ChasingCurrentTargetState.Init(_Unit, transform);
        _AttackState.Init(_Unit, transform);
    }
    protected void Update()
    {
        _CurrentState.StateUpdate();
    }
    protected void SetState(State newState)
    {
        _CurrentState.StateExit();
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
    public void SetChasingCurrentTargetState()
    {
        SetState(_ChasingCurrentTargetState);
    }
    public void SetAttackState()
    {
        SetState(_AttackState);
    }


}
