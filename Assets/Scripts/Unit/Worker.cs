using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : PlayerUnit
{
    [Header("Worker Properities")]
    [SerializeField] private float _MiningRate;
    [SerializeField] private int _MaxCountTaken;

    private WorkerAI _WorkerAI;

    public int GetMaxCountTaken() { return _MaxCountTaken; }
    public float GetMiningRate() { return _MiningRate; }
    public WorkerAI GetWorkerAI() { return _WorkerAI; }
    protected override void Awake()
    {
        base.Awake();
        _WorkerAI = _SoldierAI.GetComponent<WorkerAI>();
    }
    public override void Selected()
    {
        base.Selected();
        Commander.GoStorage += GoStorage;
        Commander.Mining += GoMining;
        Commander.GoBuilding += GoBuilding;
    }
    public override void UnSelected()
    {
        base.UnSelected();
        Commander.GoStorage -= GoStorage;
        Commander.Mining -= GoMining;
        Commander.GoBuilding -= GoBuilding;
    }

    private void GoMining(MiningBuilding building)
    {
        _WorkerAI.SetMiningState(building);
    }

    private void GoStorage(StorageResources building)
    {
        _WorkerAI.SetStorageState(building);
    }

    private void GoBuilding(ConstructionBuilding building)
    {
        _WorkerAI.SetBuildingState(building);
    }
}
