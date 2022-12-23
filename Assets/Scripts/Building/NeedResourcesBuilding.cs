using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedResourcesBuilding : MonoBehaviour
{
    [SerializeField] private int[] _PricesRes = new int[Resource._ResCount];// мб отдельно дл€ нидресов слева по центру панельку?

    private ResourceType _Type;

    public Action OnResourcesUpdated;
    public Action OnResourcesComplited;

    private Dictionary<ResourceType, int> _Cost = Resource.CreateDicRes();
    public Dictionary<ResourceType, int> GetCost() { return _Cost; }
    public int[] GetResourcesNeed()
    {
        return _PricesRes;
    }
    public void SetPrice(int[] prices)
    {
        int[] values = prices;
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
        OnResourcesUpdated?.Invoke();
    }

    public void SetRes(ResourceType type, int count)
    {
        if( count <= 0) { return; }
        if (_Cost[type] - count < 0)
        {
            Debug.Log("ѕоложили слишком много ресурсов значение ушло в минус");
            _Cost[type] = 0;
        }
        else
        {
            _Cost[type] -= count;
        }
        CheckHowResourceNeed();
        OnResourcesUpdated?.Invoke();
    }
    public bool IsNeedResources() 
    {
        for (int i = 0; i < _PricesRes.Length; i++)
        {
            if (_PricesRes[i] > 0)
            {
                return true;
            }
        }
        return false;
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
        OnResourcesComplited?.Invoke();
    }
}
