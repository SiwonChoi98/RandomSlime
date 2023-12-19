using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Exp : MonoBehaviour
{
    public float _exp; //경험치량
    [SerializeField] protected float _magnetDistance; //자석 거리
    private Rigidbody _rigid;
    private Player _player;
    
    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); //자석관련
        _magnetDistance = GameManager.Instance.player.DefaultMagneticRange;
    }
    protected virtual void Init(float exp)
    {
        _exp = exp;
    }

    protected virtual void Update()
    {
        //exp 자석
        if (gameObject.activeSelf)
        {
            _magnetDistance = 99;
            float distance = Vector3.Distance(_player.transform.position, transform.position);
            if (distance < _magnetDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 40f * Time.deltaTime);
            }
        }
        
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().CurExp += _exp + GameManager.Instance.player.addExp;
            GameManager.Instance.LevelUp(); //레벨업 체크
            gameObject.SetActive(false);
        }
    }
    
    
}
