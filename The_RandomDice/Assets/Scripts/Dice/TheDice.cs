using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class DiceData
{
    public int code;
    public Sprite sprite;
    public Color color;
    public int basicAttackDamage;
    //public string name;
    //public int atkPower;
}

[CreateAssetMenu(fileName = "TheDice", menuName = "Scriptable Object/TheDice")]
public class TheDice : ScriptableObject
{
    public DiceData[] diceDatas;
    [SerializeField]
    Vector2[] originDicePositions;
    public DiceData GetDiceData(int code) => Array.Find(diceDatas, x => x.code == code);

    public DiceData GetRandomDiceData() => diceDatas[UnityEngine.Random.Range(1, diceDatas.Length)];

    public Vector2 GetOriginDicePosition(int index) => originDicePositions[index];
}
