using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : SkillPattern
{
    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.EquipWeapon.ActiveAttacks.Count; i++)
        {
            GameManager.Instance.EquipWeapon.ActiveAttacks[i].initCoolTime = 
                GameManager.Instance.EquipWeapon.ActiveAttacks[i].initCoolTime - (0.01f * SkillLevel.value1);
        }
    }
}
