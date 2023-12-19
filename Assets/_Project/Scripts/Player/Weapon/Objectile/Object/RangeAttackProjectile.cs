using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class RangeAttackProjectile : Attack
{
    private Rigidbody2D _rigid;
    
    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }


    protected override void Start()
    {
    }

    protected override void OnEnable()
    {
        
        Init(3, 10);
       
    }

    private void Update()
    {
       
       _rigid.velocity = transform.up * moveSpeed;
        
    }
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
            Debug.Log("원거리 공격으로 인한 공격받음");
            gameObject.SetActive(false);
        }
        
    }
    

    
}