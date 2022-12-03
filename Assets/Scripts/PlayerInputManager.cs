using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager _Instance { get; private set; }

    public Action OnDownLeftClickMouse;
    public Action OnPressLeftClickMouse;
    public Action OnUpLeftClickMouse;
    public Action OnRightClickMouse;
    void Awake()
    {
        _Instance = this;
    }

    void Update()
    {
        MouseInput();
    }

    private void MouseInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnDownLeftClickMouse?.Invoke();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            OnPressLeftClickMouse?.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnUpLeftClickMouse?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            OnRightClickMouse?.Invoke();
        }
    }
}
