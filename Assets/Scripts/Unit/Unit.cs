using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(DamagableObject))]
public class Unit : MonoBehaviour
{
    protected NavMeshAgent _Agent;
    protected Animator _Animator;
    protected DamagableObject _DamagableObject;

    [Header("Properites")]
    [SerializeField] protected int _Damage;
    [SerializeField] protected float _Speed;
    [SerializeField] private float _ChasingRange;
    [SerializeField] private float _AttackRange;
    [SerializeField] private float _AttackRate;
    [SerializeField] private int _Team = 0;

    [Header("Animation")]
    [SerializeField] private float _TimeToSendDamage;

    private List<DamagableObject> _TargetsEnemy;
    private float _MinDistance;
    private DamagableObject _CurrentTarget;
    private float _CurrentDistance;
    protected SoldierAI _SoldierAI;


    public NavMeshAgent GetAgent() { return _Agent; }
    public Animator GetAnimator() { return _Animator; }
    public SoldierAI GetSoldierAI() { return _SoldierAI; }
    public int GetTeam() { return _Team; }
    public float GetChasingRange() { return _ChasingRange; }
    public float GetAttackRange() { return _AttackRange; }
    public float GetAttackRate() { return _AttackRate; }
    public float GetTimeToSendDamage() { return _TimeToSendDamage; }

    protected virtual void Awake()
    {
        _Agent = GetComponent<NavMeshAgent>();
        _Agent.speed = _Speed;
        _SoldierAI = GetComponent<SoldierAI>();
        _Animator = GetComponent<Animator>();
        _DamagableObject = GetComponent<DamagableObject>();
        _DamagableObject.OnDied += Death;
        _DamagableObject.SetTeam(_Team);
    }

    private void Update()
    {
        _Animator.SetFloat("Locomotion", _Agent.velocity.magnitude);
    }
    public void Move(Vector3 position) 
    {
        _Agent.SetDestination(position);
        _SoldierAI.SetMoveState();
    }
    public void ChasingTarget(DamagableObject enemy) 
    {
        _SoldierAI.SetTarget(enemy);
        _SoldierAI.SetChasingCurrentTargetState();
    }
    public virtual void AttackEnemy(DamagableObject enemy) 
    {
        enemy.TakeDamage(_Damage);
    }
    public DamagableObject FindClosestTarget() 
    {
        _CurrentTarget = null;
        _MinDistance = 1000f;
        if (_Team == 0)
        {
            _TargetsEnemy = BattleManager._Instance.GetAllEnemyUnitsList();
        }
        else
        {
            _TargetsEnemy = BattleManager._Instance.GetAllFriendlyDamagableObjectsList();
        }
        foreach (DamagableObject target in _TargetsEnemy)
        {
            _CurrentDistance = (target.transform.position - transform.position).sqrMagnitude;
            if(_CurrentDistance < _MinDistance)
            {
                _MinDistance = _CurrentDistance;
                _CurrentTarget = target;
            }
        }
        return _CurrentTarget;
    }
    public DamagableObject FindClosestTarget(float minDistance)
    {
        _CurrentTarget = null;
        _MinDistance = 1000f;
        if (_Team == 0)
        {
            _TargetsEnemy = BattleManager._Instance.GetAllEnemyUnitsList();
        }
        else
        {
            _TargetsEnemy = BattleManager._Instance.GetAllFriendlyDamagableObjectsList();
        }
        foreach (DamagableObject target in _TargetsEnemy)
        {
            _CurrentDistance = (target.transform.position - transform.position).sqrMagnitude;
            if (_CurrentDistance < _MinDistance)
            {
                _MinDistance = _CurrentDistance;
                _CurrentTarget = target;
            }
        }
        if (_MinDistance <= minDistance)
        {
            return _CurrentTarget;
        }
        else
        {
            return null;
        }
    }

    public virtual void Death()
    {
        _DamagableObject.OnDied -= Death;
    }
   
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _AttackRange);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, _ChasingRange);
    }
#endif  
}
