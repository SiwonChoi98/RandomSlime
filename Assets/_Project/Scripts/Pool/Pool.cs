using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Pool : MonoBehaviour
{
    
    [Header("풀")] 
    public GameObject[]? prefabs;
    [SerializeField] protected List<GameObject>[] pools;
    
    protected Dictionary<string, List<GameObject>> poolDict;
    
    
    
    protected virtual void Awake()
    {
        poolDict = new();
        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    
}
