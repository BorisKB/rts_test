using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _PanSpeed;
    [SerializeField] private Vector2 _PanLimit;

    private Vector3 _Position;

    private void Start()
    {
        PlayerInputManager._Instance.OnPressW += MoveForwad;
        PlayerInputManager._Instance.OnPressS += MoveBack;
        PlayerInputManager._Instance.OnPressA += MoveLeft;
        PlayerInputManager._Instance.OnPressD += MoveRight;
    }
    private void OnDestroy()
    {
        PlayerInputManager._Instance.OnPressW -= MoveForwad;
        PlayerInputManager._Instance.OnPressS -= MoveBack;
        PlayerInputManager._Instance.OnPressA -= MoveLeft;
        PlayerInputManager._Instance.OnPressD -= MoveRight;
    }
    private void MoveForwad()
    {
        _Position = transform.position;
        _Position.z += _PanSpeed * Time.deltaTime;
        CorrectPositon(_Position);
    }
    private void MoveBack()
    {
        _Position = transform.position;
        _Position.z -= _PanSpeed * Time.deltaTime;
        CorrectPositon(_Position);
    }
    private void MoveLeft()
    {
        _Position = transform.position;
        _Position.x -= _PanSpeed * Time.deltaTime;
        CorrectPositon(_Position);
    }
    private void MoveRight()
    {
        _Position = transform.position;
        _Position.x += _PanSpeed * Time.deltaTime;
        CorrectPositon(_Position);
    }
    private void CorrectPositon(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, -_PanLimit.x, _PanLimit.x);
        pos.z = Mathf.Clamp(pos.z, -_PanLimit.y, _PanLimit.y);
        transform.position = pos;
    }
}
