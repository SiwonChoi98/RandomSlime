using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProjectile : Attack
{
    private Rigidbody2D _rigid;

    //private bool isTakeDamage;
    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    
    protected override void OnEnable()
    {
        SoundManager.Instance.SfxPlaySound("Shield", 0.1f);
        transform.position = GameManager.Instance.player.transform.position;
        Init(damage, 40);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GameManager.Instance.player.transform.position;
        
        if(!GameManager.Instance.isActiveShield) gameObject.SetActive(false);
            
    }
    
    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyComponent = other.gameObject.GetComponent<Enemy>();
            if (enemyComponent.isShieldDamage) //쉴드 데미지를 받을 수 있는 상태일 때만
            {
                enemyComponent.TakeDamage(damage);
                enemyComponent.isShieldDamage = false;
            }
            
        }

    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
    }
}
