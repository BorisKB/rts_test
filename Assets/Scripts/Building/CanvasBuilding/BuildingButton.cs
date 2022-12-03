using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private ConstructionBuilding _Building;

    private BuildingPlacer placer;

    private void Start()
    {
        placer = BuildingPlacer._Instance;
    }

    public void TryBuy()
    {
        ConstructionBuilding building = Instantiate(_Building, Vector3.down * 1000f, Quaternion.identity);
        Renderer renderer = Instantiate(building.GetBuildingRenderer(), Vector3.down * 1000f, Quaternion.identity);
        placer.SetBulding(building, renderer);
    }
}
