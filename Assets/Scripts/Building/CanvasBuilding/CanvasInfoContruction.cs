using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInfoContruction : MonoBehaviour
{
    [SerializeField] private NeedResourcesBuilding _ConstructionBuilding;
    [SerializeField] private Text[] _RightPanelText;

    private Dictionary<ResourceType, int> res;
    // Start is called before the first frame update
    void Start()
    {
        if (_ConstructionBuilding != null)
        {
            _ConstructionBuilding.OnResourcesUpdated += OnUpdateResoursece;
        }
    }

    // Update is called once per frame
    private void OnEnable()
    {
        OnUpdateResoursece();
    }
    private void OnDestroy()
    {
        if (_ConstructionBuilding != null)
        {
            _ConstructionBuilding.OnResourcesUpdated -= OnUpdateResoursece;
        }
    }

    private void OnUpdateResoursece()
    {
        res = _ConstructionBuilding.GetCost();
        int count = 0;
        foreach (var resource in res)
        {
            _RightPanelText[count].text = resource.Key.ToString() + " : " + resource.Value.ToString();
            count++;
        }
    }
}
