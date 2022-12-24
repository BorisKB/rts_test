using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnUnitButton : MonoBehaviour
{
    [SerializeField] private int[] _Price;
    [SerializeField] private DamagableObject _UnitPrefab;
    [SerializeField] private Sprite _UnitSprite;

    public Action<DamagableObject, int[], Sprite> OnOrderUnit;
    public void OrderUnit()
    {
        OnOrderUnit?.Invoke(_UnitPrefab, _Price, _UnitSprite);
    }
}
