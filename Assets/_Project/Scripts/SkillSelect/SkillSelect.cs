using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
[Serializable]
public struct SkillLevelStar
{
    public List<GameObject> skillLevelStar;
}
public class SkillSelect : MonoBehaviour
{
    [SerializeField] private List<Slot> _slotsList;
    //레벨에 따른 별 갯수
    public List<SkillLevelStar> SkillLevelStarList;
    //현재 가지고 있는 스킬 이미지
    public List<Image> CurActiveImagesList;
    public List<Image> CurPassiveImagesList;
    //현재 나온 스킬 이미지
    public List<Image> SkillImagesList;
    public List<SkillLevel> holdSelectedSkills;
    
    //패시브 스킬 나왔을 때 이미지
    public List<GameObject> PassiveImagesList;
    private void OnEnable()
    {
        List<SkillLevel> selectedSkills = GetRandomSkills();
        
        for(var i = 0; i < _slotsList.Count;i++)
        {
            _slotsList[i].SetData(selectedSkills[i]);
            holdSelectedSkills.Add(selectedSkills[i]);
        }

        // for (int i = 0; i < GameManager.Instance.EquipWeapon.ActiveAttacks.Count; i++)
        // {
        //     CurActiveImagesList[i].sprite = GameManager.Instance.EquipWeapon.ActiveAttacks[i].sprite;
        // }
        // for (int i = 0; i < GameManager.Instance.EquipWeapon.PassiveAttacks.Count; i++)
        // {
        //     CurPassiveImagesList[i].sprite = GameManager.Instance.EquipWeapon.PassiveAttacks[i].sprite;
        // }
    }
    
    private List<SkillLevel> GetRandomSkills()
    {
        List<SkillLevel> selectedSkills = new(); //선택 스킬들
        
        //스킬 리스트에 저장
        List<SkillLevel> skillPool = SpecDataManager.Instance.SkillLevel.All.ToList();
        //레벨에 따라서 현재 가지고 있는 레벨 보다 높은 것만
        skillPool = skillPool.FindAll((t) =>
        {
            // //액티브인지 패시브인지 검사
            if (t.prj_type != PrjType.NONE)
            {
                // //데이터 시트 받아야함
                // if (GameManager.Instance.EquipWeapon.ActiveAttacks.Count >= 4)
                // {
                //     if (GameManager.Instance.EquipWeapon.ActiveAttacks.Find(k => k.SkillId == t.skill_id))
                //     {
                //         return t.level == GameManager.Instance.EquipWeapon.ActiveAttacks.Find(k => k.SkillId == t.skill_id).skillLevel + 1;
                //     }
                //
                //     return false;
                // } 
                
                
                if (GameManager.Instance.EquipWeapon.ActiveAttacks.Find(k => k.SkillId == t.skill_id))
                {
                    return t.level == GameManager.Instance.EquipWeapon.ActiveAttacks.Find(k => k.SkillId == t.skill_id).skillLevel + 1;
                }
            }
            else
            {
                // if (GameManager.Instance.EquipWeapon.PassiveAttacks.Count >= 4)
                // {
                //     if (GameManager.Instance.EquipWeapon.PassiveAttacks.Find(k => k.SkillId == t.skill_id))
                //     {
                //         return t.level == GameManager.Instance.EquipWeapon.PassiveAttacks.Find(k => k.SkillId == t.skill_id).skillLevel + 1;
                //     }
                //
                //     return false;
                // } 
                
                if (GameManager.Instance.EquipWeapon.PassiveAttacks.Find(k => k.SkillId == t.skill_id))
                {
                    return t.level == GameManager.Instance.EquipWeapon.PassiveAttacks.Find(k => k.SkillId == t.skill_id).skillLevel + 1;
                }
            }
            return t.level == 1;
        });
        

        if (skillPool.Count == 0)
            return null;
          
        
        //랜덤으로 스킬 저장
        for (int i = 0; i < 3; i++)
        {
            int indexRan = Random.Range(0, skillPool.Count);
            SkillLevel selectedSkill = skillPool[indexRan];
            skillPool.Remove(selectedSkill);
            selectedSkills.Add(selectedSkill);


            Sprite selectedSkillSprite = Resources.Load<Sprite>(selectedSkill.sprite);
            SkillImagesList[i].sprite = selectedSkillSprite;
            
            
            //나온 스킬이 패시브 스킬인지 체크
            Skill skillId = SpecDataManager.Instance.Skill.All.ToList().Find(x => x.id == selectedSkill.skill_id);
            
            // if (skillId.skill_type == SkillType.PASSIVE)
            // {
            //     PassiveImagesList[i].SetActive(true);
            // }
            // else
            // {
            //     PassiveImagesList[i].SetActive(false);
            // }
            bool isPassive = skillId.skill_type == SkillType.PASSIVE ? true : false;
            PassiveImagesList[i].SetActive(isPassive);
        }
        return selectedSkills;

    }
    
    //선택한 스킬 캐릭터 저장
    public void SelectSkill(int index)
    {
        GameManager.Instance.SKillSelect(holdSelectedSkills[index]);
        SoundManager.Instance.SfxPlaySound("ItemAdd", 0.5f);
        holdSelectedSkills.RemoveRange(0, holdSelectedSkills.Count);
        GameManager.Instance.ExitButton(gameObject);
        GameManager.Instance.SkillSelectPs.gameObject.SetActive(false);
        GameManager.Instance.SkillSelectPs.Stop();
    }


    //나가기 버튼 //test 추후 사라짐
    public void ExitButton()
    {
        holdSelectedSkills.RemoveRange(0,holdSelectedSkills.Count);
        GameManager.Instance.ExitButton(gameObject);
    }
}
