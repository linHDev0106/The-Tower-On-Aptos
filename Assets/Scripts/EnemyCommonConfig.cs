using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyCommonConfig")]
public class EnemyCommonConfig : ScriptableObject
{

    public float waveUpAtk;

    public float waveUpHP;

    public int waveCountForUpCoin;

    public int waveUpCoin;
}

