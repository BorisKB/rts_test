using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningState : StateWorker
{
    private int _CountRes;
    public Action OnMining;
    public override void StateStart()
    {
        _Timer = 0;
        _ResourceType = _WorkerAI.GetResourceType();
        _MiningBuilding = _WorkerAI.GetMiningBuilding();
        _StorageResources = _WorkerAI.GetStorageResources();

        _CountRes = _WorkerAI.GetCountRes();
        if (_ResourceType != _MiningBuilding.GetResourceType())
        {
            _ResourceType = _MiningBuilding.GetResourceType();
            if(_CountRes != 0)
            {
                _WorkerAI.SetStorageState(_StorageResources);
                return;
            }
            _WorkerAI.SetResourceType(_ResourceType);
            _CountRes = 0;
            _WorkerAI.SetCountRes(_CountRes);
        }
        _Animator.SetBool("Run", true);
    }
    public override void StateUpdate()
    {
        if (_MiningBuilding == null) 
        { 
            if(_WorkerAI.GetCountRes() > 0 && _StorageResources != null)
            {
                _WorkerAI.SetStorageState(_StorageResources);
                return;
            }
            _WorkerAI.SetIdleState(); 
            return;
        }
        _Distance = (_MiningBuilding.transform.position - _TransformUnit.position).sqrMagnitude;
        if (_Distance <= 6)
        {
            _Timer += Time.deltaTime;
            if (_Agent.hasPath)
            {
                _Animator.SetBool("Mining", true);
                _Animator.SetBool("Run", false);
                _Animator.transform.LookAt(_MiningBuilding.transform);
                _Agent.ResetPath();
            }
            if (_Timer >= _MiningRate)
            {
                OnMining?.Invoke();
                _CountRes += _MiningBuilding.GetResources(_ResourceType, _MiningPerIteration);
                _WorkerAI.SetCountRes(_CountRes);
                _Timer = 0;
            }
        }
        else
        {
            _Agent.SetDestination(_MiningBuilding.transform.position);
        }
        if (_MaxCountTaken <= _CountRes)
        {
            if (_StorageResources != null) { _WorkerAI.SetStorageState(_StorageResources); }
            else { _WorkerAI.SetIdleState(); ; }
        }
    }
    public override void StateExit()
    {
        _Animator.SetBool("Mining", false);
        _Animator.SetBool("Run", false);
    }
}
public class BuildingState : StateWorker
{
    private int[] _NeedResources;
    private int[] _ResourcesInStorage;
    public override void StateStart()
    {
        _NeedResourcesBuilding = _WorkerAI.GetNeedResourcesBuilding();
        _StorageResources = _WorkerAI.GetStorageResources();
        if (_NeedResourcesBuilding == null) { _WorkerAI.SetIdleState(); return; }
        _Animator.SetBool("Run", true);
    }
    public override void StateUpdate()
    {
        if (_NeedResourcesBuilding == null || _StorageResources == null) { _WorkerAI.SetIdleState(); return; }
        if(!_NeedResourcesBuilding.IsNeedResources()) 
        {
            _WorkerAI.SetBuildingState(null);
            return;
        }

        if(_WorkerAI.GetCountRes() == 0) 
        {
            SetNeedRes();   
            _WorkerAI.SetStorageState(_StorageResources);
            return;
        }


        _Distance = (_NeedResourcesBuilding.transform.position - _TransformUnit.position).sqrMagnitude;
        if (_Distance <= 6)
        {
            if (IsNeedCurrentRes(_WorkerAI.GetResourceType()))
            {
                SetNeedCurrentRes(_WorkerAI.GetResourceType(), _WorkerAI.GetCountRes());
                if (_WorkerAI.GetCountRes() == 0)
                {
                    SetNeedRes();
                }
                else
                {
                    _WorkerAI.SetNeedCountRes(0);
                }
                _WorkerAI.SetStorageState(_StorageResources);
                return;
            }
            else
            {
                _WorkerAI.SetStorageState(_StorageResources);
                return;
            }
        }
        else
        {
            _Agent.SetDestination(_NeedResourcesBuilding.transform.position);
        }
    }

