using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDronePojectile : Attack
{
    private Rigidbody2D _rigid;
    
    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable()
    {
        transform.position = GameManager.Instance.player.transform.position;
        Init(damage, 40);
        StartCoroutine(Delay());
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.RotateAround(GameManager.Instance.player.transform.position, Vector3.back, 150 * Time.deltaTime); //
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