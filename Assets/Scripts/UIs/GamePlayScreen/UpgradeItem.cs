using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GleyLocalization;
using UnityEngine.UI;
using Lofelt.NiceVibrations;

public class UpgradeItem : MonoBehaviour
{
    public enum TYPE
    {
        IN_GAME,
        FACTORY
    };

    public TYPE type;

    public int itemID;

    public TextMeshProUGUI itemName;

    public TextMeshProUGUI currentValueTxt;

    public TextMeshProUGUI nextValueTxt;

    public TextMeshProUGUI priceUpdateTxt;

    public UpgradeItemModel currentData;

    public Button upgradeBtn;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnable();
    }

  

    public void ProcessUpgradeItem()
    {

        if (type == TYPE.IN_GAME)
            UpgradeByCoin();
        else if (type == TYPE.FACTORY)
            UpgradeByGold();
      
    }

    public void UpgradeByCoin()
    {
        if (GameManager.instance.currentCoin >= currentData.upgradeCoin)
        {
            if (AudioManager.instance.isVibration)
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

           
            AudioManager.instance.upgradeSfx.Play();
            GameManager.instance.PlayClickFx();
            GameManager.instance.currentCoin -= currentData.upgradeCoin;
            GameManager.instance.uiManager.gameView.RefreshCurrencyText();
            GameManager.instance.upgradeTurretFx.Play();

            if (currentData.type == UpgradeItemModel.TYPE.ATK)
            {
                GameManager.instance.dataManager.turretAttackUpgradeData[itemID].UpInGameLevel();
                InitView(GameManager.instance.dataManager.turretAttackUpgradeData[itemID]);
                GameManager.instance.turretController.UpdateAttack();
            }
            else if (currentData.type == UpgradeItemModel.TYPE.HP)
            {
                GameManager.instance.dataManager.turretHpUpgradeData[itemID].UpInGameLevel();
                InitView(GameManager.instance.dataManager.turretHpUpgradeData[itemID]);
                GameManager.instance.turretController.UpdateHP();
                GameManager.instance.turretController.UpdateShieldHP();
            }

            else if (currentData.type == UpgradeItemModel.TYPE.COIN)
            {
                GameManager.instance.dataManager.turretCoinUpgradeData[itemID].UpInGameLevel();
                InitView(GameManager.instance.dataManager.turretCoinUpgradeData[itemID]);

            }

            else if (currentData.type == UpgradeItemModel.TYPE.SATELLITE)
            {
                GameManager.instance.dataManager.turretSatelliteUpgradeData[itemID].UpInGameLevel();
                InitView(GameManager.instance.dataManager.turretSatelliteUpgradeData[itemID]);
                GameManager.instance.fireBallManager.ShowFireBall(Mathf.RoundToInt((float)GameManager.instance.dataManager.turretSatelliteUpgradeData[0].currentValue));
            }

            GameManager.instance.uiManager.gameView.mainBoard.UpdateTextInBar();
        }
    }

    public void UpgradeByGold()
    {
        if (GameManager.instance.currentGold >= currentData.upgradeGold)
        {
            if (AudioManager.instance.isVibration)
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            AudioManager.instance.upgradeSfx.Play();
            GameManager.instance.PlayClickFx();
            GameManager.instance.currentGold -= currentData.upgradeGold;
            GameManager.instance.uiManager.gameView.RefreshCurrencyText();
            GameManager.instance.upgradeTurretFx.Play();

            if (currentData.type == UpgradeItemModel.TYPE.ATK)
            {
                GameManager.instance.dataManager.turretAttackUpgradeData[itemID].UpBaseLevel();
                InitView(GameManager.instance.dataManager.turretAttackUpgradeData[itemID]);
                GameManager.instance.turretController.UpdateAttack();
            }
            else if (currentData.type == UpgradeItemModel.TYPE.HP)
            {
                GameManager.instance.dataManager.turretHpUpgradeData[itemID].UpBaseLevel();
                InitView(GameManager.instance.dataManager.turretHpUpgradeData[itemID]);
                
            }

            else if (currentData.type == UpgradeItemModel.TYPE.COIN)
            {
                GameManager.instance.dataManager.turretCoinUpgradeData[itemID].UpBaseLevel();
                InitView(GameManager.instance.dataManager.turretCoinUpgradeData[itemID]);

            }

            else if (currentData.type == UpgradeItemModel.TYPE.SATELLITE)
            {
                GameManager.instance.dataManager.turretSatelliteUpgradeData[itemID].UpBaseLevel();
                InitView(GameManager.instance.dataManager.turretSatelliteUpgradeData[itemID]);
               
            }

            GameManager.instance.uiManager.factoryView.UpdateTextInBar();
            GameManager.instance.SaveCurrency();
        }
    }

    public void InitView(UpgradeItemModel mData)
    {
        currentData = mData;

        itemName.text = GleyLocalization.Manager.GetText(mData.nameID.ToString());

        if(mData.nameID == "DOUBLE_SHOT_CHANCE"
            || mData.nameID == "DOUBLE_SHOT_PER"
            || mData.nameID == "CRIT_CHANCE"
            || mData.nameID == "CRIT_PER"
            || mData.nameID == "MULTI_SHOT_CHANCE"
             || mData.nameID == "BOUNCE_CHANCE"
              || mData.nameID == "BOUNCE_PER"
              || mData.nameID == "RANGER_BONUS"
              || mData.nameID == "SLOW_CHANCE"
              || mData.nameID == "SLOW_PER"
              || mData.nameID == "DAMAGE_RESISTANCE"
              || mData.nameID == "DODGE"
              || mData.nameID == "DAMAGE_RETURN_CHANCE"
              || mData.nameID == "DAMAGE_RETURN_PER"
              || mData.nameID == "LIFE_LEAK"
              || mData.nameID == "DISCOUNT")
        {
            //if(((float)mData.currentValue * 100) >= 1.0f)
            //    currentValueTxt.text =  (Mathf.RoundToInt((float)mData.currentValue * 100)).ToString() + "%";
           // else
                currentValueTxt.text = (mData.currentValue * 100).ToString() + "%";

           // if (((float)mData.nextValue * 100) >= 1.0f)
           //     nextValueTxt.text = (Mathf.RoundToInt((float)mData.nextValue * 100)).ToString() + "%";
           // else
                nextValueTxt.text = (mData.nextValue * 100).ToString() + "%";
        }
        else
        {
            currentValueTxt.text = mData.currentValue.ToString();

            nextValueTxt.text = mData.nextValue.ToString();
        }


        if (type == TYPE.IN_GAME)
            priceUpdateTxt.text = mData.upgradeCoin.ToString();
        else if (type == TYPE.FACTORY)
            priceUpdateTxt.text = mData.upgradeGold.ToString();

        //Debug.Log("INGAME " + mData.upgradeCoin + " LEVEL" +mData.inGameLevel);

    }

    public void CheckEnable()
    {


        if (type == TYPE.IN_GAME)
        {
            if (GameManager.instance.currentCoin >= currentData.upgradeCoin)
            {
                upgradeBtn.interactable = true;
            }
            else
            {
                upgradeBtn.interactable = false;
            }
        }
        else if (type == TYPE.FACTORY)
        {
            if (GameManager.instance.currentGold >= currentData.upgradeGold)
            {
                upgradeBtn.interactable = true;
            }
            else
            {
                upgradeBtn.interactable = false;
            }
        }
    }
}
