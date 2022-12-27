using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyUnits : MonoBehaviour
{
    [SerializeField] private EnemyUnit[] _EnemyUnitPrefabs;
    [SerializeField] private Transform[] _SpawnPoints;

    public Action<EnemyUnit> OnEnemyUnitSpawned;

    private float _Timer = 0;
    private float _TimeSpawn = 5;

    public bool isNeed = true;

    private void Update()
    {
        if (isNeed)
        {
            _Timer += Time.deltaTime;
            if (_Timer >= _TimeSpawn)
            {
                EnemyUnit unit = Instantiate(_EnemyUnitPrefabs[UnityEngine.Random.Range(0, _EnemyUnitPrefabs.Length)],
                    _SpawnPoints[UnityEngine.Random.Range(0, _SpawnPoints.Length)].position,
                    Quaternion.identity);
                BattleManager._Instance.AddToAllEnemyUnitsList(unit.GetComponent<DamagableObject>());
                _TimeSpawn = 1; //UnityEngine.Random.Range(5, 10);
                _Timer = 0;
                OnEnemyUnitSpawned?.Invoke(unit);
                Debug.Log("Spawned");
            }
        }
    }
}
