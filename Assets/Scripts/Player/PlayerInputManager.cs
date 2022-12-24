using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager _Instance { get; private set; }

    #region Keyboard
    [SerializeReference] private KeyCode Key_W = KeyCode.W;
    [SerializeReference] private KeyCode Key_A = KeyCode.A;
    [SerializeReference] private KeyCode Key_D = KeyCode.D;
    [SerializeReference] private KeyCode Key_S = KeyCode.S;
    [SerializeReference] private KeyCode Key_Escape = KeyCode.Escape;
    #endregion

    #region MouseAction
    public Action OnDownLeftClickMouse;
    public Action OnPressLeftClickMouse;
    public Action OnUpLeftClickMouse;
    public Action OnRightClickMouse;
    #endregion
    #region KeyBoardAction
    public Action OnPressEscape;
    public Action OnPressW;
    public Action OnPressS;
    public Action OnPressA;
    public Action OnPressD;
    #endregion

    void Awake()
    {
        _Instance = this;
    }

    void Update()
    {
        MouseInput();
        KeyBoardInput();
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
    private void KeyBoardInput()
    {
        if (Input.GetKey(Key_W))
        {
            OnPressW?.Invoke();
        }
        if (Input.GetKey(Key_S))
        {
            OnPressS?.Invoke();
        }
        if (Input.GetKey(Key_A))
        {
            OnPressA?.Invoke();
        }
        if (Input.GetKey(Key_D))
        {
            OnPressD?.Invoke();
        }
        if (Input.GetKey(Key_Escape))
        {
            OnPressEscape?.Invoke();
        }
    }
}
