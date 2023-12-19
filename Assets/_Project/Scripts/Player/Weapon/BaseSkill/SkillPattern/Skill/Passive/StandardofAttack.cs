using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardofAttack : SkillPattern
{
    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.EquipWeapon.ActiveAttacks.Count; i++)
        {
            GameManager.Instance.EquipWeapon.ActiveAttacks[i].damage = 
                GameManager.Instance.EquipWeapon.ActiveAttacks[i].damage * (1+SkillLevel.value1/100);
            
        }
    }

    
}
