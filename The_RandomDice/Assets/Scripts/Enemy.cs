using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    TMP_Text healthTMP;

    [SerializeField]
    float speed = 1f;
    [SerializeField]
    int health;
    int wayNum;

    public int Health { 
        get => health;
        set
        {
            health = value;
            healthTMP.text = value.ToString();
        }
            }
    public float distance;


    public void Damaged(int damage)
    {
        //피통 관련
        Health -= damage;
        Health = Mathf.Max(0, Health);

        if(Health <= 0)
        {
            //사망
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(MovePathCo());
        
    //    StartCoroutine(OppositeMovePathCo());

      //  speed = Random.Range(0.5f, 2.0f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
      // if(Input.GetKeyDown(KeyCode.Space))
        {
            Damaged(3);
        }
    }

   IEnumerator MovePathCo()
    {
        while(true)
        {
            transform.position = Vector2.MoveTowards
          (transform.position, Utility.enemyWays[wayNum], speed * Time.deltaTime);
            distance += speed * Time.deltaTime;
            if ((Vector2)transform.position == Utility.enemyWays[wayNum])
            {
                wayNum++;
            }

            if (wayNum == Utility.enemyWays.Length)
            {
                gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }      
    }

    IEnumerator OppositeMovePathCo()
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards
          (transform.position, Utility.oppositeEnemyWays[wayNum], speed * Time.deltaTime);
            distance += speed * Time.deltaTime;
            if ((Vector2)transform.position == Utility.oppositeEnemyWays[wayNum])
            {
                wayNum++;
            }

            if (wayNum == Utility.oppositeEnemyWays.Length)
            {
                gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }


    void OnDisable()
    {
        distance = 0;
        wayNum = 0;
        GameManager.Inst.enemies.Remove(this);
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
