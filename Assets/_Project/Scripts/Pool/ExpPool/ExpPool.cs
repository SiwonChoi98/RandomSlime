using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPool : Pool
{
    public static ExpPool instance;
    
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
    
    public GameObject Get(int index)
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
        return select;
    }
}
