using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BubbleBubblePojectile5 : Attack
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
        
        //transform.Rotate(new Vector3(15 * Time.deltaTime,0,300* Time.deltaTime));
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