using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroupPanel : MonoBehaviour
{
    [SerializeField] private List<UnitInfoPanel> _UnitInfoPanels = new List<UnitInfoPanel>();

    public void SetupUnitsInfo(List<DamagableObject> damagableObjects)
    {
        for (int i = damagableObjects.Count-1; i < _UnitInfoPanels.Count; i++)
        {
            _UnitInfoPanels[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < damagableObjects.Count; i++)
        {
            _UnitInfoPanels[i].gameObject.SetActive(true);
            _UnitInfoPanels[i].SetUnit(damagableObjects[i]);
        }
    }
}
