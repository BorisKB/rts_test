using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit, ISelectable
{
    [SerializeField] private GameObject _SeletctedInfo;
    [SerializeField] private UnitCanvasInfo _UnitCanvasInfo;
    [SerializeField] private Sprite _Icon;

    public Action<int, int, float> OnStatsChanged;
    protected bool isAlreadyHaveTrait = false;

    public Sprite GetIcon() { return _Icon; }
    void Start()
    {
        _SeletctedInfo.SetActive(false);
        _UnitCanvasInfo.SetIcon(_Icon);
    }
    public void SetTrait(TraitBase trait)
    {
        isAlreadyHaveTrait = true;
        _Damage += trait.Damage;
        _Speed += trait.Speed;
        _Agent.speed = _Speed;
        _DamagableObject.SetArmor(trait.Armor + _DamagableObject.GetArmor());
        _DamagableObject.SetMaxHealth(trait.MaxHealth + _DamagableObject.GetMaxHealth());
        _UnitCanvasInfo.SetUnitNickName(trait.nickName);
        _SoldierAI.Initialization();
        SetStatsUI();
    }
    public void SetStatsUI()
    {
        OnStatsChanged?.Invoke(_Damage, _DamagableObject.GetArmor(), _Speed);
    }

    public override void AttackEnemy(DamagableObject enemy)
    {
        base.AttackEnemy(enemy);
        if (isAlreadyHaveTrait) { return; }
        if(UnityEngine.Random.Range(0, 100) == 1)
        {
            Debug.Log("GEt Trait");
            SetTrait(TraitsContainer._Instance.Traits[UnityEngine.Random.Range(0, TraitsContainer._Instance.Traits.Count)]);
        }
    }
    public virtual void Selected()
    {
        _SeletctedInfo.SetActive(true);
        Commander.MovePosition += Move;
        Commander.AttackEnemy += ChasingTarget;
    }

    public virtual void UnSelected()
    {
        _SeletctedInfo.SetActive(false);
        Commander.MovePosition -= Move;
        Commander.AttackEnemy -= ChasingTarget;
    }

    public override void Death()
    {
        Commander.MovePosition -= Move;
        Commander.AttackEnemy -= ChasingTarget;
    }

}
