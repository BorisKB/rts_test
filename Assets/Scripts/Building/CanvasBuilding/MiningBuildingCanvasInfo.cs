using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningBuildingCanvasInfo : MonoBehaviour
{
    [SerializeField] private MiningBuilding _MiningBuilding;
    [SerializeField] private Text _RightPanelText;
    private ResourceType _ResourecesType;
    // Start is called before the first frame update
    void Start()
    {
        _ResourecesType = _MiningBuilding.GetResourceType();
        _MiningBuilding.OnResourcesUpdated += OnUpdateResoursece;
        _MiningBuilding.GetResources(_ResourecesType, 0);
    }

    private void OnDestroy()
    {
        _MiningBuilding.OnResourcesUpdated -= OnUpdateResoursece;
    }

    private void OnUpdateResoursece(int count)
    {
        _ResourecesType = _MiningBuilding.GetResourceType();
        _RightPanelText.text = _ResourecesType.ToString() + " : " + count.ToString();
    }
}
