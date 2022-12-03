using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public static BuildingPlacer _Instance { get; private set; }

    private ConstructionBuilding _ConstructionBuildingPrefab;
    private Renderer _BuildingRenderer;

    [SerializeField] private Camera _Camera;

    private Vector3 _BuildingPosition = Vector3.zero;

    public void SetBulding(ConstructionBuilding building, Renderer buildingRenderer)
    {
        _ConstructionBuildingPrefab = building;
        _BuildingRenderer = buildingRenderer;
    }
    private void Awake()
    {
        _Instance = this;
    }
    private void Start()
    {
        PlayerInputManager._Instance.OnDownLeftClickMouse += OnDownLeftClick;
    }
    private void OnDestroy()
    {
        PlayerInputManager._Instance.OnDownLeftClickMouse -= OnDownLeftClick;
    }
    private void Update()
    {
        if(_ConstructionBuildingPrefab == null) { return; }

        Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            int x = Mathf.RoundToInt(hit.point.x);
            int z = Mathf.RoundToInt(hit.point.z);

            _BuildingPosition = new Vector3(x, 0, z);
            Color colorBuldingRenderer = _ConstructionBuildingPrefab.CanPlaceBuilding(_BuildingPosition) ? Color.green : Color.red;
            _BuildingRenderer.material.SetColor("_BaseColor", colorBuldingRenderer);
            _BuildingRenderer.transform.position = _BuildingPosition;  
        }

    }

    private void OnDownLeftClick()
    {
        if (_ConstructionBuildingPrefab == null) { return; }
        if (_ConstructionBuildingPrefab.CanPlaceBuilding(_BuildingPosition))
        {
            _ConstructionBuildingPrefab.transform.position = _BuildingPosition;
            Destroy(_BuildingRenderer.gameObject);
            _ConstructionBuildingPrefab = null;
            _BuildingRenderer = null;
        }
    }
}
