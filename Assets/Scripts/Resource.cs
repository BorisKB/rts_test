using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Stone,
    Wood,
    Steel,
    Game
}
public static class Resource
{

    static public int _ResCount = 4;

    static public Dictionary<ResourceType, int> CreateDicRes()
    {
        return new Dictionary<ResourceType, int>()
        {
            {ResourceType.Stone, 0},
            {ResourceType.Wood, 0},
            {ResourceType.Steel, 0},
            {ResourceType.Game, 0},
        };
    }
}
