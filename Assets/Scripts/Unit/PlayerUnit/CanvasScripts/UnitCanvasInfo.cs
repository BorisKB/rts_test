using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCanvasInfo : MonoBehaviour
{
    private string _UnitName = "null";
    private string _UnitNickname = "";
    [SerializeField] private Image _LeftPanelImage = null;
    [SerializeField] private Text _LeftPanelUnitNameText;

    // Start is called before the first frame update
    void Start()
    {
        _UnitName = NameGiver.GetRandomNameUnit();
        UpdateText();
    }
    private void UpdateText()
    {
        _LeftPanelUnitNameText.text = _UnitName + " " + _UnitNickname;
    }
    public void SetUnitNickName(string Nickname)
    {
        _UnitNickname = Nickname;
        UpdateText();
    }
    public void SetIcon(Sprite icon)
    {
        _LeftPanelImage.sprite = icon;
    }
}
