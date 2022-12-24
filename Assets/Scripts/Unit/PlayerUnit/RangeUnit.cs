using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUnit : PlayerUnit
{
    [SerializeField] private BulletProjectile _ProjectilePrefab;
    [SerializeField] private Transform _ProjectileSpawnPosition;
    public override void AttackEnemy(DamagableObject enemy)
    {
        BulletProjectile projectile = Instantiate(_ProjectilePrefab, _ProjectileSpawnPosition.position, transform.rotation);
        projectile.SetupProjectile(enemy, _Damage);
    }
}
