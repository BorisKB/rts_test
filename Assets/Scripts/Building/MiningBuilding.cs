using System;
using System.Collections.Generic;
using UnityEngine;

public class MiningBuilding : Building
{
    [SerializeField] private ResourceType _ResourceType;
    [SerializeField] private int _Count;

    public Action<int> OnResourcesUpdated;
    public ResourceType GetResourceType() { return _ResourceType; }
    public bool GetResources(ResourceType type, int count)
    {
        if(type == _ResourceType)
        {
            if (_Count <= 0) 
            {
                DestroyBuilding();
                return false;
            };
            _Count -= count;
            OnResourcesUpdated?.Invoke(_Count);
            return _Count >= 0 ? true : false ;
        }
        return false;
    }
    private void DestroyBuilding()
    {
        UnSelected();
        Destroy(gameObject, 0.1f);
    }
}
