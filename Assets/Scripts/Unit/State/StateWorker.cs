using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningState : StateWorker
{
    private int _CountRes;
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
    }
    public override void StateUpdate()
    {
        if (_MiningBuilding == null) { _WorkerAI.SetIdleState(); return; }
        _Distance = (_MiningBuilding.transform.position - _TransformUnit.position).sqrMagnitude;
        if (_Distance <= 6)
        {
            _Timer += Time.deltaTime;
            if (_Timer >= _MiningRate)
            {
                _CountRes += _MiningBuilding.GetResources(_ResourceType, 1) ? 1 : 0;
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
            Debug.Log(_StorageResources);
            if (_StorageResources != null) { _WorkerAI.SetStorageState(_StorageResources); }
            else { _WorkerAI.SetIdleState(); ; }
        }
    }
}
public class BuildingState : StateWorker
{
    private int[] _NeedResources;
    private int[] _ResourcesInStorage;
    public override void StateStart()
    {
        _ConstructionBuilding = _WorkerAI.GetConstructionBuilding();
        _StorageResources = _WorkerAI.GetStorageResources();
        if (_ConstructionBuilding == null) { _WorkerAI.SetIdleState(); return; }
    }
    public override void StateUpdate()
    {
        if (_ConstructionBuilding == null) { _WorkerAI.SetIdleState(); return; }

        if(_WorkerAI.GetCountRes() == 0) 
        {
            SetNeedRes();   
            _WorkerAI.SetStorageState(_StorageResources);
            return;
        }


        _Distance = (_ConstructionBuilding.transform.position - _TransformUnit.position).sqrMagnitude;
        if (_Distance <= 6)
        {
            if (isNeedCurrentRes(_WorkerAI.GetResourceType()))
            {
                SetNeedCurrentRes(_WorkerAI.GetResourceType(), _WorkerAI.GetCountRes());
                SetNeedRes();
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
            _Agent.SetDestination(_ConstructionBuilding.transform.position);
        }
    }

    private void SetNeedRes()
    {
        _NeedResources = _ConstructionBuilding.GetResourcesNeed();
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
                    break;
                }
            }
        }
    }
    private void SetNeedCurrentRes(ResourceType currentType, int count)
    {
        _NeedResources = _ConstructionBuilding.GetResourcesNeed();
        ResourceType type = ResourceType.Stone;
        for (int i = 0; i < _NeedResources.Length; i++)
        {
            if (currentType == type + i)
            {
                if (_NeedResources[i] > 0)
                {
                    if (_NeedResources[i] >= count)
                    {
                        _ConstructionBuilding.SetRes(currentType, count);
                        _WorkerAI.SetCountRes(0);
                    }
                    else 
                    {
                        _ConstructionBuilding.SetRes(currentType, _NeedResources[i]);
                        _WorkerAI.SetCountRes(count - _NeedResources[i]);
                    }
                }
            }
        }
    }
    private bool isNeedCurrentRes(ResourceType currentType)
    {
        _NeedResources = _ConstructionBuilding.GetResourcesNeed();
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
        _ConstructionBuilding = _WorkerAI.GetConstructionBuilding();
        _MiningBuilding = _WorkerAI.GetMiningBuilding();
        _CountRes = _WorkerAI.GetCountRes();
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
    private void SetResource()//213
    {
        _StorageResources.SetRes(_ResourceType, _CountRes);
        _CountRes = 0;
        _WorkerAI.SetCountRes(_CountRes);
        if (_ConstructionBuilding != null) { _WorkerAI.SetBuildingState(_ConstructionBuilding); }
        else if (_MiningBuilding != null) { _WorkerAI.SetMiningState(_MiningBuilding); }
        else { _WorkerAI.SetIdleState(); }
    }
    private void GetResource()
    {
        if (_ConstructionBuilding != null)
        {
            if (_StorageResources.GetRes(_WorkerAI.GetResourceType(), _WorkerAI.GetNeedCountRes()))
            {
                _WorkerAI.SetCountRes(_WorkerAI.GetNeedCountRes());
                _WorkerAI.SetBuildingState(_ConstructionBuilding);
                return;
            }
            else
            {
                _WorkerAI.SetBuildingState(_ConstructionBuilding);
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
    protected float _MiningRate;
    protected ResourceType _ResourceType;
    protected int _MaxCountTaken;
    protected WorkerAI _WorkerAI;
    protected StorageResources _StorageResources;
    protected MiningBuilding _MiningBuilding;
    protected ConstructionBuilding _ConstructionBuilding;

    public void Init(Unit unit ,Worker worker, Transform transform)
    {
        _Unit = unit;
        _Worker = worker;
        _TransformUnit = transform;
        _WorkerAI = worker.GetWorkerAI();
        _MiningRate = _Worker.GetMiningRate();
        _MaxCountTaken = _Worker.GetMaxCountTaken();
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
}

   
