using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointMove : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    int wayNum;

    void Start()
    {
        transform.position = Utility.enemyWays[wayNum];
    }

    
    void Update()
    {
        MovePath();
    }

    public void MovePath()
    {
        transform.position = Vector2.MoveTowards
            (transform.position, Utility.enemyWays[wayNum], speed * Time.deltaTime);
        
        if((Vector2)transform.position == Utility.enemyWays[wayNum])
        {
            wayNum++;
        }

        if(wayNum == Utility.enemyWays.Length)
        {
            wayNum = 0;
        }
    }
}
