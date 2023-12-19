using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : State<Enemy>
{
    public Animator anim;

    public override void OnInitialized() //셋팅
    {
        anim = context.GetComponent<Animator>();
    }

    public override void OnEnter() //한번실행
    {
        //context.isAttackRange = false;
    }

    public override void Update(float deltaTime) //게속업데이트
    {
        //타겟이 있으면서 히트상태가 아닐때 
        if (context.target  && !context.isHit) 
        {
            stateMachine.ChangeState<MonsterMoveState>();
            return;
        }
        
        if (context.isHit)
        {
            stateMachine.ChangeState<MonsterHitState>();
            return;
        }
    }

    public override void OnExit() //나가기
    {
        //anim?.SetBool("isIdle", false);
    }
}