using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : Attack
{
    private Rigidbody2D _rigid;
    private bool isMove = false;
    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    protected override void OnEnable()
    {
        isMove = true;
        transform.position = GameManager.Instance.player.transform.position;
        Init(damage, 40);
    }

    private void Update()
    {
        if(isMove)
            _rigid.velocity = transform.up * moveSpeed;
        else
        {
            _rigid.velocity = Vector2.zero;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            isMove = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position,5f, LayerMask.GetMask("Enemy"));
            foreach (Collider2D collider in colliders)
            {
                collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                
            }
            SoundManager.Instance.SfxPlaySound("Missile");
            GameObject missileHit = EffectPool.instance.Get(0, 0.5f);
            missileHit.transform.position = transform.position;
            gameObject.SetActive(false);
           
            
        }
    }
    
  
}
