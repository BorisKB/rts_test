using System;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    [SerializeField] private Camera _PlayerCamera;

    [SerializeField] private ParticleFx _ClickOnGround;
    [SerializeField] private ParticleFx _ClickOnEnemy;
    [SerializeField] private ParticleFx _ClickOnBuilding;

    private ParticleFx particle;

    public static Action<Vector3> MovePosition;
    public static Action<DamagableObject> AttackEnemy;
    public static Action<MiningBuilding> Mining;
    public static Action<StorageResources> GoStorage;
    public static Action<NeedResourcesBuilding> GoBuilding;

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
                MoveSquad.SetMovePosition(hit.point);
                //MovePosition?.Invoke(hit.point);
                if (particle != null) { Destroy(particle.gameObject); }
                particle = Instantiate(_ClickOnGround, hit.point, Quaternion.identity);
            }
            else if (hit.collider.TryGetComponent(out DamagableObject unit) != false && unit.GetTeam() != 0)
            {
                AttackEnemy?.Invoke(unit);
                if (particle != null) { Destroy(particle.gameObject); }
                particle = Instantiate(_ClickOnEnemy, unit.transform.position, Quaternion.identity);
                particle.SetupTarget(unit.transform);
            }
            else if (hit.collider.CompareTag("Building")) 
            {
                if (hit.collider.TryGetComponent(out MiningBuilding miningBuilding) != false)
                {
                    Mining?.Invoke(miningBuilding);
                }
                else if (hit.collider.TryGetComponent(out StorageResources storageResources) != false)
                {
                    GoStorage?.Invoke(storageResources);
                }
                else if (hit.collider.TryGetComponent(out NeedResourcesBuilding needResourcesBuilding) != false)
                {
                    GoBuilding?.Invoke(needResourcesBuilding);
                }
                if(particle != null) { Destroy(particle.gameObject); }
                particle = Instantiate(_ClickOnBuilding, hit.transform.position, Quaternion.identity);
            }

        }
    }
    #endregion
}
