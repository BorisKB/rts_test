using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGiver
{
    private static string[] _UnitNames = new string[3] { "Борис", "Валера", "Павел" };
    private static string[] _UnitNickname = new string[3] {"Бритва", "Шахтер", "Хилый"};
    private static string[] _BuildingNickname;
    public static string GetRandomNameUnit()
    {
        return _UnitNames[Random.Range(0, _UnitNames.Length)];
    }
    public static string GetRandomNicknameUnit()
    {
        return _UnitNickname[Random.Range(0, _UnitNickname.Length)];
    }
    public static string GetRandomNicknameBuilding()
    {
        return _BuildingNickname[Random.Range(0, _BuildingNickname.Length)];
    }
}
