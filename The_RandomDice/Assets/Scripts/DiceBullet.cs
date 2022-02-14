using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBullet : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    ParticleSystem _particleSystem;
    [SerializeField]
    SerializeDiceData serializeDiceData;

    [SerializeField]
    float speed;

    public DiceData diceData => GameManager.Inst.theDice.GetDiceData(serializeDiceData.code);
    
    Enemy targetEnemy;

    public void SetupDiceBullet(SerializeDiceData serializeDiceData, Enemy targetEnemy)
    {
        this.serializeDiceData = serializeDiceData;
        this.targetEnemy = targetEnemy;
        spriteRenderer.color = diceData.color;
       var particleMain = _particleSystem.main;
       particleMain.startColor = diceData.color;
   
        StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        while(true)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetEnemy.transform.position, speed * Time.deltaTime);
            yield return null;

            if((transform.position - targetEnemy.transform.position).sqrMagnitude < speed * Time.deltaTime * speed * Time.deltaTime)
            {
                transform.position = targetEnemy.transform.position;
                break;
            }
        }

        //데미지 준다
        
        int totalAttackDamage = Utility.TotalAttackDamage(diceData.basicAttackDamage, serializeDiceData.level);

        if (targetEnemy != null)
        {
            targetEnemy.Damaged(totalAttackDamage);
        }

        Die();
    }

    void Die()
    {
        spriteRenderer.enabled = false;
        _particleSystem.Play();
    }

    //IEnumerator GoObject(Vector3 startPosition, Transform targetObject)
    //{
    //    float finishTime = 1f;
    //    for(float runningTime = 0; runningTime < finishTime; runningTime += Time.deltaTime)
    //    {
    //        this.transform.position = Vector3.Lerp(startPosition, targetObject.position, runningTime);
    //        yield return null;
    //    }
    //    this.transform.position = targetObject.position;
    //}

    void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
