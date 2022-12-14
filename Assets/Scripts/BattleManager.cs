using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager _Instance { get; private set; }

    [SerializeField] private List<DamagableObject> _AllFriendlyDamagableObjects = new List<DamagableObject>();
    [SerializeField] private List<DamagableObject> _AllEnemyUnits = new List<DamagableObject>();

    public List<Building> _Buildings = new List<Building>();

    public Action<ISelectable, DamagableObject> OnSelectableDestroyed;

    private void Awake()
    {
        _Instance = this;
    }

    #region AllFriendlyDamagableObjects
    public List<DamagableObject> GetAllFriendlyDamagableObjectsList() 
    {
        return _AllFriendlyDamagableObjects;
    }

    public void AddToAllFriendlyObjectsList(DamagableObject damagableObject)
    {
        if (!_AllFriendlyDamagableObjects.Contains(damagableObject))
        {
            _AllFriendlyDamagableObjects.Add(damagableObject);
            if(damagableObject.CompareTag("Building"))
            {
                _Buildings.Add(damagableObject.GetComponent<Building>());
            }
        }
        else
        {
            Debug.Log("Try Add unit then already added");
        }
    }
    public void RemoveToAllFriendlyDamagableList(DamagableObject damagableObject)
    {
        if (_AllFriendlyDamagableObjects.Contains(damagableObject))
        {
            OnSelectableDestroyed?.Invoke(damagableObject.GetComponent<ISelectable>(), damagableObject);
            _AllFriendlyDamagableObjects.Remove(damagableObject);
            if (damagableObject.CompareTag("Building"))
            {
                _Buildings.Remove(damagableObject.GetComponent<Building>());
            }
        }
        else
        {
            Debug.Log("Try Remove unit then already Deleted");
        }
    }
    #endregion
    #region AllEnemyUnits
    public List<DamagableObject> GetAllEnemyUnitsList()
    {
        return _AllEnemyUnits;
    }

    public void AddToAllEnemyUnitsList(DamagableObject unit)
    {
        if (!_AllEnemyUnits.Contains(unit))
        {
            _AllEnemyUnits.Add(unit);
        }
        else
        {
            Debug.Log("Try Add unit then already added");
        }
    }
    public void RemoveToAllEnemyUnitsList(DamagableObject unit)
    {
        if (_AllEnemyUnits.Contains(unit))
        {
            _AllEnemyUnits.Remove(unit);
        }
        else
        {
            Debug.Log("Try Remove unit then already Deleted");
        }
    }
    #endregion
}
