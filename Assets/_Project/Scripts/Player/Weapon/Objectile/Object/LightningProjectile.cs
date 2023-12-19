using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningProjectile : Attack
{
    private float destroyTime;
    
    
    protected override void OnEnable()
    {
        destroyTime = 1;
        SoundManager.Instance.SfxPlaySound("Lightning");
        Init(damage,0);
        StartCoroutine(DelaySetFalse());
    }

    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Bomb();
        }
    }
    private IEnumerator DelaySetFalse()
    {
        yield return new WaitForSeconds(destroyTime);
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void Bomb()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position,5f, LayerMask.GetMask("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                
        }
        GameObject missileHit = EffectPool.instance.Get(3, 0.3f);
        missileHit.transform.position = transform.position;
    }
}
