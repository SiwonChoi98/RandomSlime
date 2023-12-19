using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillObjectPool : Pool
{
    public static SkillObjectPool instance;
    
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

    public GameObject Get(string key)
    {
        GameObject select = null;

        //현재 딕셔너리에 해당 키가 있는지 체크하고 없으면 새롭게 추가해줘라
        if (poolDict.TryGetValue(key, out var objectList) == false)
        {
            var newList = new List<GameObject>();
            poolDict.Add(key, newList);
            objectList = newList;
        }

        //해당 오브젝트가 있으면 활성화
        foreach (GameObject item in objectList)
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);

                break;
            }
        }

        //해당 오브젝트가 없으면 생성
        if (!select)
        {
            GameObject skillObject = Resources.Load<GameObject>(key);
            select = Instantiate(skillObject, transform);
            objectList.Add(select);
        }
        
        return select;
    }

}
