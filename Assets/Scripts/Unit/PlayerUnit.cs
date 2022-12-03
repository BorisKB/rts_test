using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit, ISelectable
{
    [SerializeField] private GameObject _SeletctedInfo;
    [SerializeField] private GameObject _CanvasInfo;

    void Start()
    {
        _SeletctedInfo.SetActive(false);
    }

    public override void AttackEnemy(Unit enemy)
    {
        base.AttackEnemy(enemy);
    }
    public virtual void Selected()
    {
        _SeletctedInfo.SetActive(true);
        _CanvasInfo.SetActive(true);
        Commander.MovePosition += Move;
        Commander.AttackEnemy += ChasingTarget;
    }

    public virtual void UnSelected()
    {
        _SeletctedInfo.SetActive(false);
        _CanvasInfo.SetActive(false);
        Commander.MovePosition -= Move;
        Commander.AttackEnemy -= ChasingTarget;
    }
}
