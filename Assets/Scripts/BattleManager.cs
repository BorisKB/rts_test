using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager _Instance { get; private set; }

    [SerializeField] private List<Unit> _AllFriendlyUnits = new List<Unit>();
    [SerializeField] private List<Unit> _AllEnemyUnits = new List<Unit>();

    private void Awake()
    {
        _Instance = this;
    }


    public List<Unit> GetAllFriendlyUnitsList() 
    {
        return _AllFriendlyUnits;
    }

    public void AddToAllFriendlyUnitsList(Unit unit)
    {
        if (_AllFriendlyUnits.Contains(unit))
        {
            _AllFriendlyUnits.Add(unit);
        }
        else
        {
            Debug.Log("Try Add unit then already added");
        }
    }
    public void RemoveToAllFriendlyUnitsList(Unit unit)
    {
        if (_AllFriendlyUnits.Contains(unit))
        {
            _AllFriendlyUnits.Remove(unit);
        }
        else
        {
            Debug.Log("Try Remove unit then already Deleted");
        }
    }

    public List<Unit> GetAllEnemyUnitsList()
    {
        return _AllEnemyUnits;
    }

    public void AddToAllEnemyUnitsList(Unit unit)
    {
        if (_AllEnemyUnits.Contains(unit))
        {
            _AllEnemyUnits.Add(unit);
        }
        else
        {
            Debug.Log("Try Add unit then already added");
        }
    }
    public void RemoveToAllEnemyUnitsList(Unit unit)
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
}
