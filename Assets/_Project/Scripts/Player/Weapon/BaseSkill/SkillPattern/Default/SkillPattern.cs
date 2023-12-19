using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public static class Config
{
    public const int SEARCH_TIME = 1;
    public const int RANSKILL_POS = 10;
    public const int SEARCH_RANGE = 20;
}
public class SkillPattern : MonoBehaviour
{
    [Header("타겟 관련")]
    public Transform nearTarget; //공격할 타겟
    public Sprite sprite; //해당 무기 이미지
    [SerializeField] protected LayerMask _targetMask; //Enemy 레이어 마스크만 공격할수 있게
    [SerializeField] protected float _searchRange; //Enemy 찾는 거리 
    protected float _searchTime; //Enemy를 몇초마다 찾을지
    protected float _initSearchTime; //서치 타임 초기화
    public int SkillId;//스킬 종류 번호
    public float damage; //데미지
    public int skillLevel; //스킬 레벨
    public float coolTime; //스킬 쿨타임
    public float initCoolTime; //스킬 쿨타임 초기화
    
    public SkillLevel SkillLevel;

    public RaycastHit2D[] targets;
    protected virtual void Init()
    {
        _searchRange = Config.SEARCH_RANGE;
        _searchTime = Config.SEARCH_TIME;
        coolTime = SkillLevel.skill_cooltime;
        initCoolTime = coolTime;
        SkillId = SkillLevel.skill_id;
        skillLevel = SkillLevel.level;
        damage = (GameManager.Instance.player.DefaultDamage * SkillLevel.base_damage_rate);
        Sprite sp = Resources.Load<Sprite>(SkillLevel.sprite);
        sprite = sp;
    }
    
    public virtual void SetSkillLevel(SkillLevel skillLevel)
    {
        SkillLevel = skillLevel;
        Init();
    }
    
    protected virtual void FixedUpdate()
    {
        //TODO 성능이슈 날수있어서 나중에 다시 봐야함
        targets = Physics2D.CircleCastAll(GameManager.Instance.player.transform.position, _searchRange, Vector2.zero, 0 , _targetMask);
        nearTarget = GetNearest();
    }

    //타겟 찾기
    protected virtual Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = GameManager.Instance.player.transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
    
    //스킬 오브젝트 생성
    protected virtual GameObject GetObject()
    {
        if (nearTarget != null || SkillLevel.prj_type == PrjType.BOMB)
        {
            return SkillObjectPool.instance.Get(SkillLevel.projectile_name);
        }
        return null;
    }
    
    //스킬 오브젝트 생성 후 초기화를 해당 레벨 따라서 스킬 개수만큼 생성
    private IEnumerator CountAttack()
    {
        for (int i = 0; i < SkillLevel.base_obj_count-1; i++)
        {
            yield return new WaitForSeconds(0.1f);
            
            StartAttack();
        }
    }
    
    //공격
    public virtual void Fire()
    {
        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            if (coolTime <= 0)
            {
                StartAttack();
                StartCoroutine(CountAttack());
                
                //번개 구름일 때
                if (SkillLevel.prj_type == PrjType.LIGHTNING)
                {
                    coolTime = initCoolTime - GameManager.Instance.player.addCoolTime; 
                }
                else
                {
                    coolTime = initCoolTime;
                }
                
            }
        }
    }

    //스킬 오브젝트 생성 후 초기화 
    public virtual void StartAttack()
    {
        GameObject ob = GetObject();
        if (ob)
        {
            if (nearTarget && SkillLevel.prj_type != PrjType.SHIELD && SkillLevel.prj_type != PrjType.BOMB)
            {
                ob.transform.LookAt(nearTarget.position);
            }
            Attack attackComponent = ob.GetComponent<Attack>();
            SetAttackData(attackComponent);
            SetAttackRotation(attackComponent);
            GetSoundCheck();
            
        }
    }

    //사운드 및 공격에 필요한 데이터 초기화
    private void GetSoundCheck()
    {
        switch (SkillLevel.prj_type)
        {
            case PrjType.BUBBLE:
                SoundManager.Instance.SfxPlaySound("BubbleBubble", 0.05f);
                
                Animator anim = GameManager.Instance.player.GunImage.GetComponent<Animator>();
                anim.SetTrigger("DoShot");
                break;
            default:
                break;
        }
    }
    
    public virtual void SetAttackData(Attack ob)
    {
        ob.damage = damage;
        ob.transform.localScale = new Vector3(SkillLevel.prj_scale, SkillLevel.prj_scale, 1);
        
        if (SkillLevel.prj_type == PrjType.LIGHTNING)
        {
            ob.transform.position = nearTarget.position;
            
        }
        
    }

    public virtual void SetAttackRotation(Attack ob)
    {
        if (SkillLevel.prj_type != PrjType.SHIELD && SkillLevel.prj_type != PrjType.BOMB
            && SkillLevel.prj_type != PrjType.LIGHTNING)
        {
            Vector3 targetPos1 = nearTarget.position;
            Vector3 dir = targetPos1 - GameManager.Instance.player.transform.position;
            dir = dir.normalized;
            ob.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            
        }
       
    }
    
    
    protected virtual void Update()
    {
        Fire();
    }
}
