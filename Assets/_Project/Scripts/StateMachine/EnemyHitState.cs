using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHitState : State<Enemy>
{
    public Animator anim;

    public override void OnInitialized() //셋팅
    {
        anim = context.GetComponent<Animator>();
    }

    public override void OnEnter() //한번실행
    {
        context.isHit = false;
        context.StartCoroutine(HitColor());
        
        
        GameObject hit = EffectPool.instance.Get(1, 0.1f);
        hit.transform.position = context.transform.position;

        context.DamageText(context.TextDamage);
    }

    
    
    public override void Update(float deltaTime) //게속업데이트
    {
        if (!context.isHit)
        {
            if (context.CurHealth > 0) //현재 체력이 0보다 높으면 idle로 돌아가고
            {
                stateMachine.ChangeState<MonsterIdleState>();
            }   
        }

        if (context.CurHealth <= 0)
        {
            stateMachine.ChangeState<MonsterDeadState>();
        }
        
    }

    private IEnumerator HitColor()
    {
        context.sprite.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.05f);
        context.sprite.color = new Color(255, 255, 255, 1);
    }

    public override void OnExit() //나가기
    {
        context.isHit = false;
        //context.sprite.color = new Color(255, 255, 255, 1);
    }
}