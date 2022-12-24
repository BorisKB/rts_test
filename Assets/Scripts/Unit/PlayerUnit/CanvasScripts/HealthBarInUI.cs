using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarInUI : MonoBehaviour
{
    [SerializeField] private DamagableObject _DamagalbeObject;
    [SerializeField] private Slider _HealthBar;
    [SerializeField] private Text _HealthText;

    private void Awake()
    {
        _DamagalbeObject.OnHealthChanged += SetValue;
        _HealthBar.maxValue = _DamagalbeObject.GetMaxHealth();
        _HealthBar.value = _DamagalbeObject.GetMaxHealth();
        _HealthText.text = _HealthBar.value.ToString() + " / " + _HealthBar.maxValue.ToString();
    }
    private void OnDestroy()
    {
        _DamagalbeObject.OnHealthChanged -= SetValue;
    }

    private void SetValue(int health, int maxHealth)
    {
        if(_HealthBar.maxValue != maxHealth) { _HealthBar.maxValue = maxHealth; }
        _HealthBar.value = health;
        _HealthText.text = health.ToString() + " / " + maxHealth.ToString();
    }
}
