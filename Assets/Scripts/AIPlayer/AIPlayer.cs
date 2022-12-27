using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    [SerializeField] private AIgroup[] _AIgroups;
    [SerializeField] private Transform _PlaceToStay;

    private Vector3 _MissionPosition;
    private Mission _Mission;

    private SpawnEnemyUnits _SpawnEnemyUnits;

    private void Awake()
    {
        _SpawnEnemyUnits = GetComponent<SpawnEnemyUnits>();
        _SpawnEnemyUnits.OnEnemyUnitSpawned += SetUnitInGroup;
        for (int i = 0; i < _AIgroups.Length; i++)
        {
            _AIgroups[i].OnMissionComplited += SetMissionForGroup;
        }
    }
    private void OnDestroy()
    {
        _SpawnEnemyUnits.OnEnemyUnitSpawned -= SetUnitInGroup;
        for (int i = 0; i < _AIgroups.Length; i++)
        {
            _AIgroups[i].OnMissionComplited -= SetMissionForGroup;
        }
    }
    private void SetUnitInGroup(EnemyUnit unit)
    {
        for (int i = 0; i < _AIgroups.Length; i++)
        {
            if (!_AIgroups[i].isFull)
            {
                _AIgroups[i].SetUnitInList(unit);
                return;
            }
        }
        unit.GetComponent<DamagableObject>().TakeDamage(100000);
        _SpawnEnemyUnits.isNeed = false;
    }

    private void SetMissionForGroup(AIgroup aIgroup)
    {
        CalculateMission(aIgroup);
        aIgroup.SetMission(_Mission, _MissionPosition);
        Debug.Log(_Mission);
    }

    private void CalculateMission(AIgroup aIgroup)
    {
        if (aIgroup.isGroupReady && aIgroup.isFull)
        {
            _Mission = Mission.Attack;
            _MissionPosition = BattleManager._Instance._Buildings[Random.Range(0, BattleManager._Instance._Buildings.Count)].transform.position;
        }
        else
        {
            _Mission = Mission.Stay;
            _MissionPosition = _PlaceToStay.position;
        }
    }
}
