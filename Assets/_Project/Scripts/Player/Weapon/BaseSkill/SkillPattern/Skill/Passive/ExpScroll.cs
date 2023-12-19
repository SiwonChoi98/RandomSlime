using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpScroll : SkillPattern
{
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.player.addExp = GameManager.Instance.player.addExp + (SkillLevel.value1/100);
    }

}
