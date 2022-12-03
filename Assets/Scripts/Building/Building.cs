using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Building : MonoBehaviour, ISelectable
{
    [SerializeField] private int[] _Price;
    [SerializeField] private int _XSize = 1;
    [SerializeField] private int _ZSize = 1;
    [SerializeField] private GameObject _CanvasInfo;

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
