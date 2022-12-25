using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoPanel : MonoBehaviour
{
    [SerializeField] private Image _Image;

    [SerializeField] private HealthBarInUI _HealthBarInUI;

    private PlayerUnit _Unit;
    private void Awake()
    {
        _HealthBarInUI.gameObject.SetActive(false);
    }

    public void SetUnit(DamagableObject damagableObject)
    {
        if (damagableObject == null)
        {
            _HealthBarInUI.UnsubscribeDamagableObject();
            _HealthBarInUI.gameObject.SetActive(false);
        }
        else
        {
            _Unit = damagableObject.GetComponent<PlayerUnit>();
            _Image.sprite = _Unit.GetIcon();
            _HealthBarInUI.gameObject.SetActive(true);
            _HealthBarInUI.Init(damagableObject);
        }
    }
}
