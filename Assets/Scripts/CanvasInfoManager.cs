using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInfoManager : MonoBehaviour
{
    [SerializeField] private UnitGroupPanel _UnitGroupPanel;
    [SerializeField] private CanvasInfo _UnitGroupPanelCanvasInfo;

    private List<DamagableObject> _DamagableObjects;

    private CanvasInfo _CurrentCanvasInfo;
    private void Start()
    {
        Selector.OnSelectedDamagableObjectUpdated += SetNeedCanvasInfo;
    }
    private void OnDestroy()
    {
        Selector.OnSelectedDamagableObjectUpdated -= SetNeedCanvasInfo;
    }

    private void SetNeedCanvasInfo(List<DamagableObject> damagableObjects)
    {
        if(_CurrentCanvasInfo != null) 
        {
            _CurrentCanvasInfo.SetActiveCanvasInfo(false);
            _CurrentCanvasInfo = null;
        }
        if (damagableObjects.Count == 0) { return; }
        else if(damagableObjects.Count == 1)
        {
            _CurrentCanvasInfo = damagableObjects[0].GetComponent<CanvasInfo>();
            _CurrentCanvasInfo.SetActiveCanvasInfo(true);
        }
        else
        {
            _CurrentCanvasInfo = _UnitGroupPanelCanvasInfo;
            _CurrentCanvasInfo.SetActiveCanvasInfo(true);
            _UnitGroupPanel.SetupUnitsInfo(damagableObjects);
        }
    }
}
