using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class DataManager : MonoBehaviour
{
   
    [HideInInspector]
    public List<UpgradeItemModel> turretAttackUpgradeData;


    [HideInInspector]
    public List<UpgradeItemModel> turretHpUpgradeData;

     [HideInInspector]
    public List<UpgradeItemModel> turretCoinUpgradeData;


     [HideInInspector]
    public List<UpgradeItemModel> turretSatelliteUpgradeData;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitData()
    {
        LoadTurretLevel();
    }

    public void Refresh()
    {
        for(int i = 0; i < turretAttackUpgradeData.Count;i++)
        {
            turretAttackUpgradeData[i].inGameLevel = 0;
            turretAttackUpgradeData[i].UpdateValue();
        }

        for (int i = 0; i < turretHpUpgradeData.Count; i++)
        {
            turretHpUpgradeData[i].inGameLevel = 0;
            turretHpUpgradeData[i].UpdateValue();
        }

        for (int i = 0; i < turretCoinUpgradeData.Count; i++)
        {
            turretCoinUpgradeData[i].inGameLevel = 0;
            turretCoinUpgradeData[i].UpdateValue();
        }

        for (int i = 0; i < turretSatelliteUpgradeData.Count; i++)
        {
            turretSatelliteUpgradeData[i].inGameLevel = 0;
            turretSatelliteUpgradeData[i].UpdateValue();
        }
    }

    public void LoadTurretLevel()
    {
        turretAttackUpgradeData = new List<UpgradeItemModel>();

        for(int i = 0; i < GameManager.instance.configManager.turretConfig.ATK.Length; i++)
        {
            //int currentLv = PlayerPrefs.GetInt("TurretAttack" + i.ToString());
            //turretAttackLevel.Add(currentLv);

            UpgradeItemModel item = new UpgradeItemModel();


            item.itemID = i;

            item.nameID = GameManager.instance.configManager.turretConfig.ATK[i].nameID;

            item.type = UpgradeItemModel.TYPE.ATK;

            item.baseLevel = item.GetBaseLevel();

            item.inGameLevel = 0;

            item.UpdateValue();

            turretAttackUpgradeData.Add(item);
        }

        for (int i = 0; i < GameManager.instance.configManager.turretConfig.HP.Length; i++)
        {

            UpgradeItemModel item = new UpgradeItemModel();

            item.itemID = i;

            item.nameID = GameManager.instance.configManager.turretConfig.HP[i].nameID;

            item.type = UpgradeItemModel.TYPE.HP;

            item.baseLevel = item.GetBaseLevel();


            item.inGameLevel = 0;

            item.UpdateValue();

            turretHpUpgradeData.Add(item);
        }

        for (int i = 0; i < GameManager.instance.configManager.turretConfig.COIN.Length; i++)
        {

            UpgradeItemModel item = new UpgradeItemModel();

            item.itemID = i;

            item.nameID = GameManager.instance.configManager.turretConfig.COIN[i].nameID;

            item.type = UpgradeItemModel.TYPE.COIN;

            item.baseLevel = item.GetBaseLevel();


            item.inGameLevel = 0;

            item.UpdateValue();

            turretCoinUpgradeData.Add(item);
        }


        for (int i = 0; i < GameManager.instance.configManager.turretConfig.SATELLITE.Length; i++)
        {

            UpgradeItemModel item = new UpgradeItemModel();

            item.itemID = i;

            item.nameID = GameManager.instance.configManager.turretConfig.SATELLITE[i].nameID;

            item.type = UpgradeItemModel.TYPE.SATELLITE;

            item.baseLevel = item.GetBaseLevel();


            item.inGameLevel = 0;

            item.UpdateValue();

            turretSatelliteUpgradeData.Add(item);
        }
    }



}


[System.Serializable]
public class UpgradeItemModel
{
    public int itemID;

    public string nameID;

    public enum TYPE
    {
        ATK,
        HP,
        COIN,
        SATELLITE
    }

    public TYPE type;

    public int baseLevel;

    public int inGameLevel;

    public double currentValue;

    public double nextValue;

    public int upgradeGold;

    public int upgradeCoin;

    public void UpInGameLevel()
    {
        inGameLevel++;
        UpdateValue();
    }
    public void UpBaseLevel()
    {
        baseLevel++;

        PlayerPrefs.SetInt(nameID, baseLevel);

        UpdateValue();
    }

    public int GetBaseLevel()
    { 
       return PlayerPrefs.GetInt(nameID);
    }

