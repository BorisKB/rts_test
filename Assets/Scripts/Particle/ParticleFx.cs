using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFx : MonoBehaviour
{
    [SerializeField] private float _DestroyTime;
    private Transform _TargetTransform;

    private void Start()
    {
        Destroy(gameObject, _DestroyTime);
    }
    private void LateUpdate()
    {
        if(_TargetTransform != null)
        {
            transform.position = _TargetTransform.position;
        }
    }
    public void SetupTarget(Transform target)
    {
        _TargetTransform = target;
    }
}
