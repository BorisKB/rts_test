using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCanvasInfo : MonoBehaviour
{
    private string _UnitName = "null";
    private string _UnitNickname = "null";
    [SerializeField] private Sprite _LeftPanelUnitIcon;
    [SerializeField] private Image _LeftPanelImage = null;
    [SerializeField] private Text _LeftPanelUnitNameText;

    // Start is called before the first frame update
    void Start()
    {
        _UnitName = NameGiver.GetRandomNameUnit();
        _UnitNickname = NameGiver.GetRandomNicknameUnit();
        _LeftPanelUnitNameText.text = _UnitName;   
    }

    public void SetUnitNickName(string Nickname)
    {
        _LeftPanelUnitNameText.text = _UnitName + " " + Nickname;
    }
    public void SetIcon(Sprite icon)
    {
        _LeftPanelImage.sprite = icon;
    }
}
