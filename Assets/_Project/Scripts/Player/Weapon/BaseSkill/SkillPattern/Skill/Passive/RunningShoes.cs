using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningShoes : SkillPattern
{
    private void Start()
    {
        GameManager.Instance.player.MoveSpeed = GameManager.Instance.player.MoveSpeed * 
                                                (1 + SkillLevel.value1/100);
    }
}
