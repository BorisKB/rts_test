using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionBuilding : Building
{
    [SerializeField] private Renderer _BuildingRenderer;
    [SerializeField] private Building _BuildingPrefab;
    [SerializeField] private LayerMask _BuildingBlockLayer;
    [SerializeField]private int[] _PricesRes;
    
    private BoxCollider _BuildingCollider;
    private ResourceType _Type;
    public Action onResourcesUpdated;

    private Dictionary<ResourceType, int> _Cost = new Dictionary<ResourceType, int>()
    {
        {ResourceType.Stone, 0},
        {ResourceType.Wood, 0},
        {ResourceType.Steel, 0},
        {ResourceType.Gold, 0},
    };
    public Dictionary<ResourceType, int> GetCost() { return _Cost; }
    public int[] GetResourcesNeed()
    {
        return _PricesRes;
    }
    public Renderer GetBuildingRenderer() { return _BuildingRenderer; }
    private void Awake()
    {
        _BuildingCollider = GetComponent<BoxCollider>();
        int[] values = _BuildingPrefab.GetPrice();
        for (int i = 0; i < values.Length; i++)
        {
            _PricesRes[i] = values[i];
        }
        _Type = ResourceType.Stone;
        for (int i = 0; i < _PricesRes.Length; i++)
        {
            _Cost[_Type] = _PricesRes[i];
            _Type += 1;
        }
        onResourcesUpdated?.Invoke();
    }

    public bool CanPlaceBuilding(Vector3 point)
    {
        if (Physics.CheckBox(point + _BuildingCollider.center, _BuildingCollider.size / 2, Quaternion.identity, _BuildingBlockLayer))
        {
            return false;
        }
        return true;
    }
    public void SetRes(ResourceType type, int count)
    {
        if (_Cost[type] - count < 0)
        {
            Debug.Log("Положили слишком много ресурсов значение ушло в минус");
            _Cost[type] = 0;
        }
        else
        {
            _Cost[type] -= count;
        }
        CheckHowResourceNeed();
        onResourcesUpdated?.Invoke();
    }
    private void CheckHowResourceNeed()
    {
        _Type = ResourceType.Stone;
        for (int i = 0; i < _PricesRes.Length; i++)
        {
            _PricesRes[i] = _Cost[_Type];
            _Type += 1;
        }
        for (int i = 0; i < _PricesRes.Length; i++)
        {
            if (_PricesRes[i] > 0)
            {
                return;
            }
        }
        SetBuilding();
    }

    private void SetBuilding() 
    {
        Instantiate(_BuildingPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
