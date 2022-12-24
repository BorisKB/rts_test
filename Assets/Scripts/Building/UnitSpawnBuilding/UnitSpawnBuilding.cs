using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSpawnBuilding : Building
{
    [SerializeField] SpawnUnitButton[] _Buttons;
    [SerializeField] NeedResourcesBuilding _NeedResourcesBuilding;
    [SerializeField] private Transform _PositionSpawn;
    [SerializeField] private Transform _QueueObject;
    [SerializeField] private int _QueueCount = 0;

    [SerializeField] private List<Queue> _QueueList = new List<Queue>();

    private SpawnBuildingQueueUI[] spawnBuildingQueueUI;
    private Queue _QueueForSort;
    private DamagableObject _CurrentUnit;
    void Start()
    {
        spawnBuildingQueueUI = _QueueObject.GetComponentsInChildren<SpawnBuildingQueueUI>();
        for (int i = 0; i < _QueueCount; i++)
        {
            _QueueList.Add(new Queue());
        }
        _NeedResourcesBuilding.OnResourcesComplited += SpawnUnit;
        foreach (var button in _Buttons)
        {
            button.OnOrderUnit += SetOrderUnit;
        }
    }
    private void OnDestroy()
    {
        _NeedResourcesBuilding.OnResourcesComplited -= SpawnUnit;
        foreach (var button in _Buttons)
        {
            button.OnOrderUnit -= SetOrderUnit;
        }
    }

    private void SetOrderUnit(DamagableObject unit, int[] price, Sprite unitSprite)
    {
        for (int i = 0; i < _QueueCount; i++)
        {
            if (_QueueList[i].isEmpty)
            {
                _QueueList[i].Setup(unit, price, unitSprite);
                spawnBuildingQueueUI[i].SetSprite(unitSprite);
                break;
            }
        }
        if (_CurrentUnit == null)
        {
            SetCurrentQueue();
        }
    }
    private void SetCurrentQueue()
    {
        if (!_QueueList[0].isEmpty)
        {
            _CurrentUnit = _QueueList[0].GetUnit();
            _NeedResourcesBuilding.SetPrice(_QueueList[0].GetPrice());
        }
    }
    private void SortQueue()
    {
        _QueueForSort = _QueueList[0];
        _QueueForSort.ClearQueue();
        _QueueList.Remove(_QueueForSort);
        _QueueList.Add(_QueueForSort);
        for (int i = 0; i < _QueueCount; i++)
        {
            spawnBuildingQueueUI[i].SetSprite(_QueueList[i].GetSprite());
        }
    }
    private void SpawnUnit()
    {
        DamagableObject unit = Instantiate(_CurrentUnit , _PositionSpawn.position, Quaternion.identity);
        BattleManager._Instance.AddToAllFriendlyObjectsList(unit);
        _CurrentUnit = null;
        SortQueue();
        SetCurrentQueue();
    }
}

public class Queue
{
    private DamagableObject _DamagableObject;
    private int[] _Price = new int[Resource._ResCount];
    public bool isEmpty = true;

    private Sprite sprite;
    public void Setup(DamagableObject damagableObject, int[] price, Sprite unitSprite)
    {
        this._DamagableObject = damagableObject;
        for (int i = 0; i < price.Length; i++)
        {
            _Price[i] = price[i];
        }
        isEmpty = false;
        sprite = unitSprite;
    }
    public Sprite GetSprite() { return sprite; }
    public void ClearQueue()
    {
        isEmpty = true;
        sprite = null;
    }
    public DamagableObject GetUnit() { return _DamagableObject; }
    public int[] GetPrice() { return _Price; }
}
