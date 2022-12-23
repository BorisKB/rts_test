using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitButton : MonoBehaviour
{
    [SerializeField] private int[] _Price;
    [SerializeField] private DamagableObject _UnitPrefab;

    public Action<DamagableObject, int[]> OnOrderUnit;
    public void OrderUnit()
    {
        OnOrderUnit?.Invoke(_UnitPrefab, _Price);
    }
}
