using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public struct activeLevelStar
{
    public List<GameObject> activelevelStar;
}
public class PauseUI : MonoBehaviour
{
    public Image[] activeSkillImage; //엑티브 스킬 이미지
    public Image[] passiveSkillImage; //패시브 스킬 이미지
    public List<activeLevelStar> activeStars;
    public List<activeLevelStar> passiveStars;
    private void OnEnable()
    {
        //엑티브 스킬이미지 활성화
        for (int i = 0; i < GameManager.Instance.EquipWeapon.ActiveAttacks.Count; i++)
        {
            activeSkillImage[i].sprite = GameManager.Instance.EquipWeapon.ActiveAttacks[i].sprite;
            
        }
       
        for (int i = 0; i < GameManager.Instance.EquipWeapon.ActiveAttacks.Count; i++)
        {
            for (int j = 0; j < GameManager.Instance.EquipWeapon.ActiveAttacks[i].SkillLevel.level; j++)
            {
                activeStars[i].activelevelStar[j].SetActive(true);

                SetStarColor(i, j);
            }
        }
        
        
    }

    private void SetStarColor(int i, int j)
    {
        if (GameManager.Instance.EquipWeapon.ActiveAttacks[i].SkillLevel.level == 5)
        {
            activeStars[i].activelevelStar[j].GetComponent<Image>().color = Color.red;    
        }
        else
        {
            activeStars[i].activelevelStar[j].GetComponent<Image>().color = Color.white;
        }  
    }
   
    private void OnDisable()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                activeStars[i].activelevelStar[j].SetActive(false);    
            }
        }
    }
}
