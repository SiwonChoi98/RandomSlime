using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpLevel3 : Exp
{
    protected override void Awake()
    {
        base.Awake();
        Init(SpecDataManager.Instance.GameConfig.Get(3011).value);
    }
}
