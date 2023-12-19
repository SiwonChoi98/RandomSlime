using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


public class Player : MonoBehaviour, IDamageable
{
    
    [Header("캐릭터 스탯")] 
    [SerializeField] private string _name; //이름 
    [SerializeField] private float _moveSpeed; //이동속도
    [SerializeField] private float _curHealth; //현재체력
    [SerializeField] private float _maxHealth; //최대체력
    [SerializeField] private float _defaultDamage; //공격력
    [SerializeField] private float _defaultDefensive; //방어력
    [SerializeField] private int _level; //레벨
    [SerializeField] private float _curExp; //현재 경험치
    [SerializeField] private float _maxExp; //최대 경험치
    [SerializeField] private float _defaultManeticRange; //기본 자석 거리

    [Header("패시브 관련")]
    public float addCoolTime; //추가 쿨타임 (전기증폭기)
    public float addDamage; //추가 데미지 
    public float addExp; //추가 경험치
    public float addCritical; //추가 크리티컬 확률
    public float CurHealth { get => _curHealth; set => _curHealth = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    public float CurExp { get => _curExp; set => _curExp = value; }
    public float MaxExp {get => _maxExp; set => _maxExp = value;}
    public int Level { get => _level; set => _level = value;}
    public float DefaultDamage { get => _defaultDamage; set => _defaultDamage = value;}
    
    public float DefaultMagneticRange {get => _defaultManeticRange; set => _defaultManeticRange = value;}
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    [Header("-----------------------------------------------------------------------------------------------------")]
    
    [Header("조이스틱")]
    [SerializeField] private FloatingJoystick joy;
    
    [Header("키보드")]
    public Vector2 moveVec;
    private float _hAxis;
    private float _vAxis;
    
    [Header("컴포넌트")] 
    private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _weaponSprite;
    public Rigidbody2D _rigid;
    public EquipWeapon equipWeapon;
    public Animator Animator;

    //물총 이미지
    public GameObject GunImage;
    ///유니티 내 메서드
    #region UnityMethod

    private void Awake()
    {
        ComponentSetting();
    }
    private void FixedUpdate()
    {
        Move_Joystick();
    }
    
    #endregion

    ///유저 스탯 셋팅
    public void UserSetting(string name,float speed, float curHealth, float maxHealth, float defaultDamage,
        float defaultDefensive, int level, int curExp, int maxExp, float magneticRange)
    {
        _name = name;
        _moveSpeed = speed;
        _curHealth = curHealth;
        _maxHealth = maxHealth;
        _defaultDamage = defaultDamage;
        _defaultDefensive = defaultDefensive;
        _level = level;
        _curExp = curExp;
        _maxExp = maxExp;
        _defaultManeticRange = magneticRange;
    }

    //컴포넌트 셋팅
    private void ComponentSetting()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        equipWeapon = GetComponentInChildren<EquipWeapon>();
        Animator = GetComponent<Animator>();
        joy = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();
    }
    ///<summary>
    /// 조이스틱 이동
    ///</summary>
    private void Move_Joystick()
    {
        float x = joy.Horizontal;
        float y = joy.Vertical;
        moveVec = new Vector2(x, y).normalized;

        _rigid.MovePosition(_rigid.position + moveVec * (_moveSpeed * Time.fixedDeltaTime));
        
        SpriteFlip();

        //
        // if (moveVec.x != 0)
        // {
        //     Animator.SetBool("IsMove", true);
        // }
        // else
        // {
        //     Animator.SetBool("IsMove", false);
        // }
        
        
        // //이동 애니메이션
        bool isMoveAnim = moveVec.x != 0 ? true : false;
        Animator.SetBool("IsMove", isMoveAnim);
        
        
    }
    
    
    private void SpriteFlip()
    {
        if (moveVec.x != 0)
        {
            _sprite.flipX = moveVec.x > 0;
            _weaponSprite.flipX = moveVec.x > 0;

            int gunPos = moveVec.x < 0 ? -2 : 2;
            GunImage.transform.position = transform.position + new Vector3(gunPos, 0, 0);
            
        }
    }

    
    public virtual void TakeDamage(float damage)
    {
        _curHealth -= damage;
        StartCoroutine(HitColor());
        Debug.Log("데미지 받음");
    }

    private IEnumerator HitColor()
    {
        _sprite.color = new Color(150, 0, 0, 1);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(255, 255, 255, 1);
    }

   
}
