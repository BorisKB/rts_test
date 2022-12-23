using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trait", menuName = "Traits/TraitWorker")]
public class TraitWorker : TraitBase
{
    public int MiningPerIteration = 0;
    public int MaxCountTake = 0;
}
