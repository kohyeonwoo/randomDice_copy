using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Dice : MonoBehaviour
{
    [Header("컴포넌트")]
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Order order;

    [Header("변수값들")]
    public SerializeDiceData serializeDiceData;
    [SerializeField]
    Transform[] dots;

    public DiceData diceData => GameManager.Inst.theDice.GetDiceData(serializeDiceData.code);

    public void SetUpSlot(SerializeDiceData serializeDiceData)
    {
        this.serializeDiceData = serializeDiceData;
        var diceData = GameManager.Inst.theDice.GetDiceData(serializeDiceData.code);  
        spriteRenderer.sprite = diceData.sprite;
        SetDots(serializeDiceData.level);

        for(int i =0; i < Utility.MAX_DICE_LEVEL; i++)
        {
            dots[i].GetComponent<SpriteRenderer>().color = diceData.color;
        }

        if(serializeDiceData.code == 0)
        {
            gameObject.SetActive(false);
        }

        if(gameObject.activeSelf)
        {
            StartCoroutine(AttackCo());
        }
        
    }

    public void SetDots(int level)
    {

        for(int i =0; i < Utility.MAX_DICE_LEVEL; i++)
        {
            dots[i].gameObject.SetActive(i < level);
        }

        //점들의 위치 조정
        Vector2[] position = new Vector2[1];

        switch (level)
        {
            case 1: 
                position = new Vector2[] { Vector2.zero }; break;
            case 2:
                position = new Vector2[] { new Vector2(-0.35f, -0.33f), new Vector2(0.35f, 0.41f) }; break;
            case 3:
                position = new Vector2[] { new Vector2(-0.35f, -0.33f), Vector2.zero ,new Vector2(0.35f, 0.41f) }; break;
            case 4:
                position = new Vector2[] { new Vector2(-0.35f, -0.33f), new Vector2(-0.35f, 0.33f), new Vector2(0.35f, -0.33f), new Vector2(0.35f, 0.41f) }; break;
            case 5:
                position = new Vector2[] { new Vector2(-0.35f, -0.33f), new Vector2(-0.35f, 0.33f), Vector2.zero ,new Vector2(0.35f, -0.33f), new Vector2(0.35f, 0.41f) }; break;
            case 6:
                position = new Vector2[] { new Vector2(-0.35f, -0.33f), new Vector2(-0.35f, 0f) , new Vector2(-0.35f, 0.33f), new Vector2(0.33f, -0.33f), new Vector2(0.33f, 0f), new Vector2(0.35f, 0.41f) }; break;
        }

        for(int i =0; i < position.Length; i++)
        {
            dots[i].localPosition = position[i];
        }
    }

    public void OnMouseDown()
    {
        order.SetMostFrontOrder(true);
    }

    public void OnMouseDrag()
    {
        transform.position = Utility.MousePos;
    }

    public void OnMouseUp()
    {   
        MoveTransform(GameManager.Inst.GetOriginDicePositions(serializeDiceData.index), true, 0.2f,
            () => order.SetMostFrontOrder(false));

        //같은 코드와 같은 레벨이면 합쳐짐(머지)
        GameObject[] raycastAll = GameManager.Inst.GetRayCastAll(Utility.DICE_LAYER);
        GameObject targetDiceObj = Array.Find(raycastAll, x => x.gameObject != gameObject);

        if(targetDiceObj != null)
        {
            var targetDice = targetDiceObj.GetComponent<Dice>();

            if(serializeDiceData.code == targetDice.serializeDiceData.code &&
                serializeDiceData.level == targetDice.serializeDiceData.level)
            {
                int nextLevel = serializeDiceData.level + 1;
                if(nextLevel > Utility.MAX_DICE_LEVEL)
                {
                    return;
                }

                var targetSerializeDiceData = targetDice.serializeDiceData;
               // targetSerializeDiceData.code = GameManager.Inst.theDice.GetRandomDiceData().code;
                targetSerializeDiceData.level = nextLevel;
                //targetSerializeDiceData.isFull = false;
                targetDice.SetUpSlot(targetSerializeDiceData);
             
                gameObject.SetActive(false);
            }
        }
    }


    void MoveTransform(Vector2 targetPos, bool useDotween, float duration = 0f, TweenCallback action = null)
    {
        if(useDotween)
        {
            transform.DOMove(targetPos, duration).OnComplete(action);
        }else
        {
            transform.position = targetPos;
        }
    }

    IEnumerator AttackCo()
    {
        while (true)
        {
            Enemy targetEnemy = GameManager.Inst.GetRandomEnemy();

            if (targetEnemy != null)
            {
                var diceBulletObj = ObjectPooler.SpawnFromPool("diceBullet", dots[0].position, Utility.QI);
                diceBulletObj.GetComponent<DiceBullet>().SetupDiceBullet(serializeDiceData, targetEnemy);
            }
            yield return Utility.delayDiceBulletSpawn;
        }  
    }

    void OnDisable()
    {
        serializeDiceData.isFull = false;
        serializeDiceData = null;
        spriteRenderer.sprite = null;
        SetDots(0);

        for(int i = 0; i < Utility.MAX_DICE_LEVEL; i++)
        {
            dots[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
        
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