    public override void StateExit()
    {
        _Animator.SetBool("Run", false);
    }
    private void SetNeedRes()
    {
        _NeedResources = _NeedResourcesBuilding.GetResourcesNeed();
        _ResourcesInStorage = _StorageResources.GetResCount();
        int minNeedInStorage;
        ResourceType type = ResourceType.Stone;
        for (int i = 0; i < _NeedResources.Length; i++)
        {
            if (_NeedResources[i] > 0)
            {
                if (_ResourcesInStorage[i] > 0)
                {
                    minNeedInStorage = Math.Min(_ResourcesInStorage[i], _NeedResources[i]);
                    _WorkerAI.SetResourceType(type += i);
                    _WorkerAI.SetNeedCountRes(Math.Min(minNeedInStorage, _MaxCountTaken));
                    return;
                }
            }
        }
        _WorkerAI.SetNeedCountRes(0);
        _WorkerAI.SetBuildingState(null);
    }
    private void SetNeedCurrentRes(ResourceType currentType, int count)
    {
        _NeedResources = _NeedResourcesBuilding.GetResourcesNeed();
        ResourceType type = ResourceType.Stone;
        for (int i = 0; i < _NeedResources.Length; i++)
        {
            if (currentType == type + i)
            {
                if (_NeedResources[i] > 0)
                {
                    if (_NeedResources[i] >= count)
                    {
                        _NeedResourcesBuilding.SetRes(currentType, count);
                        _WorkerAI.SetCountRes(0);
                    }
                    else 
                    {
                        _NeedResourcesBuilding.SetRes(currentType, _NeedResources[i]);
                        _WorkerAI.SetCountRes(count - _NeedResources[i]);
                    }
                }
            }
        }
    }
    private bool IsNeedCurrentRes(ResourceType currentType)
    {
        _NeedResources = _NeedResourcesBuilding.GetResourcesNeed();
        ResourceType type = ResourceType.Stone;
        for (int i = 0; i < _NeedResources.Length; i++)
        {
            if(currentType == type + i)
            {
                if (_NeedResources[i] > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
public class StorageState : StateWorker
{
    private int _CountRes;
    public override void StateStart()
    {
        _ResourceType = _WorkerAI.GetResourceType();
        _StorageResources = _WorkerAI.GetStorageResources();
        _NeedResourcesBuilding = _WorkerAI.GetNeedResourcesBuilding();
        _MiningBuilding = _WorkerAI.GetMiningBuilding();
        _CountRes = _WorkerAI.GetCountRes();
        _Animator.SetBool("Run", true);
    }
    public override void StateUpdate()
    {
        if (_StorageResources == null) { _WorkerAI.SetIdleState(); return; }

        _Distance = (_StorageResources.transform.position - _TransformUnit.position).sqrMagnitude;
        if (_Distance <= 6)
        {
            if (_CountRes == 0)
            {
                GetResource();
                return;
            }
            else
            {
                SetResource();
                return;
            }
        }
        else
        {
            _Agent.SetDestination(_StorageResources.transform.position);
        }
    }
    public override void StateExit()
    {
        _Animator.SetBool("Run", false);
    }
    private void SetResource()
    {
        _StorageResources.SetRes(_ResourceType, _CountRes);
        _CountRes = 0;
        _WorkerAI.SetCountRes(_CountRes);
        if (_NeedResourcesBuilding != null) { _WorkerAI.SetBuildingState(_NeedResourcesBuilding); }
        else if (_MiningBuilding != null) { _WorkerAI.SetMiningState(_MiningBuilding); }
        else { _WorkerAI.SetIdleState(); }
    }
    private void GetResource()
    {
        if (_NeedResourcesBuilding != null)
        {
            if (_StorageResources.GetRes(_WorkerAI.GetResourceType(), _WorkerAI.GetNeedCountRes()))
            {
                _WorkerAI.SetCountRes(_WorkerAI.GetNeedCountRes());
                _WorkerAI.SetBuildingState(_NeedResourcesBuilding);
                return;
            }
            else
            {
                _WorkerAI.SetBuildingState(_NeedResourcesBuilding);
                return;
            }
        }
        if (_MiningBuilding != null) { _WorkerAI.SetMiningState(_MiningBuilding); }
        else { _WorkerAI.SetIdleState(); }
    }
}
public class StateWorker : State
{
    protected Worker _Worker;
    protected int _MiningPerIteration;
    protected float _MiningRate;
    protected ResourceType _ResourceType;
    protected int _MaxCountTaken;
    protected WorkerAI _WorkerAI;
    protected StorageResources _StorageResources;
    protected MiningBuilding _MiningBuilding;
    protected NeedResourcesBuilding _NeedResourcesBuilding;

    public override void Init(Unit unit, Transform transform)
    {
        _Unit = unit;
        _Worker = unit.GetComponent<Worker>();
        _TransformUnit = transform;
        _WorkerAI = _Worker.GetWorkerAI();
        _MiningRate = _Worker.GetMiningRate();
        _MiningPerIteration = _Worker.GetMiningPerIteration();
        _MaxCountTaken = _Worker.GetMaxCountTaken();
        _Animator = _Unit.GetAnimator();
        _Agent = _Unit.GetAgent();
        _SoldierAI = _Unit.GetSoldierAI();
        _ChasingRange = _Unit.GetChasingRange();
        _AttackRangeSqr = _Unit.GetAttackRange();
        _AttackRate = _Unit.GetAttackRate();
        _AttackRangeSqr = _AttackRangeSqr * _AttackRangeSqr;
        _ChasingRange = _ChasingRange * _ChasingRange;
    }
}

   
