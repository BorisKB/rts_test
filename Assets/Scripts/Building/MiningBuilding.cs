using System;
using System.Collections.Generic;
using UnityEngine;

public class MiningBuilding : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject _CanvasInfo;
    [SerializeField] private ResourceType _ResourceType;
    [SerializeField] private int _Count;

    public Action<int> OnResourcesUpdated;
    public ResourceType GetResourceType() { return _ResourceType; }
    public int GetResources(ResourceType type, int count)
    {
        if(type == _ResourceType)
        {
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
        _CanvasInfo.SetActive(true);
    }

    public void UnSelected()
    {
        if (_CanvasInfo != null)
        {
            _CanvasInfo.SetActive(false);
        }
    }
}
