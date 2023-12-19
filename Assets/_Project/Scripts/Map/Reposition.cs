using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    //private Monster _monster;
    [SerializeField] private int moveSize;
    void Awake()
    {
        //_monster = gameObject.GetComponent<Monster>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }
        
        Vector2 playerPos = GameManager.Instance.player.transform.position;
        Vector2 myPos = transform.position; 
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffy = Mathf.Abs(playerPos.y - myPos.y);

        float dirX = playerPos.x < myPos.x ? -1 : 1;
        float dirY = playerPos.y < myPos.y ? -1 : 1;
        switch (gameObject.transform.tag)
        {
            case "Floor":
                if (diffX > diffy) { transform.position += Vector3.right * dirX * moveSize; }
                else if (diffX < diffy) { transform.position += Vector3.up * dirY * moveSize; }
                break;
            // case "Enemy":
            //     if (_monster.MonsterCurHealth > ((_monster.MonsterMaxHealth / 10) * 9)) { transform.Translate(playerDir * 70 + new Vector3(Random.Range(-3, 3f), 0f, Random.Range(-3f, 3f)), Space.World); } //Ã¼·ÂÀÌ 90ÆÛ ÀÌÇÏ¸é ÀÌµ¿x 
            //     else { return; }
            //     break;
        }
        
    }
}