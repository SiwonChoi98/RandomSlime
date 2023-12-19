using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpLevel2 : Exp
{
    protected override void Awake()
    {
        base.Awake();
        Init(SpecDataManager.Instance.GameConfig.Get(3010).value);
    }
}
