using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Enemy List")]
    [Space(5)]
    public Enemy[] enemyList;
}

[System.Serializable]
public class Enemy
{

    public string enemyName;

    public enum AttackType
    {
        MELEE,
        RANGE
    }

    public AttackType atkType;

    public float atk;

    public float hp;

    public float moveSpeed;

    public float atkSpeed;

    public float atkRange;

    public int baseKillCoin;

    public int killGold;

    public float killGoldRate;
}
