using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    [SerializeField] private Camera _PlayerCamera;
    [SerializeField] private int _MaxUnitSelected;
    [SerializeField] private Image _FrameImage;
    private Vector2 _StartFrame;
    private Vector2 _EndFrame;
    private Vector2 _MinFrame;
    private Vector2 _MaxFrame;
    private Vector2 _SizeFrame;

    private List<ISelectable> _SelectedObjects = new List<ISelectable>();
    private List<DamagableObject> _SelectedDamagableObjects = new List<DamagableObject>();
    public static Action<List<DamagableObject>> OnSelectedDamagableObjectUpdated;

    private void Start()
    {
        PlayerInputManager._Instance.OnDownLeftClickMouse += OnDownLeftClick;
        PlayerInputManager._Instance.OnPressLeftClickMouse += OnPressLeftClick;
        PlayerInputManager._Instance.OnUpLeftClickMouse += OnUpLeftClick;
        PlayerInputManager._Instance.OnPressEscape += UnselectAll;
        BattleManager._Instance.OnSelectableDestroyed += ClearThisFromList;
        _MaxUnitSelected -= 1;// для массивов так надо
    }
    private void OnDestroy()
    {
        PlayerInputManager._Instance.OnDownLeftClickMouse -= OnDownLeftClick;
        PlayerInputManager._Instance.OnPressLeftClickMouse -= OnPressLeftClick;
        PlayerInputManager._Instance.OnUpLeftClickMouse -= OnUpLeftClick;
        PlayerInputManager._Instance.OnPressEscape -= UnselectAll;
        BattleManager._Instance.OnSelectableDestroyed -= ClearThisFromList;
    }

    #region Input
    private void UnselectAll()
    {
        ClearAllInList();
    }
    private void OnDownLeftClick() 
    {
        //ClearAllInList();

        _StartFrame = Input.mousePosition;
        Ray ray = _PlayerCamera.ScreenPointToRay(_StartFrame);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out DamagableObject selectedDamagableGameObject) == false) return;
            if (selectedDamagableGameObject.GetTeam() != 0) return;
            ClearAllInList();
            AddToList(selectedDamagableGameObject.GetComponent<ISelectable>(), selectedDamagableGameObject);
            if (selectedDamagableGameObject.TryGetComponent(out Unit unit))
            {
                MoveSquad.SelectedUnits.Add(unit);
            }
        }
    }
    private void OnPressLeftClick()
    {
        _EndFrame = Input.mousePosition;

        _MinFrame = Vector2.Min(_StartFrame, _EndFrame);
        _MaxFrame = Vector2.Max(_StartFrame, _EndFrame);
        _SizeFrame = _MaxFrame - _MinFrame;

        if (_SizeFrame.magnitude < 15) return;
        ClearAllInList();
        _FrameImage.enabled = true;
        _FrameImage.rectTransform.anchoredPosition = _MinFrame;
        _FrameImage.rectTransform.sizeDelta = _SizeFrame;

    }
    private void OnUpLeftClick()
    {
        Rect rect = new Rect(_MinFrame, _SizeFrame);

        List<DamagableObject> units = BattleManager._Instance.GetAllFriendlyDamagableObjectsList();
        for (int i = 0; i < units.Count; i++)
        {
            if (_SelectedObjects.Count <= _MaxUnitSelected)
            {
                Vector2 screenPosition = _PlayerCamera.WorldToScreenPoint(units[i].transform.position);
                if (rect.Contains(screenPosition))
                {
                    if (!units[i].CompareTag("Building"))
                    {
                        AddToList(units[i].GetComponent<ISelectable>(), units[i]);
                        MoveSquad.SelectedUnits.Add(units[i].GetComponent<Unit>());
                    }
                }
            }
            else
            {
                break;
            }
        }
        _FrameImage.enabled = false;
    }
    #endregion

    #region InteractWihtSelectedObjectsList
    private void AddToList(ISelectable selectedObject, DamagableObject damagableObject) 
    {
        if(_SelectedObjects.Contains(selectedObject) == false) 
        {
            _SelectedObjects.Add(selectedObject);
            _SelectedDamagableObjects.Add(damagableObject);
            selectedObject.Selected();
            OnSelectedDamagableObjectUpdated?.Invoke(_SelectedDamagableObjects);
        }
    }
    private void ClearAllInList() 
    {
        foreach (var selectedObject in _SelectedObjects)
        {
            selectedObject.UnSelected();
        }
        _SelectedObjects.Clear();
        _SelectedDamagableObjects.Clear();
        MoveSquad.SelectedUnits.Clear();
        OnSelectedDamagableObjectUpdated?.Invoke(_SelectedDamagableObjects);
    }
    private void ClearThisFromList(ISelectable selectable, DamagableObject damagableObject)
    {
        if (_SelectedObjects.Contains(selectable))
        {
            selectable.UnSelected();
            _SelectedObjects.Remove(selectable);
            _SelectedDamagableObjects.Remove(damagableObject);
            if (damagableObject.TryGetComponent(out Unit unit))
            {
                MoveSquad.SelectedUnits.Remove(unit);
            }
            OnSelectedDamagableObjectUpdated?.Invoke(_SelectedDamagableObjects);
        }
    }
    #endregion
}
