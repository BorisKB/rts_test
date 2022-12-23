using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionBuilding : Building
{
    [SerializeField] private Renderer _BuildingRenderer;
    [SerializeField] private Building _BuildingPrefab;
    [SerializeField] private LayerMask _BuildingBlockLayer;
    [SerializeField] private NeedResourcesBuilding _NeedsResourcesBuilding;
    [SerializeField]private int[] _PricesRes;
    
    private BoxCollider _BuildingCollider;
    public Action onResourcesUpdated;

    public Dictionary<ResourceType, int> GetCost() { return _NeedsResourcesBuilding.GetCost(); }
    public Renderer GetBuildingRenderer() { return _BuildingRenderer; }
    private void Awake()
    {
        _BuildingCollider = GetComponent<BoxCollider>();
        _NeedsResourcesBuilding = GetComponent<NeedResourcesBuilding>();
        _NeedsResourcesBuilding.SetPrice(_BuildingPrefab.GetPrice());
        _NeedsResourcesBuilding.OnResourcesComplited += SetBuilding;
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
    private void SetBuilding() 
    {
        Instantiate(_BuildingPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        _NeedsResourcesBuilding.OnResourcesComplited -= SetBuilding;
    }

}
