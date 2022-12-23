using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatsUI : MonoBehaviour
{
    [SerializeField] private PlayerUnit _Unit;

    [SerializeField] private Text _DamageStatsText;
    [SerializeField] private Text _ArmorStatsText;
    [SerializeField] private Text _SpeedStatsText;

    private void Start()
    {
        _Unit.OnStatsChanged += StatsChanged;
        _Unit.SetStatsUI();
    }
    private void OnDestroy()
    {
        _Unit.OnStatsChanged -= StatsChanged;

    }
    private void StatsChanged(int damage, int armor, float speed)
    {
        _DamageStatsText.text = "Damage: - " + damage.ToString();
        _ArmorStatsText.text = "Armor: - " + armor.ToString();
        _SpeedStatsText.text = "Speed: - " + speed.ToString();
    }
}
