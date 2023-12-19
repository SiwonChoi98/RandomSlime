using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MysticArsenal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum AttackType
{
    MELEE,
    RANGE
}
public class Enemy : MonoBehaviour, IDamageable
{
    public AttackType AttackType;
    [SerializeField] protected int _id; //몬스터 이름
    [SerializeField] protected float _moveSpeed; //이동속도
    [SerializeField] protected float _curHealth; //현재체력
    [SerializeField] protected float _maxHealth; //최대체력
    [SerializeField] protected float _defaultDamage; //공격력
    
    [Header("원거리 몬스터 전용")]
    protected float _defaultAttackRange;
    protected bool _isAttack;
    protected float _attackTime;
    protected float _initAttackTime;
    public float CriticalValue;
    public float CurHealth { get => _curHealth; set => _curHealth = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }

    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    
    [FormerlySerializedAs("Sprite")]
    [Header("---------------------------------------------------------------------------------------------")]
    
    [Header("컴포넌트")] 
    public SpriteRenderer sprite;

    public CapsuleCollider2D CapsuleCollider2D;
    public Rigidbody2D rigid;
    public Animator anim;
    [Header("타겟 관련")]
    public Transform target; //공격할 타겟
    
    //상태
    public bool isShieldDamage = true;
    public bool isHit = false;
    
    public float curShieldTime = 3; //현재 쉴드데미지 타임
    public float maxShieldTime = 3; //현재 쉴드데미지 타임

    public Monster monster;
    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    [Header("상태")] 
    protected StateMachine<Enemy> stateMachine; //상태머신
    
    protected virtual void Start()
    {
        CapsuleCollider2D.enabled = true;
        target = GameObject.FindWithTag("Player").transform;
        CriticalValue = SpecDataManager.Instance.GameConfig.Get(3013).value;
        //상태머신 삽입
        AddState();
    }

    protected virtual void OnEnable()
    {
        sprite.color = new Color(255, 255, 255, 1);
        CapsuleCollider2D.enabled = true;
        //Init();
        
        //상태머신 삽입
        AddState();
    }

    //모든 상태들 삽입
    private void AddState()
    {
        stateMachine = new StateMachine<Enemy>(this, new MonsterIdleState());
        stateMachine.AddState(new MonsterMoveState());
        stateMachine.AddState(new MonsterDeadState());
        stateMachine.AddState(new MonsterHitState());
    }
    
    public virtual void SetMonster(Monster _monster)
    {
        monster = _monster;
        Init();
    }
    //웨이브 별 추가 체력,공격력 곱연산
    protected float AddMultiple()
    {
        return GameManager.Instance.EnemyDatas[GameManager.Instance.StageIndex].multiple;
    }
    protected virtual void Init()
    {
        _id = monster.id;
        _moveSpeed = monster.move_speed;
        _curHealth = monster.hp * AddMultiple();
        _maxHealth = monster.hp * AddMultiple();
        _defaultDamage = monster.atk * AddMultiple();
        
        if(GameManager.Instance.DicSprites[_id])
            sprite.sprite = GameManager.Instance.DicSprites[_id];

        AttackType = (AttackType)monster.atk_type;
        
        //test
        curShieldTime = 1;
        maxShieldTime = 1;

        _attackTime = 5;
        _initAttackTime = 5;
        _defaultAttackRange = 8;
    }
    protected void FixedUpdate()
    {
        //상태 계속 지속
        stateMachine.Update(Time.fixedDeltaTime);
    }

    protected void Update()
    {
        SpriteFlip();

        //쉴드 데미지 시간계산
        ShieldDamageTime();
        
        //원거리 몬스터 공격 시간
        AttackCoolTime();
    }
    private void SpriteFlip()
    {
        if (this.gameObject.activeSelf)
        {
            if (target.position.x > rigid.position.x)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
             
        }
    }

    public int TextDamage;
    public bool isCri;
    public void DamageText(float damage)
    {
        DamageTextPool.instance.Get(0, damage, transform, isCri);
    }
    
    //데미지 받는 곳
    public virtual void TakeDamage(float damage)
    {
        int intDamage = GetDamage(damage);//(int)Math.Round(damage)
        _curHealth -= intDamage;
        isHit = true;
        
        SoundManager.Instance.SfxPlaySound("MonsterHit", 0.03f);
        
        TextDamage = intDamage;

    }

