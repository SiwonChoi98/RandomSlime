using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCandy : SkillPattern
{
    private void Start()
    {
        GameManager.Instance.player.CurHealth = GameManager.Instance.player.CurHealth *
            (1+SkillLevel.value1/100);
        GameManager.Instance.player.MaxHealth = GameManager.Instance.player.MaxHealth *
            (1+SkillLevel.value1/100);
    }
}
