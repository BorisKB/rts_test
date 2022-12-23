using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitsContainer : MonoBehaviour
{
    public static TraitsContainer _Instance;
    public List<TraitBase> Traits = new List<TraitBase>();
    public List<TraitWorker> WorkerTraits = new List<TraitWorker>();
    private void Awake()
    {
        _Instance = this;
    }
}
