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
        if (_DamagalbeObject != null) { Init(_DamagalbeObject); };
    }

    public void Init(DamagableObject damagableObject)
    {
        if(_DamagalbeObject != null) { UnsubscribeDamagableObject(); }

        _DamagalbeObject = damagableObject;
        _DamagalbeObject.OnHealthChanged += SetValue;
        _HealthBar.maxValue = _DamagalbeObject.GetMaxHealth();
        _HealthBar.value = _DamagalbeObject.GetHealth();
        if (_HealthText != null)
        {
            _HealthText.text = _HealthBar.value.ToString() + " / " + _HealthBar.maxValue.ToString();
        }
    }
    public void UnsubscribeDamagableObject()
    {
        _DamagalbeObject.OnHealthChanged -= SetValue;
    }
    private void OnDestroy()
    {
        UnsubscribeDamagableObject();
    }

    private void SetValue(int health, int maxHealth)
    {
        if (_HealthBar.maxValue != maxHealth) { _HealthBar.maxValue = maxHealth; }
        _HealthBar.value = health;
        if (_HealthText != null) 
        { 
            _HealthText.text = health.ToString() + " / " + maxHealth.ToString();
        }
    }
}
