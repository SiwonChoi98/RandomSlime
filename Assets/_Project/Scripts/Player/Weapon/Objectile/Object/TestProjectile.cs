using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestProjectile : MonoBehaviour
{
    private Rigidbody2D _rigid;
    
    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private Vector2 dir;
    private float x;
    private float y;
    private void Start()
    {
        
        x = Random.Range(-10f, 10f);
        y = Random.Range(-10f, 10f);
        
        dir = new Vector2(x, y).normalized;
        //_rigid.AddForce(dir * 100, ForceMode2D.Impulse);
    }
    
    protected void OnEnable()
    {
        //Init(3, 10);
    }
    
    // protected void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Enemy"))
    //     {
    //         other.gameObject.GetComponent<Enemy>().TakeDamage(20);
    //         //gameObject.SetActive(false);
    //     }
    //     
    // }

    private IEnumerator DelayZero()
    {
        _rigid.AddForce(dir * 30, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        _rigid.velocity = Vector2.zero;
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DelayZero());
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(20);
        }
        
        if (other.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(DelayZero());
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        
    }

    
}