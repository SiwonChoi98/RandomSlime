using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

public class BlockProjectile : Attack
{
    private Rigidbody2D _rigid;
    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    protected override void OnEnable()
    {
        int ran = Random.Range(3 * -1, 3);
        int ran2 = Random.Range(3 * -1, 3);
        Vector3 vec = new Vector3(ran, ran2, 0);
        
        transform.position = GameManager.Instance.player.transform.position + vec;
        
        
        Init(damage, 40);
        _rigid.AddForce(Vector3.up * 30, ForceMode2D.Impulse);
    }
    
}
