using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum StatePattern
{
    IdleState,
    ChasingState,
    MoveState,
    AttackState,
    MiningState,
    BuildingState,
    StorageState
}
public class IdleState : State
{
    public override void StateStart()
    {
        _Timer = 0f;
        _CurrentTarget = _SoldierAI.GetTarget();
        _Agent.stoppingDistance = _StoppingDistance;
    }
    public override void StateUpdate()
    {
        _Timer += Time.deltaTime;
        if (_Timer > 1)
        {
            _Timer = 0;
            _CurrentTarget = _Unit.FindClosestTarget(_ChasingRange);
        }
        if (_CurrentTarget != null)
        {
            _SoldierAI.SetTarget(_CurrentTarget);
            _SoldierAI.SetChasingState();
        }
    }
}
public class MoveState : State
{
    public override void StateStart()
    {
        _Animator.SetBool("Run", true);
        _Agent.stoppingDistance = _StoppingDistance;
        _Timer = 0;
    }
    public override void StateUpdate()
    {
        _Timer += Time.deltaTime;
        if (_Timer > 0.15f)
        {
            if ((_Agent.pathEndPosition - _SoldierAI.transform.position).sqrMagnitude <= _StoppingDistance * _StoppingDistance)
            {
                _SoldierAI.SetIdleState();
            }
        }
    }
    public override void StateExit()
    {
        _Animator.SetBool("Run", false);
    }
}
public class ChasingCurrentTargetState : State
{
    public override void StateStart()
    {
        _CurrentTarget = _SoldierAI.GetTarget();
        _Agent.stoppingDistance = _AttackRange - 0.5f;
        _Animator.SetBool("Chasing", true);
    }
    public override void StateUpdate()
    {
        if (_CurrentTarget == null) {  _Agent.stoppingDistance = _StoppingDistance; _SoldierAI.SetIdleState(); return; }
   
        _Distance = (_CurrentTarget.transform.position - _TransformUnit.position).sqrMagnitude;
        if (_Distance <= _AttackRangeSqr)
        {
            _Agent.stoppingDistance = _StoppingDistance;
            _SoldierAI.SetAttackState();
        }
        else
        {
            _Agent.SetDestination(_CurrentTarget.transform.position);
        }
    }
    public override void StateExit()
    {
        _Animator.SetBool("Chasing", false);
    }
    public override void Init(Unit unit, Transform transform)
    {
        base.Init(unit, transform);
        _StoppingDistance = _Agent.stoppingDistance;
    }
}
public class ChasingState : State
{
    public override void StateStart()
    {
        _CurrentTarget = _SoldierAI.GetTarget();
        _Agent.stoppingDistance = _AttackRange - 0.5f;
        _Animator.SetBool("Chasing", true);
    }
    public override void StateUpdate()
    {
        if (_CurrentTarget == null) {  _Agent.stoppingDistance = _StoppingDistance; _SoldierAI.SetIdleState(); return; }

        _Distance = (_CurrentTarget.transform.position - _TransformUnit.position).sqrMagnitude;
        if (_Distance < _ChasingRange)
        {
            if (_Distance <= _AttackRangeSqr)
            {
                _Agent.stoppingDistance = _StoppingDistance;
                _SoldierAI.SetAttackState();
            }
            else
            {
                _Agent.SetDestination(_CurrentTarget.transform.position);
            }
        }
        else
        {
            _SoldierAI.SetTarget(null);
            _SoldierAI.SetIdleState();
        }
    }
    public override void StateExit()
    {
            _Animator.SetBool("Chasing", false);
    }
}
public class AttackState : State
{
    private float _TimeToSendDamage;
    public override void StateStart()
    {
        _Timer = 0f;
        _Agent.ResetPath();
        _CurrentTarget = _SoldierAI.GetTarget();
        _IsAttack = false;
        _Animator.SetTrigger("Attack");
    }

    public override void StateUpdate()
    {
        if(_CurrentTarget == null) { _SoldierAI.SetIdleState(); return; }

        _Timer += Time.deltaTime;
        _Unit.transform.LookAt(_CurrentTarget.transform);
        if ((_IsAttack == false) && (_Timer >= _TimeToSendDamage))
        {
            _IsAttack = true;
            _Unit.AttackEnemy(_CurrentTarget);
        }
        else if (_Timer >= _AttackRate)
        {
            _SoldierAI.SetIdleState();
        }
    }
    public override void Init(Unit unit, Transform transform)
    {
        base.Init(unit, transform);
        _TimeToSendDamage = unit.GetTimeToSendDamage();
    }

}
public class State
{
    protected Unit _Unit;
    protected NavMeshAgent _Agent;//
    protected Animator _Animator;
    protected Transform _TransformUnit;//
    protected DamagableObject _CurrentTarget;

    protected float _ChasingRange;//
    protected float _AttackRangeSqr;//
    protected float _AttackRange;//
    protected float _StoppingDistance;

    protected float _AttackRate;//
    protected bool _IsAttack = false;//

    protected float _Timer;//
    protected float _Distance;//
    protected SoldierAI _SoldierAI;

    public virtual void Init(Unit unit, Transform transform)
    {
        _Unit = unit;
        _TransformUnit = transform;
        _Animator = _Unit.GetAnimator();
        _Agent = _Unit.GetAgent();
        _SoldierAI = _Unit.GetSoldierAI();
        _ChasingRange = _Unit.GetChasingRange();
        _AttackRangeSqr = _Unit.GetAttackRange();
        _AttackRate = _Unit.GetAttackRate();
        _AttackRange = _AttackRangeSqr;
        _StoppingDistance = _Agent.stoppingDistance;
        _AttackRangeSqr = _AttackRangeSqr * _AttackRangeSqr;
        _ChasingRange = _ChasingRange * _ChasingRange;
    }
    protected virtual void Start()
    {
    }

    public virtual void StateUpdate()
    {

    }
    public virtual void StateStart()
    {

    }
    public virtual void StateExit()
    {

    }
 
}
