using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : SkillPattern
{
    private void Start()
    {
        GameManager.Instance.player.addCritical = GameManager.Instance.player.addCritical
                                                  + SkillLevel.value1;
    }
}
