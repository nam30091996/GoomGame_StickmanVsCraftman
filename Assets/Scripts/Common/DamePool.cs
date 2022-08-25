using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class DamePool : Singleton<DamePool>
{
    [HideInInspector] public LeanGameObjectPool pool;

    public override void Awake()
    {
        base.Awake();
        pool = GetComponent<LeanGameObjectPool>();
    }
}
