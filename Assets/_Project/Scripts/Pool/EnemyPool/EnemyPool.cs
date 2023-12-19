using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : Pool
{
    public static EnemyPool instance;
    
    [Header("몬스터 처음 생성 갯수")] 
    [SerializeField] private int _initEnemyCount;
    
    protected override void Awake()
    {
        base.Awake();
        Singleton();
    }

    private void Start()
    {
        InitSetEnemy();
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
    
    //첫 시작 시 몬스터 풀링에 일정 갯수 저장
     private void InitSetEnemy()
     {
         _initEnemyCount = 200;
    
         for (int i = 0; i < _initEnemyCount; i++)
         {
             GameObject select = Instantiate(prefabs[0], transform);
             pools[0].Add(select);
             select.SetActive(false);
         }
     }

    
    public GameObject Get(int index, Monster monster)
    {
        GameObject select = null;
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                GameManager.Instance.EnemyActiveCount++; //몬스터 관리
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
            GameManager.Instance.EnemyActiveCount++;
        }
       
        select.GetComponent<Enemy>().SetMonster(monster);
        
        return select;
    }
}
