using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trait", menuName = "Traits/TraitBase")]
public class TraitBase : ScriptableObject
{
    public string nickName = "null";
    public int Damage = 0;
    public int Armor = 0;
    public int MaxHealth = 0;
    public float Speed = 0;
}
