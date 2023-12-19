using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : Pool
{
    public static EffectPool instance;
    
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
    }
    //데미지 풀
    public GameObject Get(int index, float time)
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
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        
        StartCoroutine(effctSetFalse(select, time)); //시간 경과 후 비활성화
        
        return select;
    }
    private IEnumerator effctSetFalse(GameObject select, float time) //비활성화
    {
        yield return new WaitForSeconds(time);
        select.SetActive(false);
    }
}
