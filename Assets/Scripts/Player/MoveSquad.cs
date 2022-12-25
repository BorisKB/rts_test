using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveSquad
{
    public static List<Unit> SelectedUnits = new List<Unit>();

    public static void SetMovePosition(Vector3 pos)
    {
        if (SelectedUnits.Count == 0) { return; }
        else if (SelectedUnits.Count == 1)
        {
            if (SelectedUnits[0] == null) { return; }
            SelectedUnits[0].Move(pos);
        }
        else
        {
            int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(SelectedUnits.Count));
            int row;
            int column;
            Vector3 point;
            for (int i = 0; i < SelectedUnits.Count; i++)
            {
                row = i / rowNumber;
                column = i % rowNumber;
                point = pos + new Vector3(row, 0f, column);
                SelectedUnits[i].Move(point);
            }
        }
    }
}