    public void UpdateValue()
    {
        if(type == TYPE.ATK)
        {
            currentValue = Common.CalTurretAttackValue(GameManager.instance.configManager.turretConfig.ATK[itemID].startValue,
                GameManager.instance.configManager.turretConfig.ATK[itemID].levelUpBase, baseLevel + inGameLevel,
                GameManager.instance.configManager.turretConfig.ATK[itemID].levelUpMulti);

            nextValue = Common.CalTurretAttackValue(GameManager.instance.configManager.turretConfig.ATK[itemID].startValue,
               GameManager.instance.configManager.turretConfig.ATK[itemID].levelUpBase, baseLevel + inGameLevel + 1,
               GameManager.instance.configManager.turretConfig.ATK[itemID].levelUpMulti);

            upgradeCoin = Common.CalTurretAttackPrice(GameManager.instance.configManager.turretConfig.ATK[itemID].lvlupStartCoin,
              GameManager.instance.configManager.turretConfig.ATK[itemID].lvlupBaseCoinPrice, inGameLevel,
              GameManager.instance.configManager.turretConfig.ATK[itemID].lvlupMultiCoinPrice);

            upgradeGold = Common.CalTurretAttackPrice(GameManager.instance.configManager.turretConfig.ATK[itemID].lvlupStartGold,
               GameManager.instance.configManager.turretConfig.ATK[itemID].lvlupBaseGoldPrice, baseLevel,
               GameManager.instance.configManager.turretConfig.ATK[itemID].lvlupMultiGoldPrice);
        }

        else if(type == TYPE.HP)
        {

            currentValue = Common.CalTurretHpValue(GameManager.instance.configManager.turretConfig.HP[itemID].startValue,
                GameManager.instance.configManager.turretConfig.HP[itemID].levelUpBase, baseLevel + inGameLevel,
                GameManager.instance.configManager.turretConfig.HP[itemID].levelUpMulti);

            nextValue = Common.CalTurretHpValue(GameManager.instance.configManager.turretConfig.HP[itemID].startValue,
               GameManager.instance.configManager.turretConfig.HP[itemID].levelUpBase, baseLevel+ inGameLevel + 1,
               GameManager.instance.configManager.turretConfig.HP[itemID].levelUpMulti);

            upgradeCoin = Common.CalTurretHpPrice(GameManager.instance.configManager.turretConfig.HP[itemID].lvlupStartCoin,
              GameManager.instance.configManager.turretConfig.HP[itemID].lvlupBaseCoinPrice, inGameLevel,
              GameManager.instance.configManager.turretConfig.HP[itemID].lvlupMultiCoinPrice);

            upgradeGold = Common.CalTurretHpPrice(GameManager.instance.configManager.turretConfig.HP[itemID].lvlupStartGold,
               GameManager.instance.configManager.turretConfig.HP[itemID].lvlupBaseGoldPrice, baseLevel,
               GameManager.instance.configManager.turretConfig.HP[itemID].lvlupMultiGoldPrice);
        }

        else if (type == TYPE.COIN)
        {

            currentValue = Common.CalTurretHpValue(GameManager.instance.configManager.turretConfig.COIN[itemID].startValue,
                GameManager.instance.configManager.turretConfig.COIN[itemID].levelUpBase, baseLevel + inGameLevel,
                GameManager.instance.configManager.turretConfig.COIN[itemID].levelUpMulti);

            nextValue = Common.CalTurretHpValue(GameManager.instance.configManager.turretConfig.COIN[itemID].startValue,
               GameManager.instance.configManager.turretConfig.COIN[itemID].levelUpBase, baseLevel + inGameLevel + 1,
               GameManager.instance.configManager.turretConfig.COIN[itemID].levelUpMulti);

            upgradeCoin = Common.CalTurretHpPrice(GameManager.instance.configManager.turretConfig.COIN[itemID].lvlupStartCoin,
              GameManager.instance.configManager.turretConfig.COIN[itemID].lvlupBaseCoinPrice, inGameLevel,
              GameManager.instance.configManager.turretConfig.COIN[itemID].lvlupMultiCoinPrice);

            upgradeGold = Common.CalTurretHpPrice(GameManager.instance.configManager.turretConfig.COIN[itemID].lvlupStartGold,
               GameManager.instance.configManager.turretConfig.COIN[itemID].lvlupBaseGoldPrice, baseLevel,
               GameManager.instance.configManager.turretConfig.COIN[itemID].lvlupMultiGoldPrice);
        }

        else if (type == TYPE.SATELLITE)
        {

            currentValue = Common.CalTurretHpValue(GameManager.instance.configManager.turretConfig.SATELLITE[itemID].startValue,
                GameManager.instance.configManager.turretConfig.SATELLITE[itemID].levelUpBase, baseLevel + inGameLevel,
                GameManager.instance.configManager.turretConfig.SATELLITE[itemID].levelUpMulti);

            nextValue = Common.CalTurretHpValue(GameManager.instance.configManager.turretConfig.SATELLITE[itemID].startValue,
               GameManager.instance.configManager.turretConfig.SATELLITE[itemID].levelUpBase, baseLevel + inGameLevel + 1,
               GameManager.instance.configManager.turretConfig.SATELLITE[itemID].levelUpMulti);

            upgradeCoin = Common.CalTurretHpPrice(GameManager.instance.configManager.turretConfig.SATELLITE[itemID].lvlupStartCoin,
              GameManager.instance.configManager.turretConfig.SATELLITE[itemID].lvlupBaseCoinPrice, inGameLevel,
              GameManager.instance.configManager.turretConfig.SATELLITE[itemID].lvlupMultiCoinPrice);

            upgradeGold = Common.CalTurretHpPrice(GameManager.instance.configManager.turretConfig.SATELLITE[itemID].lvlupStartGold,
               GameManager.instance.configManager.turretConfig.SATELLITE[itemID].lvlupBaseGoldPrice, baseLevel,
               GameManager.instance.configManager.turretConfig.SATELLITE[itemID].lvlupMultiGoldPrice);
        }
    }
}
