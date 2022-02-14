using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SerializeDiceData
{
    public int index;
    public bool isFull;
    public int code;
    public int level;

    public SerializeDiceData(int index, bool isFull ,int code, int level)
    {
        this.index = index;
        this.isFull = isFull;
        this.code = code;
        this.level = level;
    }
}


public class Utility
{
    public const int MAX_DICE_LEVEL = 6;

    public static readonly Quaternion QI = Quaternion.identity;

    public static Vector3 MousePos
    {
        get
        {
            var result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.z = 0;
            return result;
        }
    }

    public static GameObject[] GetRayCastAll(int layerMask)
    {
        var mousePos = MousePos;
        mousePos.z = -100f;
        var raycastHit2D = Physics2D.RaycastAll(mousePos, Vector3.forward, float.MaxValue, 1 << layerMask);
        return Array.ConvertAll(raycastHit2D, x => x.collider.gameObject);
    }

    //public static Vector2[] GetDotPositions(int level) => 
    // level switch
    //{
    //  
    //        1=>new Vector2[] { Vector2.zero},
    //        2 =>new Vector2[] { new Vector2(-0.35f, -0.33f), new Vector2(0.35f, 0.41f) },
    //        3 =>new Vector2[] { new Vector2(-0.35f, -0.33f), Vector2.zero, new Vector2(0.35f, 0.41f) },
    //        4 =>new Vector2[] { new Vector2(-0.35f, -0.33f), new Vector2(-0.35f, 0.33f), new Vector2(0.35f, -0.33f), new Vector2(0.35f, 0.41f) },
    //        5 =>new Vector2[] { new Vector2(-0.35f, -0.33f), new Vector2(-0.35f, 0.33f), Vector2.zero, new Vector2(0.35f, -0.33f), new Vector2(0.35f, 0.41f) },
    //        6 =>new Vector2[] { new Vector2(-0.35f, -0.33f), new Vector2(-0.35f, 0f), new Vector2(-0.35f, 0.33f), new Vector2(0.33f, -0.33f), new Vector2(0.33f, 0f), new Vector2(0.35f, 0.41f) },
    //        _ => new Vector2[] { Vector2.zero}
    //};

    public static readonly Vector2[] enemyWays = new Vector2[]
    { new Vector2(-2.466f, -3.904f), new Vector2(-2.459f, -0.856f), new Vector2(2.41f, -0.856f),  new Vector2(2.45f,  -3.88f)};

    public static readonly Vector2[] oppositeEnemyWays = new Vector2[]
    { new Vector2(2.515452f, 3.64f), new Vector2(2.515452f, 0.16f), new Vector2(-2.44f, 0.16f),  new Vector2(-2.44f,  3.32f)};


    public static readonly WaitForSeconds delayWave = new WaitForSeconds(1);
    public static readonly WaitForSeconds delayDiceBulletSpawn = new WaitForSeconds(0.5f);

    public static int TotalAttackDamage(int basicAttackDamage, int level)
    {
        int result = basicAttackDamage + level * 3;
        return result;
    }

   public const int DICE_LAYER = 8;
}
