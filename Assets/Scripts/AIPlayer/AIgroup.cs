using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Mission
{
    Stay,
    Attack
}
public class AIgroup : MonoBehaviour
{
    [SerializeField] private List<EnemyUnit> _GroupUnits = new List<EnemyUnit>();

    public int limitGroup = 5;
    public bool isGroupReady = false;
    public bool isFull = false;
    public Mission currentMission;
    public Action<AIgroup> OnMissionComplited;

    private float _Timer = 0;
    private Vector3 _MissionPosition;

    private void Start()
    {
        OnMissionComplited?.Invoke(this);
    }

    private void Update()
    {
        if(currentMission == Mission.Stay)
        {
            _Timer += Time.deltaTime;
            if(_Timer > 10)
            {
                for (int i = 0; i < _GroupUnits.Count; i++)
                {
                    if ((_MissionPosition - _GroupUnits[i].transform.position).magnitude > 10)
                    {
                        isGroupReady = false;
                        break;
                    }
                    isGroupReady = true;
                }
                OnMissionComplited?.Invoke(this);
            }
        }
        if(currentMission == Mission.Attack)
        {
            _Timer += Time.deltaTime;
            if(_Timer > 120)
            {
                AttackGroup(_MissionPosition);
                _Timer = 0;
            }
        }
    }
    private void OnUnitDied(EnemyUnit unit)
    {
        unit.OnDied -= OnUnitDied;
        _GroupUnits.Remove(unit);
        if(_GroupUnits.Count == 0)
        {
            isGroupReady = false;
            isFull = false;
            OnMissionComplited?.Invoke(this);
        }
    }
    private void AttackGroup(Vector3 pos)
    {
        for (int i = 0; i < _GroupUnits.Count; i++)
        {
            _GroupUnits[i].MoveCheck(pos);
        }
    }
    private void MoveGroup(Vector3 pos)
    {
        for (int i = 0; i < _GroupUnits.Count; i++)
        {
            _GroupUnits[i].Move(pos);
        }
    }
    public void SetUnitInList(EnemyUnit unit)
    {
        _GroupUnits.Add(unit);
        unit.OnDied += OnUnitDied;
        if(_GroupUnits.Count == limitGroup)
        {
            isFull = true;
            OnMissionComplited?.Invoke(this);
        }
        StartCoroutine(DelayForInit(unit));
    }
    private IEnumerator DelayForInit(EnemyUnit unit)
    {
        yield return new WaitForSeconds(0.15f);
        unit.Move(_MissionPosition);
    }
    public void SetMission(Mission mission, Vector3 position)
    {
        _MissionPosition = position;
        switch (mission)
        {
            case Mission.Stay:
                _Timer = 0;
                if (_GroupUnits.Count == 0) { break; }
                break;
            case Mission.Attack:
                _Timer = 0;
                //AttackGroup(_MissionPosition);
                break;
            default:
                break;
        }
        currentMission = mission;
    }
}
