using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class BoomerangPojectile : Attack
{
    private Rigidbody2D _rigid;
    private bool isMove = false;
    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    protected override void OnEnable()
    {
     
        transform.position = GameManager.Instance.player.transform.position;
        Init(damage, 40); 
        
        StartCoroutine(ObjMove());;
    }
    private IEnumerator ObjMove()
    {
        _rigid.AddForce(transform.up * 30 , ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        Vector3 targetPos1 = GameManager.Instance.player.transform.position;;
        Vector3 dir = targetPos1 - transform.position;
        dir = dir.normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        _rigid.AddForce(transform.up * 60 , ForceMode2D.Impulse);
    }
    
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * Time.fixedDeltaTime, 30);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        //Spec.Bool = true;
    }
    

    
}

// public static class Spec
// {
//     public static SpecDataManager Data => SpecDataManager.Instance;
//
//     private static bool _bool;
//     public static bool Bool
//     {
//         get
//         {
//             return _bool;
//         }
//         set
//         {
//             _bool = value;
//         }
//     }
// }