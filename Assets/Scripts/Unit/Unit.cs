using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Unit : MonoBehaviour
{
    protected NavMeshAgent _Agent;
    protected Animator _Animator;

    [Header("Properites")]
    [SerializeField] private int _MaxHealth;
    [SerializeField] private int _Health;
    [SerializeField] private int _Armor;
    [SerializeField] private int _Damage;
    [SerializeField] private float _ChasingRange;
    [SerializeField] private float _AttackRange;
    [SerializeField] private int _Team = 0;
    [SerializeField] private float _AttackRate;

    [Header("Animation")]
    [SerializeField] private float _TimeToSendDamage;

    private Unit _TargetEnemy;
    private List<Unit> _TargetsEnemy;
    private float _MinDistance;
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
    public Unit GetTarget() { return _TargetEnemy; }
    public void SetTarget(Unit target) { _TargetEnemy = target; }
    protected virtual void Awake()
    {
        _Agent = GetComponent<NavMeshAgent>();
        _SoldierAI = GetComponent<SoldierAI>();
        _Animator = GetComponent<Animator>();
    }

    public void Move(Vector3 position) 
    {
        _Agent.SetDestination(position);
        _SoldierAI.SetMoveState();
    }
    public void ChasingTarget(Unit enemy) 
    {
        _TargetEnemy = enemy;
        _SoldierAI.SetChasingState();
    }
    public virtual void AttackEnemy(Unit enemy) 
    {
        enemy.TakeDamage(_Damage);
    }
    public void TakeDamage(int amount)
    {
        int damage;
        if (_Armor > 0)
        {
            damage = Mathf.RoundToInt((float)amount / _Armor);
        }
        else
        {
            damage = amount;
        }
        damage = damage > 1 ? damage : 1;
        _Health = math.max(_Health - damage, 0);

        if(_Health == 0) 
        {
            Death();
        }
    }
    public Unit FindClosestTarget() 
    {
        Unit currentTarget = null;
        _MinDistance = 1000f;
        if (_Team == 0)
        {
            _TargetsEnemy = BattleManager._Instance.GetAllEnemyUnitsList();
        }
        else
        {
            _TargetsEnemy = BattleManager._Instance.GetAllFriendlyUnitsList();
        }
        foreach (Unit target in _TargetsEnemy)
        {
            _CurrentDistance = (target.transform.position - transform.position).sqrMagnitude;
            if(_CurrentDistance < _MinDistance)
            {
                _MinDistance = _CurrentDistance;
                currentTarget = target;
            }
        }
        return currentTarget;
    }
    public Unit FindClosestTarget(float minDistance)
    {
        Unit currentTarget = null;
        _MinDistance = 1000f;
        if (_Team == 0)
        {
            _TargetsEnemy = BattleManager._Instance.GetAllEnemyUnitsList();
        }
        else
        {
            _TargetsEnemy = BattleManager._Instance.GetAllFriendlyUnitsList();
        }
        foreach (Unit target in _TargetsEnemy)
        {
            _CurrentDistance = (target.transform.position - transform.position).magnitude;
            if (_CurrentDistance < _MinDistance)
            {
                _MinDistance = _CurrentDistance;
                currentTarget = target;
            }
        }
        if (_MinDistance <= minDistance)
        {
            return currentTarget;
        }
        else
        {
            return null;
        }
    }

    private void Death()
    {
        //some die
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
