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
            _SoldierAI.SetChasingState();
        }
    }
}
public class MoveState : State
{
    public override void StateStart()
    {
        _Animator.SetBool("Move", true);
    }
    public override void StateUpdate()
    {
        if (_Agent.hasPath == false)
        {
            _Animator.SetBool("Move", false);
            _SoldierAI.SetIdleState();
        }
    }
}
public class ChasingState : State
{
    public override void StateStart()
    {
        _CurrentTarget = _Unit.GetTarget();
        _Agent.stoppingDistance = _AttackRange - 0.5f;
        _Animator.SetBool("Chasing", true);
    }
    public override void StateUpdate()
    {
        if (_CurrentTarget == null) { _Animator.SetBool("Chasing", false); }

        _Distance = (_CurrentTarget.transform.position - _TransformUnit.position).sqrMagnitude;
        if (_Distance < _ChasingRange)
        {
            if (_Distance <= _AttackRange)
            {
                _Animator.SetBool("Chasing", false);
                _SoldierAI.SetAttackState();
            }
            else
            {
                _Agent.SetDestination(_CurrentTarget.transform.position);
            }
        }
        else
        {
            _Animator.SetBool("Chasing", false);
            _SoldierAI.SetIdleState();
        }
    }
}
public class AttackState : State
{
    public override void StateStart()
    {
        _Timer = 0f;
        _Agent.ResetPath();
        _CurrentTarget = _Unit.GetTarget();
        _IsAttack = false;
        _Animator.SetTrigger("Attack1");
    }

    public override void StateUpdate()
    {
        _Timer += Time.deltaTime;
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
}
public class State
{
    protected Unit _Unit;
    protected NavMeshAgent _Agent;
    protected Animator _Animator;
    protected Transform _TransformUnit;
    protected Unit _CurrentTarget;
    protected float _ChasingRange;
    protected float _AttackRange;

    protected float _TimeToSendDamage;
    protected float _AttackRate;
    protected bool _IsAttack = false;

    protected float _Timer;
    protected float _Distance;
    protected SoldierAI _SoldierAI;

    public void Init(Unit unit, Transform transform)
    {
        _Unit = unit;
        _TransformUnit = transform;
        _Animator = _Unit.GetAnimator();
        _Agent = _Unit.GetAgent();
        _SoldierAI = _Unit.GetSoldierAI();
        _ChasingRange = _Unit.GetChasingRange();
        _AttackRange = _Unit.GetAttackRange();
        _TimeToSendDamage = _Unit.GetTimeToSendDamage();
        _AttackRate = _Unit.GetAttackRate();
        _AttackRange = _AttackRange * _AttackRange;
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
 
}
