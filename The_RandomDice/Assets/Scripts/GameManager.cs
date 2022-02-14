using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    void Awake() => Inst = this;

    public TheDice theDice;

    [SerializeField]
    Vector2[] originalDicePositions;

    [SerializeField]
    SerializeDiceData[] serializeDiceDatas; //모든 주사위의 정보를 직렬화 해준다
    public List<Enemy> enemies;

    private void Update()
    {
        Invoke("StartWaveCo", 2.0f);
           
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            StartCoroutine(StartWaveCo());
        }
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     //  
        //  
        //}

        //  Invoke("SpawnEnemy", 2.0f);

        //   SpawnEnemy();
    }

    public bool TryRandomSpawn(int level = 1)
    {
        //비어 있는 사각형 배열에서 랜덤하게 찾아 생성 
        var emptySerializeDiceDatas = Array.FindAll(serializeDiceDatas, x => x.isFull == false);

        if(emptySerializeDiceDatas.Length <= 0)
        {
            return false;
        }

        int randIndex = emptySerializeDiceDatas[Random.Range(0,emptySerializeDiceDatas.Length)].index;
        
        Vector3 randPos = originalDicePositions[randIndex];
        var randDiceData = theDice.GetRandomDiceData();
        var dice = ObjectPooler.SpawnFromPool("dice", randPos, Utility.QI).GetComponent<Dice>();

        var serializeDiceData = new SerializeDiceData(randIndex, true, randDiceData.code, level);
        dice.SetUpSlot(serializeDiceData);
        serializeDiceDatas[randIndex] = serializeDiceData;

        return true;
    }

    public void SpawnBtnClick() => TryRandomSpawn();

    public Vector2 GetOriginDicePositions(int index) => originalDicePositions[index];

    public GameObject[] GetRayCastAll(int layerMask)
    {

        var mousePos = Utility.MousePos;
        mousePos.z = -100f;
        var raycastHit2D = Physics2D.RaycastAll(mousePos, Vector3.forward, float.MaxValue, 1 << layerMask);
        var results = Array.ConvertAll(raycastHit2D, x => x.collider.gameObject);

        for (int i = 0; i < results.Length; i++)
        {
            print(results[i].name);
        }

        return results;
    }

   IEnumerator StartWaveCo()
    {
        print("웨이브 시작");

        for(int i =0; i < 10; i++)
        {
            SpawnEnemy();
            yield return Utility.delayWave;
        }

        print("웨이브 종료");
    }

    void SpawnEnemy()
    {
        var enemyObj = ObjectPooler.SpawnFromPool("enemy", Utility.enemyWays[0], Utility.QI);
        enemies.Add(enemyObj.GetComponent<Enemy>());
    }

    void SpawnOppositeEnemy()
    {
        var enemyObj = ObjectPooler.SpawnFromPool("enemy", Utility.oppositeEnemyWays[0], Utility.QI);
        enemies.Add(enemyObj.GetComponent<Enemy>());
    }

    void ArrangeEnemies()
    {
        //거리가 작은게 오더 낮고, 거리 큰게 오더 높음
        enemies.Sort((x, y) => x.distance.CompareTo(y.distance));
    
        for(int i =0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Order>().SetOrder(i);
        }
    }

    public Enemy GetRandomEnemy()
    {
        if (enemies.Count <= 0)
            return null;

        return enemies[Random.Range(0, enemies.Count)];
    }

}
