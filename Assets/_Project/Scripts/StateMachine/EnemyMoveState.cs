using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveState : State<Enemy>
{
    //public Animator anim;

    public override void OnInitialized() //셋팅
    {
        //anim = context.GetComponent<Animator>();
    }

    public override void OnEnter() //한번실행
    {
        //anim?.SetBool("IsMove", true);
    }

    public override void Update(float deltaTime) //게속업데이트
    {
        if (context.isHit)
            stateMachine.ChangeState<MonsterHitState>();
        
        //타겟이 있으면 공격타입에 따라서 행동
        if (context.target)
        {
            context.GetAttackType();
        }
        //타겟이 없으면 idle 상태로
        else 
        {
            stateMachine.ChangeState<MonsterIdleState>();
        }
        
        // //보스 스테이지 진입 시 노말 몬스터는 전부 Dead
        // if (GameManager.Instance.IsBoss && context.monster.monster_type == MonsterType.NORMAL)
        // {
        //     stateMachine.ChangeState<MonsterDeadState>();
        // }
    }

    public override void OnExit() //나가기
    {
        //anim?.SetBool("IsMove", false);
    }
}