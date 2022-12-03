using System;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    [SerializeField] private Camera _PlayerCamera;

    public static Action<Vector3> MovePosition;
    public static Action<Unit> AttackEnemy;
    public static Action<MiningBuilding> Mining;
    public static Action<StorageResources> GoStorage;
    public static Action<ConstructionBuilding> GoBuilding;

    private void Start()
    {
        PlayerInputManager._Instance.OnRightClickMouse += OnRightClick;
    }
    private void OnDestroy()
    {
        PlayerInputManager._Instance.OnRightClickMouse -= OnRightClick;
    }

    #region Input
    private void OnRightClick() 
    {
        Ray ray = _PlayerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                MovePosition?.Invoke(hit.point);
            }
            else if (hit.collider.TryGetComponent(out Unit unit) != false && unit.GetTeam() != 0) 
            {
                AttackEnemy?.Invoke(unit);
            }
            else if(hit.collider.TryGetComponent(out MiningBuilding miningBuilding) != false)
            {
                Mining?.Invoke(miningBuilding);
            }
            else if(hit.collider.TryGetComponent(out StorageResources storageResources) != false)
            {
                GoStorage?.Invoke(storageResources);
            }
            else if(hit.collider.TryGetComponent(out ConstructionBuilding constructionBuilding) != false) 
            {
                GoBuilding?.Invoke(constructionBuilding);
            }

        }
    }
    #endregion
}
