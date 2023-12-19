using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineProjectile : Attack
{
    private float destroyTime;
    
    
    protected override void OnEnable()
    {
        destroyTime = 4;
        int ran = Random.Range(Config.RANSKILL_POS * -1, Config.RANSKILL_POS);
        int ran2 = Random.Range(Config.RANSKILL_POS * -1, Config.RANSKILL_POS);
        Vector3 vec = new Vector3(ran, ran2, 0);
        
        transform.position = GameManager.Instance.player.transform.position + vec;
        Init(damage,0);
        StartCoroutine(DelaySetFalse());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator DelaySetFalse()
    {
        yield return new WaitForSeconds(destroyTime);
        if (gameObject.activeSelf)
        {
            Bomb();
        }
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Bomb();
        }
    }


    private void Bomb()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position,5f, LayerMask.GetMask("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                
        }
        GameObject missileHit = EffectPool.instance.Get(2, 0.3f);
        missileHit.transform.position = transform.position;
        gameObject.SetActive(false);
    }
}
