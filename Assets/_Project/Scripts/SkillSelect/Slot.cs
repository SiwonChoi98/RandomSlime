using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Text nameText; //슬롯 이름
    public Text addExplanation; //추가 설명
    public GameObject newTextGo; //new 텍스트 오브젝트
    private SkillLevel _skill; 
    public void SetData(SkillLevel skill)
    {
        _skill = skill;
        InitUI(skill);
    }
    private void InitUI(SkillLevel skillLevel)
    {
        Skill skill = SpecDataManager.Instance.Skill.Get(_skill.skill_id);
        
        nameText.text = skill.name;
        addExplanation.text = string.Format(_skill.desc, _skill.value1); 
        
        if (skillLevel.level > 1)
        {
            newTextGo.SetActive(false);
        }
        else
        {
            newTextGo.SetActive(true);
        }
    }
    
    
}
