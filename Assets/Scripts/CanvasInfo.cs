using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInfo : MonoBehaviour
{
    [SerializeField] private GameObject _CanvasInfo;
    void Start()
    {
        _CanvasInfo.SetActive(false);
    }

    public void SetActiveCanvasInfo(bool state)
    {
        if(_CanvasInfo == null) { return; }
        _CanvasInfo.SetActive(state);
    }
}
