using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "World")]

public class EnemyInWorld : ScriptableObject
{
    public List<Wave> listEnemyWave;

    public int worldStartDelay;

    public int startCoin;

    public int loopWave;

    public int worldID;
}

[System.Serializable]
public class Wave
{
    public List<Turn> listEnemyTurn;
}

[System.Serializable]
public class Turn
{
    [HideInInspector]
    public List<string> listUnitRow;

    public List<EnemyController> listUnit;

    public float minInterval, maxInterval;

    public float turnStartDelay;
}