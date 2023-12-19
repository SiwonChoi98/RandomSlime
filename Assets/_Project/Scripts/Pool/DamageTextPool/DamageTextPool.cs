using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class DamageTextPool : Pool
{
    public static DamageTextPool instance;
    [Header("데미지 텍스트")]
    private StringBuilder damageTextBuilder = new StringBuilder();
    
    protected override void Awake()
    {
        base.Awake();
        Singleton();
    }
    protected void Singleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        //DontDestroyOnLoad(gameObject);
    }
    //데미지 풀
    public GameObject Get(int index, float damage, Transform pos, bool isCri)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(prefabs[index], pos);
            pools[index].Add(select);
        }

        float ran = Random.Range(-0.3f, 0.3f);
        select.transform.position = pos.position + new Vector3(ran,0,0); //생성위치
        select.transform.SetParent(transform); //생성 위치 부모
        select.transform.rotation = Quaternion.Euler(new Vector3(0,0,0)); //카메라 방향으로 회전

        select.GetComponentInChildren<Text>().text = ""; //데미지 텍스트 생성되기전 비워주기

        if (isCri)
        {
            select.GetComponentInChildren<Text>().color = new Color32(255,175,54,255);
        }
        else
        {
            select.GetComponentInChildren<Text>().color = new Color32(255,255,255,255);
        }
        
        if(damageTextBuilder.Length != 0) damageTextBuilder.Remove(0, damageTextBuilder.Length); //스트링빌더 비워주기
        damageTextBuilder.Append(damage); //스트링빌더 추가
        for(int i=0; i<damageTextBuilder.Length; i++)
        {
            select.GetComponentInChildren<Text>().text += damageTextBuilder[i].ToString();
        }
        //select.GetComponentInChildren<Text>().text = damage.ToString(); //텍스트 수정
        StartCoroutine(DmgSetFalse(select)); //시간 경과 후 비활성화
        
        return select;
    }
    private IEnumerator DmgSetFalse(GameObject select) //비활성화
    {
        yield return new WaitForSeconds(0.1f);
        select.SetActive(false);
    }
}