using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class WorkerAI : SoldierAI
{
    private Worker _Worker;
    private StorageResources _StorageResources;
    private MiningBuilding _MiningBuilding;
    private NeedResourcesBuilding _NeedResourcesBuilding;
    [SerializeField] private ResourceType _ResourceType = 0;
    protected MiningState _MiningState = new MiningState();
    protected BuildingState _BuildingState = new BuildingState();
    protected StorageState _StorageState = new StorageState();

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
    public NeedResourcesBuilding GetNeedResourcesBuilding() { return _NeedResourcesBuilding; }
    protected override void Start()
    {
        base.Start();
        _Worker = GetComponent<Worker>();
        _MiningState.OnMining += CheckTrait;
    }
    public override void Initialization()
    {
        base.Initialization();
        _MiningState.Init(_Unit, transform);
        _BuildingState.Init(_Unit, transform);
        _StorageState.Init(_Unit, transform);
    }
    private void OnDestroy()
    {
        _MiningState.OnMining -= CheckTrait;
    }

    private void CheckTrait()
    {
        if(Random.Range(0, 100) == 1)
        {
            Debug.Log("GetWorkerTrait");
            _Worker.SetTraitWorker(TraitsContainer._Instance.WorkerTraits[Random.Range(0, TraitsContainer._Instance.WorkerTraits.Count)]);
        }
    }
    public void SetMiningState(MiningBuilding miningBuilding)
    {
        _MiningBuilding = miningBuilding;
        SetState(_MiningState);
    }
    public void SetBuildingState(NeedResourcesBuilding building)
    {
        _NeedResourcesBuilding = building;
        SetState(_BuildingState);
    }
    public void SetStorageState(StorageResources storageResources)
    {
        _StorageResources = storageResources;
        SetState(_StorageState);
    }
}
