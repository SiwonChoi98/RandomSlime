using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBubblePojectile : Attack
{
    private Rigidbody2D _rigid;
    
    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    
    protected override void OnEnable()
    {
        transform.position = GameManager.Instance.player.transform.position;
        Init(damage, 40); //이동속도도 받아서 
    }

    private void Update()
    {
        _rigid.velocity = transform.up * moveSpeed;
        
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}