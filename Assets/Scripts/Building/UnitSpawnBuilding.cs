using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnBuilding : Building
{
    [SerializeField] SpawnUnitButton[] _Buttons;
    [SerializeField] NeedResourcesBuilding _NeedResourcesBuilding;
    [SerializeField] private Transform _PositionSpawn;

    private DamagableObject _CurrentUnit;
    void Start()
    {
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

    private void SetOrderUnit(DamagableObject unit, int[] price)
    {
        if (_CurrentUnit == null)
        {
            _CurrentUnit = unit;
            _NeedResourcesBuilding.SetPrice(price);
        }
    }
    private void SpawnUnit()
    {
        DamagableObject unit = Instantiate(_CurrentUnit , _PositionSpawn.position, Quaternion.identity);
        BattleManager._Instance.AddToAllFriendlyObjectsList(unit);
        _CurrentUnit = null;
    }
}
