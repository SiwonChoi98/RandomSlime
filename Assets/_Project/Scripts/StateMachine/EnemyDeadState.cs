using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadState : State<Enemy>
{
    //public Animator anim;

    public override void OnInitialized() //셋팅
    {
        //anim = context.GetComponent<Animator>();
    }

    public override void OnEnter() //한번실행
    {
        //anim.SetTrigger("DoDead"); 
        
        //GameManager.instance.monsterCount--;
        
        context.GetComponent<CapsuleCollider2D>().enabled = false; //콜라이더 비활성화
        context.MoveSpeed = 0;
        context.StartCoroutine(FadOut());
        
        context.SetExpGem();
        
        GameManager.Instance.EnemyActiveCount--; //몬스터 활성화된 수 관리
        GameManager.Instance.EnemyKillCount++; //몬스터 처치 수 관리
        
        
        stateMachine.ChangeState<MonsterIdleState>(); //초기 상태로 이동

      
    }

    private IEnumerator FadOut()
    {
        float fadeCount = 1;
        while (fadeCount > 0)
        {
            fadeCount -= 0.5f;
            yield return new WaitForSeconds(0.1f);
            context.sprite.color = new Color(255, 255, 255, fadeCount);
        }
        context.gameObject.SetActive(false); //비활성화
    }
    
    public override void Update(float deltaTime) //게속업데이트
    {
        
    }

    public override void OnExit() //나가기
    {
       
    }
    
}