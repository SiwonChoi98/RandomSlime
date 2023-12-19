using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmallDroneBase : SkillPattern
{
    
    private Transform ObjectGet(int count)
    {
        return SkillObjectPool.instance.Get(SkillLevel.projectile_name).transform;
    }

    private void Update()
    {
        Fire();
    }
    
   
}
