using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float _Speed = 5f;

    private Rigidbody _Rigidbody;
    private Vector3 _Position;

    private Transform _TargetTransform;
    private DamagableObject _Target;
    private int _Damage = 0;
    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _Rigidbody.isKinematic = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_TargetTransform != null && _Target != null)
        {
            _Position = _TargetTransform.position - transform.position + Vector3.up;
            _Rigidbody.velocity = _Position.normalized * _Speed;
            transform.LookAt(_TargetTransform.position + Vector3.up);
            if (_Position.sqrMagnitude < 1)
            {
                _Target.TakeDamage(_Damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetupProjectile(DamagableObject target, int damage)
    {
        _Target = target;
        _TargetTransform = target.transform;
        _Damage = damage;
    }
}
