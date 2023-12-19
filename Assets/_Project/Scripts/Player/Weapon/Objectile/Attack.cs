using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


public class Attack : MonoBehaviour
{
    //기본공격 및 스킬 베이스
    public float damage; //스킬공격력
    public float moveSpeed; //스킬 이동속도

    protected virtual void Start()
    {
        transform.position = GameManager.Instance.player.transform.position;
    }

   
    protected virtual void OnEnable()
    {
    }

    protected virtual void Init(float _damage, float _moveSpeed)
    {
        damage = _damage;
        moveSpeed = _moveSpeed;
    }
    //데미지 
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
        
    }
    
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Area"))
        {
            gameObject.SetActive(false);
        }
    }
    
}