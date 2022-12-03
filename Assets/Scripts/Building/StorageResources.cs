using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum ResourceType
{
    Stone,
    Wood,
    Steel,
    Gold
}
public class StorageResources : Building
{

    private Dictionary<ResourceType, int> _ResourceTypes = new Dictionary<ResourceType, int>()
    {
        {ResourceType.Stone, 25},
        {ResourceType.Wood, 25},
        {ResourceType.Steel, 25},
        {ResourceType.Gold, 25},
    };

    private int[] _CountResources = new int[4];
    private ResourceType _Type;
    public Action onResourcesUpdated;

    public void SetRes(ResourceType type, int count)
    {
        _ResourceTypes[type] += count;
        onResourcesUpdated?.Invoke();
        ASFD();
    }
    public bool GetRes(ResourceType needType, int needCount )
    {
        if (_ResourceTypes[needType] - needCount >= 0)
        {
            _ResourceTypes[needType] -= needCount;
            onResourcesUpdated?.Invoke();
            ASFD();
            return true;
        }
        else
        {
            Debug.Log("Not Enough Res");
            return false;
        }

    } 
    public bool GetInfoRes(ResourceType needType, int needCount)
    {
        if (_ResourceTypes[needType] - needCount >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void ASFD()
    {
        _Type = ResourceType.Stone;
        for (int i = 0; i < _CountResources.Length; i++)
        {
            _CountResources[i] = _ResourceTypes[_Type];
            _Type += 1;
        }
    }
    public int[] GetResCount()
    {
        return _CountResources;
    }
    public Dictionary<ResourceType, int> GetResourecesTypes()
    {
        return _ResourceTypes;
    }

}
