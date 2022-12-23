using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Worker : PlayerUnit
{
    [Header("Worker Properities")]
    [SerializeField] private float _MiningRate;
    [SerializeField] private int _MiningPerIteration;
    [SerializeField] private int _MaxCountTaken;

    private WorkerAI _WorkerAI;

    public int GetMaxCountTaken() { return _MaxCountTaken; }
    public int GetMiningPerIteration() { return _MiningPerIteration; }
    public float GetMiningRate() { return _MiningRate; }
    public WorkerAI GetWorkerAI() { return _WorkerAI; }
    protected override void Awake()
    {
        base.Awake();
        _WorkerAI = _SoldierAI.GetComponent<WorkerAI>();
    }

    public void SetTraitWorker(TraitWorker trait)
    {
        if (isAlreadyHaveTrait) { return; }
        SetTrait(trait);
        _MiningPerIteration += trait.MiningPerIteration;
        _MaxCountTaken += trait.MaxCountTake;
        _WorkerAI.Initialization();
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

    private void GoBuilding(NeedResourcesBuilding building)
    {
        _WorkerAI.SetBuildingState(building);
    }
}
