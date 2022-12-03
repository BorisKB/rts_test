using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageBuildingCanvasInfo : MonoBehaviour
{
    [SerializeField] private StorageResources _StorageResources;
    [SerializeField] private Text[] _RightPanelText;

    private Dictionary<ResourceType, int> res;
    // Start is called before the first frame update
    void Start()
    {
        if (_StorageResources != null)
        {
            _StorageResources.onResourcesUpdated += OnUpdateResoursece;
        }
    }

    // Update is called once per frame
    private void OnEnable()
    {
        OnUpdateResoursece();
    }
    private void OnDestroy()
    {
        if (_StorageResources != null)
        {
            _StorageResources.onResourcesUpdated -= OnUpdateResoursece;
        }
    }

    private void OnUpdateResoursece()
    {
        res = _StorageResources.GetResourecesTypes();
        int count = 0;
        foreach (var resource in res)
        {
            _RightPanelText[count].text = resource.Key.ToString() + " : " + resource.Value.ToString();
            count++;
        }
    }
}
