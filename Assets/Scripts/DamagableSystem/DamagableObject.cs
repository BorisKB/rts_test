using System;
using Unity.Mathematics;
using UnityEngine;

public class DamagableObject : MonoBehaviour
{
    [Header("Properites / gameplay")]
    [SerializeField] private int _MaxHealth;
    [SerializeField] private int _Health;
    [SerializeField] private int _Armor = 0;
    [SerializeField] private int _Team = 0;

    [Header("Properites")]
    [SerializeField] private Transform _HealthBarPosition;
    [SerializeField] private HealthBar _HealthBarPrefab;

    public Action OnDied;
    public Action<int, int> OnHealthChanged;
    public int GetTeam() { return _Team; }
    public void SetTeam(int value) { _Team = value; }
    public int GetArmor() { return _Armor; }
    public void SetArmor(int value) { _Armor = value; }
    public int GetMaxHealth() { return _MaxHealth; }
    public int GetHealth() { return _Health; }
    public void SetMaxHealth(int value) { _MaxHealth = value; OnHealthChanged?.Invoke(_Health, _MaxHealth); }
    private void Start()
    {
        if(_MaxHealth == 0) { _MaxHealth = _Health; }
        HealthBar healthBar = Instantiate(_HealthBarPrefab, transform.position, quaternion.identity);
        healthBar.Setup(_HealthBarPosition, this);
    }

    public void TakeDamage(int amount)
    {
        if (_Health <= 0) { return; }
        int damage = amount - _Armor > 0 ? amount - _Armor : 1;

        _Health = math.max(_Health - damage, 0);

        OnHealthChanged?.Invoke(_Health, _MaxHealth);
        if (_Health == 0)
        {
            Death();
        }
    }
    public void Death()
    {
        OnDied?.Invoke();
        if(_Team == 0) { BattleManager._Instance.RemoveToAllFriendlyDamagableList(this); }
        else { BattleManager._Instance.RemoveToAllEnemyUnitsList(this); }
        Destroy(gameObject);
    }

}
