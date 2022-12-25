using System;
using System.Collections.Generic;
using UnityEngine;

public class MiningBuilding : MonoBehaviour, ISelectable
{
    [SerializeField] private ResourceType _ResourceType;
    [SerializeField] private int _Count;

    [SerializeField] private ParticleFx _ParticleFxPrefab;
    [SerializeField] private Transform _ParticleFxPosition;
    public Action<int> OnResourcesUpdated;
    public ResourceType GetResourceType() { return _ResourceType; }
    public int GetResources(ResourceType type, int count)
    {
        if(type == _ResourceType)
        {
            Instantiate(_ParticleFxPrefab, _ParticleFxPosition.position, Quaternion.identity);
            if (_Count - count <= 0) 
            {
                DestroyBuilding();
                return _Count;
            };
            _Count -= count;
            OnResourcesUpdated?.Invoke(_Count);
            return count;
        }
        return 0;
    }

    private void DestroyBuilding()
    {
        Destroy(gameObject);
    }

    public void Selected()
    {
    }

    public void UnSelected()
    {
    }
}
