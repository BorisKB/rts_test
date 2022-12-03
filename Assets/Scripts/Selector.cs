using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    [SerializeField] private Camera _PlayerCamera;

    [SerializeField] private Image _FrameImage;
    private Vector2 _StartFrame;
    private Vector2 _EndFrame;
    private Vector2 _MinFrame;
    private Vector2 _MaxFrame;
    private Vector2 _SizeFrame;

    private List<ISelectable> _SelectedObjects = new List<ISelectable>();

    private void Start()
    {
        PlayerInputManager._Instance.OnDownLeftClickMouse += OnDownLeftClick;
        PlayerInputManager._Instance.OnPressLeftClickMouse += OnPressLeftClick;
        PlayerInputManager._Instance.OnUpLeftClickMouse += OnUpLeftClick;
    }
    private void OnDestroy()
    {
        PlayerInputManager._Instance.OnDownLeftClickMouse -= OnDownLeftClick;
        PlayerInputManager._Instance.OnPressLeftClickMouse -= OnPressLeftClick;
        PlayerInputManager._Instance.OnUpLeftClickMouse -= OnUpLeftClick;
    }

    #region Input
    private void OnDownLeftClick() 
    {
        ClearAllInList();

        _StartFrame = Input.mousePosition;
        Ray ray = _PlayerCamera.ScreenPointToRay(_StartFrame);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out ISelectable selectedGameObject) == false) return;
            AddToList(selectedGameObject);
        }
    }
    private void OnPressLeftClick()
    {
        _EndFrame = Input.mousePosition;

        _MinFrame = Vector2.Min(_StartFrame, _EndFrame);
        _MaxFrame = Vector2.Max(_StartFrame, _EndFrame);
        _SizeFrame = _MaxFrame - _MinFrame;

        if (_SizeFrame.magnitude < 15) return;

        _FrameImage.enabled = true;
        _FrameImage.rectTransform.anchoredPosition = _MinFrame;
        _FrameImage.rectTransform.sizeDelta = _SizeFrame;

    }
    private void OnUpLeftClick()
    {
        Rect rect = new Rect(_MinFrame, _SizeFrame);

        List<Unit> testUnits = BattleManager._Instance.GetAllFriendlyUnitsList();
        for (int i = 0; i < testUnits.Count; i++)
        {
            Vector2 screenPosition = _PlayerCamera.WorldToScreenPoint(testUnits[i].transform.position);
            if (rect.Contains(screenPosition))
            {
                AddToList(testUnits[i].GetComponent<ISelectable>());
            }
        }
        _FrameImage.enabled = false;
    }
    #endregion

    #region InteractWihtSelectedObjectsList
    private void AddToList(ISelectable selectedObject) 
    {
        if(_SelectedObjects.Contains(selectedObject) == false) 
        {
            _SelectedObjects.Add(selectedObject);
            selectedObject.Selected();
        }
    }
    private void ClearAllInList() 
    {
        foreach (var selectedObject in _SelectedObjects)
        {
            selectedObject.UnSelected();
        }
        _SelectedObjects.Clear();
    }
    #endregion
}