    //데미지 조정
    private int GetDamage(float damage)
    {
        float random = Random.Range(0, 100);
        int intDamage = 0;
        CriticalValue += GameManager.Instance.player.addCritical;
        //크리티컬 체크
        if (random < CriticalValue)
        {
            isCri = true;
            intDamage = (int)(damage * SpecDataManager.Instance.GameConfig.Get(3014).value);
        }
        else
        {
            isCri = false;
            intDamage = (int)Math.Round(damage);
        }
        return intDamage;
    }
    //데미지 주는 곳
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(_defaultDamage);
        }
    }
    
    //독가스 방아막 데미지 받을 수 있는 상태인지 체크
    public void ShieldDamageTime()
    {
        if (!isShieldDamage)
        {
            if (curShieldTime > 0)
            {
                curShieldTime -= Time.deltaTime;
                if (curShieldTime <= 0)
                {
                    curShieldTime = maxShieldTime;
                    isShieldDamage = true;
                }
            }
        }
    }

    //몬스터 죽을 때 떨어질 경험치
    //이 부분 굳이 오브젝트를 나눌 필요없이 데이터와 이미지만 바꿔주면 된다.
    
    //해당 몬스터의 rewardGroup에 있는 id를 RewardGroup에 있는 id를 찾고
    //해당 RewardType에 이름과 GameConfig에 이름을 찾아서 그 value를 리턴해준다.
    //이렇게 할 시 굳이 3개로 나누지 않고 1개로 생성 시켜도 안에 데이터 값만 바꾸기 때문에 오브젝트가 훨씬 절약 된다.
    //이미지는 스프라이트 네임이라는 컬럼을 만들어서 Resources로 찾아온다.
    public void SetExpGem()
    {
        GameObject exp = null;
        if (monster.reward_group_id == SpecDataManager.Instance.RewardGroup.Get(20001).id)
        {
            exp = ExpPool.instance.Get(0);
        }
        else if (monster.reward_group_id == SpecDataManager.Instance.RewardGroup.Get(20002).id)
        {
            exp = ExpPool.instance.Get(1);
        }
        else if (monster.reward_group_id == SpecDataManager.Instance.RewardGroup.Get(20003).id)
        {
            exp = ExpPool.instance.Get(2);
        }
        exp.transform.position = transform.position;
    }

    public void GetAttackType()
    {
        if (AttackType == AttackType.MELEE)
        {
            rigid.MovePosition(Vector2.MoveTowards(transform.position, target.position, (Time.fixedDeltaTime * MoveSpeed)));
        }
        else if(AttackType == AttackType.RANGE)
        {
            if (AttackDistanceCheak())
            {
                rigid.velocity = Vector2.zero;
                if (_isAttack)
                {
                    Fire();
                    _isAttack = false;
                }
            }
            else
            {
                rigid.MovePosition(Vector2.MoveTowards(transform.position, target.position, (Time.fixedDeltaTime * MoveSpeed)));
            }
            
        }   
        // else if ()
        // {
        //     
        // }
    }

    #region 원거리 몬스터 공격

    //공격 사거리 체크
    public bool AttackDistanceCheak()
    {
        Vector3 thisPos = transform.position;
        Vector3 targetPos = target.position;

        thisPos.y = 0;
        targetPos.y = 0;
        
        float _distance = Vector3.Distance(thisPos, targetPos); // 나중에 플레이어와 몬스터의 키를 弧娩. 완료
        
        if (_distance < _defaultAttackRange) // 만약 몬스터가 공격 사거리 내로 들어왔다면,
        {
            return true;
        }
        return false;
        
    }

    //공격가능한 상태인지
    public void AttackCoolTime()
    {
        if (!_isAttack)
        {
            _attackTime -= Time.deltaTime;
            if (_attackTime <= 0)
            {
                _attackTime = _initAttackTime;
                _isAttack = true;
            }
        }
    }

    //원거리 몬스터 공격
    private void Fire()
    {
        GameObject projectile = SkillObjectPool.instance.Get("Prefabs/Projectile/RangeMonsterProjectile");
        projectile.transform.position = transform.position;
        Vector3 targetPos1 = target.position;
        Vector3 dir = targetPos1 - transform.position;
        dir = dir.normalized;
        
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    #endregion
   

}
