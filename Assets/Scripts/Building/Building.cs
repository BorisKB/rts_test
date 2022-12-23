using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(DamagableObject))]
public class Building : MonoBehaviour, ISelectable
{
    [SerializeField] private int[] _Price = new int[Resource._ResCount];
    [SerializeField] private int _XSize = 1;
    [SerializeField] private int _ZSize = 1;
    [SerializeField] private GameObject _CanvasInfo;

    private DamagableObject _DamagableObject;

    private void Awake()
    {
        _DamagableObject = GetComponent<DamagableObject>();
        _DamagableObject.OnDied += Death;
    }
    public int[] GetPrice() { return _Price; }

    public void Selected()
    {
        if (_CanvasInfo != null)
        {
            _CanvasInfo.SetActive(true);
        }
    }

    public void UnSelected()
    {
        if (_CanvasInfo != null)
        {
            _CanvasInfo.SetActive(false);
        }
    }

    protected virtual void Death()
    {
        _DamagableObject.OnDied -= Death;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < _XSize; x++)
        {
            for (int z = 0; z < _ZSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z), new Vector3(1, 0, 1));
            }
        }
    }
}
