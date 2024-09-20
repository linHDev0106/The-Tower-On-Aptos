using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TurretConfig")]
public class TurretConfig : ScriptableObject
{

    public UpgradeItemData[] ATK;

    public UpgradeItemData[] HP;

    public UpgradeItemData[] COIN;

    public UpgradeItemData[] SATELLITE;
}


[System.Serializable]
public class UpgradeItemData
{
    public string nameID;

    public double startValue;

    public double levelUpBase;

    public double levelUpMulti;

    public double maxValue;

    public int lvlupStartCoin;

    public int lvlupBaseCoinPrice;

    public int lvlupMultiCoinPrice;

    public int lvlupStartGold;

    public int lvlupBaseGoldPrice;

    public int lvlupMultiGoldPrice;
}
