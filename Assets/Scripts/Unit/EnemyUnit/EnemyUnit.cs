using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public Action<EnemyUnit> OnDied;
    public override void Death()
    {
        base.Death();
        OnDied?.Invoke(this);
    }
}
