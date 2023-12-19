using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBase : SkillPattern
{
    private bool isCount = true;
    
    private void Start()
    {
        isCount = true;
    }
    
    protected override GameObject GetObject()
    {
        if (isCount) //필드내 shield 오브젝트에선 1개만 생성
        {
            isCount = false;
            GameManager.Instance.isActiveShield = true;
            return SkillObjectPool.instance.Get(SkillLevel.projectile_name);

        }
        return null;
    }
    private void Update()
    {
        Fire();
    }
}
