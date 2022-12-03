using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class WorkerAI : SoldierAI
{
    private Worker _Worker;
    private StorageResources _StorageResources;
    private MiningBuilding _MiningBuilding;
    private ConstructionBuilding _ConstructionBuilding;
    [SerializeField] private ResourceType _ResourceType = 0;
    protected StateWorker _MiningState = new MiningState();
    protected StateWorker _BuildingState = new BuildingState();
    protected StateWorker _StorageState = new StorageState();

    private int _Count = 0;
    private int _NeedCount = 0;

    public int GetCountRes() { return _Count; }
    public void SetCountRes(int value) { _Count = value; }
    public int GetNeedCountRes() { return _NeedCount; }
    public void SetNeedCountRes(int value) { _NeedCount = value; }
    public ResourceType GetResourceType() { return _ResourceType; }
    public void SetResourceType(ResourceType type) { _ResourceType = type; }
    public StorageResources GetStorageResources() { return _StorageResources; }
    public MiningBuilding GetMiningBuilding() { return _MiningBuilding; }
    public ConstructionBuilding GetConstructionBuilding() { return _ConstructionBuilding; }
    protected override void Start()
    {
        base.Start();
        _Worker = GetComponent<Worker>();
        _MiningState.Init(_Unit, _Worker, transform);
        _BuildingState.Init(_Unit, _Worker, transform);
        _StorageState.Init(_Unit, _Worker, transform);
    }
    public void SetMiningState(MiningBuilding miningBuilding)
    {
        _MiningBuilding = miningBuilding;
        SetState(_MiningState);
    }
    public void SetBuildingState(ConstructionBuilding constructionBuilding)
    {
        _ConstructionBuilding = constructionBuilding;
        SetState(_BuildingState);
    }
    public void SetStorageState(StorageResources storageResources)
    {
        _StorageResources = storageResources;
        SetState(_StorageState);
    }
}
