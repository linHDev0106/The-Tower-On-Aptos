using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using static UnityEditor.Progress;

public class FactoryView : BaseView
{
    // [HideInInspector]
    public GameObject atkTabOn, atkTabOff;
    //  [HideInInspector]
    public GameObject defenseTabOn, defenseTabOff;
    //  [HideInInspector]
    public GameObject resourceTabOn, resourceTabOff;
    // [HideInInspector]
    public GameObject alliesTabOn, alliesTabOff;

    //[HideInInspector]
    public CanvasGroup attackView, hpView, resourceView, alliesView;

    public UpgradeItem itemAttackObject, itemHpObject, itemResourceObject, itemAlliesObject;

    public RectTransform attackObjectRoot, hpObjectRoot, coinObjectRoot, alliesObjectRoot;

    [HideInInspector]
    public List<UpgradeItem> attackItemList;
    [HideInInspector]
    public List<UpgradeItem> hpItemList;
    [HideInInspector]
    public List<UpgradeItem> resourceItemList;
    [HideInInspector]
    public List<UpgradeItem> alliesItemList;

    public TextMeshProUGUI unlockAtkName, unlockAtkDes, unlockAtkPrice, unlockAtkText;

    public TextMeshProUGUI unlockHpName, unlockHpDes, unlockHpPrice, unlockHpText;

    public TextMeshProUGUI unlockResourceName, unlockResourceDes, unlockResourcePrice, unlockResourceText;

    public TextMeshProUGUI unlockAlliesName, unlockAlliesDes, unlockAlliesPrice, unlockAlliesText;

    public TextMeshProUGUI attackValueTxt, hpValueTxt, attackSpeedValueTxt;

    public TextMeshProUGUI factoryTxt;

    public override void InitView()
    {
        attackItemList = new List<UpgradeItem>();
        hpItemList = new List<UpgradeItem>();
        resourceItemList = new List<UpgradeItem>();
        alliesItemList = new List<UpgradeItem>();
        ClickAtkTab();
        UpdateTextInBar();
    }

    public override void Start()
    {
        InitView();
    }

    public override void Update()
    {

    }

    public void UpdateTextInBar()
    {
        attackValueTxt.text = GameManager.instance.turretController.attack.ToString();
        hpValueTxt.text = GameManager.instance.turretController.HPMax.ToString();
        attackSpeedValueTxt.text = ((float)GameManager.instance.dataManager.turretAttackUpgradeData[2].currentValue).ToString();
    }

    public void ClickAtkTab()
    {
        //Debug.Log("CLICK ATK");
        AudioManager.instance.btnClickSfx.Play();


        atkTabOn.SetActive(true);
        atkTabOff.SetActive(false);

        defenseTabOn.SetActive(false);
        defenseTabOff.SetActive(true);

        resourceTabOn.SetActive(false);
        resourceTabOff.SetActive(true);

        alliesTabOn.SetActive(false);
        alliesTabOff.SetActive(true);


        ActiveView(attackView, true);
        ActiveView(hpView, false);
        ActiveView(resourceView, false);
        ActiveView(alliesView, false);

        LoadAttackItem();

    }

    public void ClickDefenseTab()
    {
        //Debug.Log("CLICK DEFENSE");
        AudioManager.instance.btnClickSfx.Play();

        atkTabOn.SetActive(false);
        atkTabOff.SetActive(true);

        defenseTabOn.SetActive(true);
        defenseTabOff.SetActive(false);

        resourceTabOn.SetActive(false);
        resourceTabOff.SetActive(true);

        alliesTabOn.SetActive(false);
        alliesTabOff.SetActive(true);

        ActiveView(attackView, false);
        ActiveView(hpView, true);
        ActiveView(resourceView, false);
        ActiveView(alliesView, false);

        LoadHpItem();

    }

    public void ClickResourceTab()
    {
        //Debug.Log("CLICK RESOURCE");
        AudioManager.instance.btnClickSfx.Play();
        atkTabOn.SetActive(false);
        atkTabOff.SetActive(true);

        defenseTabOn.SetActive(false);
        defenseTabOff.SetActive(true);

        resourceTabOn.SetActive(true);
        resourceTabOff.SetActive(false);

        alliesTabOn.SetActive(false);
        alliesTabOff.SetActive(true);

        ActiveView(attackView, false);
        ActiveView(hpView, false);
        ActiveView(resourceView, true);
        ActiveView(alliesView, false);

        LoadResourceItem();

    }


    public void ClickAlliesTab()
    {
        //Debug.Log("CLICK ALLIES");
        AudioManager.instance.btnClickSfx.Play();
        atkTabOn.SetActive(false);
        atkTabOff.SetActive(true);

        defenseTabOn.SetActive(false);
        defenseTabOff.SetActive(true);

        resourceTabOn.SetActive(false);
        resourceTabOff.SetActive(true);

        alliesTabOn.SetActive(true);
        alliesTabOff.SetActive(false);

        ActiveView(attackView, false);
        ActiveView(hpView, false);
        ActiveView(resourceView, false);
        ActiveView(alliesView, true);

        LoadAlliesItem();
    }

    void ActiveView(CanvasGroup group, bool isActive)
    {
        if (isActive)
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
        }
        else
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    public void LoadAttackItem()
    {
        //Debug.Log("LOAD ITEMS");

        if (attackItemList.Count <= 0)
        {
            attackItemList.Add(itemAttackObject);

            itemAttackObject.transform.localPosition = Vector3.zero;
            itemAttackObject.transform.localScale = Vector3.one;

            itemAttackObject.itemID = 0;

            itemAttackObject.currentData.type = UpgradeItemModel.TYPE.ATK;

            for (int i = 0; i < GameManager.instance.configManager.turretConfig.ATK.Length - 1; i++)
            {
                UpgradeItem item = Instantiate(itemAttackObject, Vector3.zero, Quaternion.identity);
                item.itemID = i + 1;
                item.transform.parent = attackObjectRoot.transform;

                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;

                attackItemList.Add(item);
            }
        }


        UpdateAttackData();

        for (int i = 0; i < attackItemList.Count; i++)
        {

            attackItemList[i].gameObject.SetActive(false);
            attackItemList[i].transform.parent = null;

        }


        for (int i = 0; i < GameManager.instance.turretUnlockAtkLevel + 1; i++)
        {
            for (int j = 0; j < GameManager.instance.configManager.turretLockConfig.ATK[i].attSet.Count; j++)
            {
                for (int k = 0; k < attackItemList.Count; k++)
                {

                    if (GameManager.instance.configManager.turretConfig.ATK[k].nameID == GameManager.instance.configManager.turretLockConfig.ATK[i].attSet[j])
                    {
                        attackItemList[k].gameObject.SetActive(true);
                        attackItemList[k].transform.parent = attackObjectRoot.transform;
                        attackItemList[k].transform.localPosition = Vector3.zero;
                        attackItemList[k].transform.localScale = Vector3.one;
                    }

                }
            }
        }

        if (GameManager.instance.turretUnlockAtkLevel + 1 == GameManager.instance.configManager.turretLockConfig.DEFENSE.Count)
        {
            unlockAtkName.transform.parent.gameObject.SetActive(false);
        }
        else

        ShowUnlockAtk();
    }

    public void LoadHpItem()
    {
        //Debug.Log("LOAD ITEMS");

        if (hpItemList.Count <= 0)
        {
            hpItemList.Add(itemHpObject);

            itemHpObject.transform.localPosition = Vector3.zero;
            itemHpObject.transform.localScale = Vector3.one;

            itemHpObject.itemID = 0;

            itemHpObject.currentData.type = UpgradeItemModel.TYPE.HP;

            for (int i = 0; i < GameManager.instance.configManager.turretConfig.HP.Length - 1; i++)
            {
                UpgradeItem item = Instantiate(itemHpObject, Vector3.zero, Quaternion.identity);
                item.itemID = i + 1;
                item.transform.parent = hpObjectRoot.transform;

                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;

                hpItemList.Add(item);
            }
        }


        UpdateHpData();

        for (int i = 0; i < hpItemList.Count; i++)
        {

            hpItemList[i].gameObject.SetActive(false);
            hpItemList[i].transform.parent = null;

        }


        for (int i = 0; i < GameManager.instance.turretUnlockHpLevel + 1; i++)
        {
            for (int j = 0; j < GameManager.instance.configManager.turretLockConfig.DEFENSE[i].attSet.Count; j++)
            {
                for (int k = 0; k < hpItemList.Count; k++)
                {

                    if (GameManager.instance.configManager.turretConfig.HP[k].nameID == GameManager.instance.configManager.turretLockConfig.DEFENSE[i].attSet[j])
                    {
                        hpItemList[k].gameObject.SetActive(true);
                        hpItemList[k].transform.parent = hpObjectRoot.transform;
                        hpItemList[k].transform.localPosition = Vector3.zero;
                        hpItemList[k].transform.localScale = Vector3.one;
                    }

                }
            }
        }
        if (GameManager.instance.turretUnlockHpLevel + 1 == GameManager.instance.configManager.turretLockConfig.DEFENSE.Count)
        {
            unlockHpName.transform.parent.gameObject.SetActive(false);
        }

       else

        ShowUnlockHp();
    }

    public void LoadResourceItem()
    {
        //Debug.Log("LOAD ITEMS");

        if (resourceItemList.Count <= 0)
        {
            resourceItemList.Add(itemResourceObject);

            itemResourceObject.transform.localPosition = Vector3.zero;
            itemResourceObject.transform.localScale = Vector3.one;

            itemResourceObject.itemID = 0;

            itemResourceObject.currentData.type = UpgradeItemModel.TYPE.COIN;

            for (int i = 0; i < GameManager.instance.configManager.turretConfig.COIN.Length - 1; i++)
            {
                UpgradeItem item = Instantiate(itemResourceObject, Vector3.zero, Quaternion.identity);
                item.itemID = i + 1;
                item.transform.parent = coinObjectRoot.transform;

                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;

                resourceItemList.Add(item);
            }
        }


        UpdateResourceData();

        for (int i = 0; i < resourceItemList.Count; i++)
        {

            resourceItemList[i].gameObject.SetActive(false);
            resourceItemList[i].transform.parent = null;

        }


        for (int i = 0; i < GameManager.instance.turretUnlockResourceLevel + 1; i++)
        {
            for (int j = 0; j < GameManager.instance.configManager.turretLockConfig.RESOURCE[i].attSet.Count; j++)
            {
                for (int k = 0; k < resourceItemList.Count; k++)
                {

                    if (GameManager.instance.configManager.turretConfig.COIN[k].nameID == GameManager.instance.configManager.turretLockConfig.RESOURCE[i].attSet[j])
                    {
                        resourceItemList[k].gameObject.SetActive(true);
                        resourceItemList[k].transform.parent = coinObjectRoot.transform;
                        resourceItemList[k].transform.localPosition = Vector3.zero;
                        resourceItemList[k].transform.localScale = Vector3.one;
                    }

                }
            }
        }

        if (GameManager.instance.turretUnlockResourceLevel + 1 == GameManager.instance.configManager.turretLockConfig.RESOURCE.Count)
        {
            unlockResourceName.transform.parent.gameObject.SetActive(false);
        }
        else
        ShowUnlockResource();
    }

    public void LoadAlliesItem()
    {
        //Debug.Log("LOAD ALLIES");

        if (alliesItemList.Count <= 0)
        {
            alliesItemList.Add(itemAlliesObject);

            itemAlliesObject.transform.localPosition = Vector3.zero;
            itemAlliesObject.transform.localScale = Vector3.one;

            itemAlliesObject.itemID = 0;

            itemAlliesObject.currentData.type = UpgradeItemModel.TYPE.SATELLITE;

            for (int i = 0; i < GameManager.instance.configManager.turretConfig.SATELLITE.Length - 1; i++)
            {
                UpgradeItem item = Instantiate(itemAlliesObject, Vector3.zero, Quaternion.identity);
                item.itemID = i + 1;
                item.transform.parent = alliesObjectRoot.transform;

                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;

                alliesItemList.Add(item);
            }
        }


        UpdateAlliesData();

        for (int i = 0; i < alliesItemList.Count; i++)
        {

            alliesItemList[i].gameObject.SetActive(false);
            alliesItemList[i].transform.parent = null;

        }


        for (int i = 0; i < GameManager.instance.turretUnlockAlliesLevel + 1; i++)
        {
            for (int j = 0; j < GameManager.instance.configManager.turretLockConfig.ALLIES[i].attSet.Count; j++)
            {
                for (int k = 0; k < alliesItemList.Count; k++)
                {

                    if (GameManager.instance.configManager.turretConfig.SATELLITE[k].nameID == GameManager.instance.configManager.turretLockConfig.ALLIES[i].attSet[j])
                    {
                        alliesItemList[k].gameObject.SetActive(true);
                        alliesItemList[k].transform.parent = alliesObjectRoot.transform;
                        alliesItemList[k].transform.localPosition = Vector3.zero;
                        alliesItemList[k].transform.localScale = Vector3.one;
                    }

                }
            }
        }

        if (GameManager.instance.turretUnlockAlliesLevel + 1 == GameManager.instance.configManager.turretLockConfig.ALLIES.Count)
        {
            unlockAlliesName.transform.parent.gameObject.SetActive(false);
        }
        else
            ShowUnlockAllies();
    }

    public void UpdateAttackData()
    {
        for (int i = 0; i < attackItemList.Count; i++)
        {

            attackItemList[i].InitView(GameManager.instance.dataManager.turretAttackUpgradeData[i]);


        }
    }

    public void UpdateHpData()
    {
        for (int i = 0; i < hpItemList.Count; i++)
        {

            hpItemList[i].InitView(GameManager.instance.dataManager.turretHpUpgradeData[i]);


        }
    }

    public void UpdateResourceData()
    {
        for (int i = 0; i < resourceItemList.Count; i++)
        {

            resourceItemList[i].InitView(GameManager.instance.dataManager.turretCoinUpgradeData[i]);


        }
    }

    public void UpdateAlliesData()
    {
        for (int i = 0; i < alliesItemList.Count; i++)
        {

            alliesItemList[i].InitView(GameManager.instance.dataManager.turretSatelliteUpgradeData[i]);


        }
    }


    public void ShowUnlockAtk()
    {
        if (GameManager.instance.turretUnlockAtkLevel + 1 < GameManager.instance.configManager.turretLockConfig.ATK.Count)
        {
            string nameIDUnlock = GameManager.instance.configManager.turretLockConfig.ATK[GameManager.instance.turretUnlockAtkLevel + 1].attSet[0];
            unlockAtkName.text = GleyLocalization.Manager.GetText(nameIDUnlock + "_SET_NAME");
            unlockAtkDes.text = GleyLocalization.Manager.GetText(nameIDUnlock + "_SET_DES");
           // Debug.Log(nameIDUnlock + "_SET_DES");
            unlockAtkPrice.text = GameManager.instance.configManager.turretLockConfig.ATK[GameManager.instance.turretUnlockAtkLevel + 1].unlockPrice.ToString();
            unlockAtkText.text = GleyLocalization.Manager.GetText("UNLOCK");
            LayoutRebuilder.ForceRebuildLayoutImmediate(attackObjectRoot);
        }
    }


    public void ShowUnlockHp()
    {
        if (GameManager.instance.turretUnlockHpLevel + 1 < GameManager.instance.configManager.turretLockConfig.DEFENSE.Count)
        {
            string nameIDUnlock = GameManager.instance.configManager.turretLockConfig.DEFENSE[GameManager.instance.turretUnlockHpLevel + 1].attSet[0];
            unlockHpName.text = GleyLocalization.Manager.GetText(nameIDUnlock + "_SET_NAME");
            unlockHpDes.text = GleyLocalization.Manager.GetText(nameIDUnlock + "_SET_DES");
            unlockHpPrice.text = GameManager.instance.configManager.turretLockConfig.DEFENSE[GameManager.instance.turretUnlockHpLevel + 1].unlockPrice.ToString();
            unlockHpText.text = GleyLocalization.Manager.GetText("UNLOCK");
            LayoutRebuilder.ForceRebuildLayoutImmediate(hpObjectRoot);

        }

    }

    public void ShowUnlockResource()
    {
        if (GameManager.instance.turretUnlockResourceLevel + 1 < GameManager.instance.configManager.turretLockConfig.RESOURCE.Count)
        {
          
            string nameIDUnlock = GameManager.instance.configManager.turretLockConfig.RESOURCE[GameManager.instance.turretUnlockResourceLevel + 1].attSet[0];
           // Debug.Log("RESOURCE " + nameIDUnlock);
            unlockResourceName.text = GleyLocalization.Manager.GetText(nameIDUnlock + "_SET_NAME");
            unlockResourceDes.text = GleyLocalization.Manager.GetText(nameIDUnlock + "_SET_DES");
            unlockResourcePrice.text = GameManager.instance.configManager.turretLockConfig.RESOURCE[GameManager.instance.turretUnlockResourceLevel + 1].unlockPrice.ToString();
            unlockResourceText.text = GleyLocalization.Manager.GetText("UNLOCK");
            LayoutRebuilder.ForceRebuildLayoutImmediate(coinObjectRoot);

        }

    }

    public void ShowUnlockAllies()
    {
        if (GameManager.instance.turretUnlockAlliesLevel + 1 < GameManager.instance.configManager.turretLockConfig.ALLIES.Count)
        {

            string nameIDUnlock = GameManager.instance.configManager.turretLockConfig.ALLIES[GameManager.instance.turretUnlockAlliesLevel + 1].attSet[0];
           // Debug.Log("RESOURCE " + nameIDUnlock);
            unlockAlliesName.text = GleyLocalization.Manager.GetText(nameIDUnlock + "_SET_NAME");
            unlockAlliesDes.text = GleyLocalization.Manager.GetText(nameIDUnlock + "_SET_DES");
            unlockAlliesPrice.text = GameManager.instance.configManager.turretLockConfig.ALLIES[GameManager.instance.turretUnlockAlliesLevel + 1].unlockPrice.ToString();
            unlockAlliesText.text = GleyLocalization.Manager.GetText("UNLOCK");
            LayoutRebuilder.ForceRebuildLayoutImmediate(alliesObjectRoot);

        }

    }

    public void UnlockAtk()
    {
        if (GameManager.instance.currentGold < GameManager.instance.configManager.turretLockConfig.ATK[GameManager.instance.turretUnlockAtkLevel + 1].unlockPrice)
        {
            GameManager.instance.uiManager.notiView.InitContent(GleyLocalization.Manager.GetText("ALERT_LACK_GOLD"));
            GameManager.instance.uiManager.notiView.ShowView();
            return;
        }
          
        AudioManager.instance.upgradeSfx.Play();
        GameManager.instance.SubGold(GameManager.instance.configManager.turretLockConfig.ATK[GameManager.instance.turretUnlockAtkLevel + 1].unlockPrice);

        GameManager.instance.turretUnlockAtkLevel++;

        PlayerPrefs.SetInt("TurretUnlockAtkLevel", GameManager.instance.turretUnlockAtkLevel);





        LoadAttackItem();

        if (GameManager.instance.turretUnlockAtkLevel + 1 == GameManager.instance.configManager.turretLockConfig.DEFENSE.Count)
        {
            unlockAtkName.transform.parent.gameObject.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(attackObjectRoot);
    }

    public void UnlockHp()
    {
        if (GameManager.instance.currentGold < GameManager.instance.configManager.turretLockConfig.ATK[GameManager.instance.turretUnlockHpLevel + 1].unlockPrice)
        {
            GameManager.instance.uiManager.notiView.InitContent(GleyLocalization.Manager.GetText("ALERT_LACK_GOLD"));
            GameManager.instance.uiManager.notiView.ShowView();
            return;
        }
        AudioManager.instance.upgradeSfx.Play();
        GameManager.instance.SubGold(GameManager.instance.configManager.turretLockConfig.ATK[GameManager.instance.turretUnlockHpLevel + 1].unlockPrice);

        GameManager.instance.turretUnlockHpLevel++;

        PlayerPrefs.SetInt("TurretUnlockHpLevel", GameManager.instance.turretUnlockHpLevel);




        LoadHpItem();


        if (GameManager.instance.turretUnlockHpLevel + 1 == GameManager.instance.configManager.turretLockConfig.DEFENSE.Count)
        {
            unlockHpName.transform.parent.gameObject.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(hpObjectRoot);


    }

   

    public void UnlockResource()
    {
        if (GameManager.instance.currentGold < GameManager.instance.configManager.turretLockConfig.RESOURCE[GameManager.instance.turretUnlockResourceLevel + 1].unlockPrice)
        {
            GameManager.instance.uiManager.notiView.InitContent(GleyLocalization.Manager.GetText("ALERT_LACK_GOLD"));
            GameManager.instance.uiManager.notiView.ShowView();
            return;
        }
        AudioManager.instance.upgradeSfx.Play();
        GameManager.instance.SubGold(GameManager.instance.configManager.turretLockConfig.RESOURCE[GameManager.instance.turretUnlockResourceLevel + 1].unlockPrice);

        GameManager.instance.turretUnlockResourceLevel++;

        PlayerPrefs.SetInt("TurretUnlockResourceLevel", GameManager.instance.turretUnlockResourceLevel);




        LoadResourceItem();


        if (GameManager.instance.turretUnlockResourceLevel + 1 == GameManager.instance.configManager.turretLockConfig.RESOURCE.Count)
        {
            unlockResourceName.transform.parent.gameObject.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(coinObjectRoot);


    }

    public void UnlockAllies()
    {
        if (GameManager.instance.currentGold < GameManager.instance.configManager.turretLockConfig.ALLIES[GameManager.instance.turretUnlockAlliesLevel + 1].unlockPrice)
        {
            GameManager.instance.uiManager.notiView.InitContent(GleyLocalization.Manager.GetText("ALERT_LACK_GOLD"));
            GameManager.instance.uiManager.notiView.ShowView();
            return;
        }
        AudioManager.instance.upgradeSfx.Play();
        GameManager.instance.SubGold(GameManager.instance.configManager.turretLockConfig.ALLIES[GameManager.instance.turretUnlockAlliesLevel + 1].unlockPrice);

        GameManager.instance.turretUnlockAlliesLevel++;

        PlayerPrefs.SetInt("TurretUnlockAlliesLevel", GameManager.instance.turretUnlockAlliesLevel);




        LoadAlliesItem();


        if (GameManager.instance.turretUnlockAlliesLevel + 1 == GameManager.instance.configManager.turretLockConfig.ALLIES.Count)
        {
            unlockAlliesName.transform.parent.gameObject.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(alliesObjectRoot);


    }

    public override void SetLangText()
    {
        factoryTxt.text = GleyLocalization.Manager.GetText("TAB_FACTORY");

    }

    public void SetLangUpgradeText()
    {
        

        ShowUnlockAtk();
        ShowUnlockHp();
        ShowUnlockResource();
        ShowUnlockAllies();

        UpdateAttackData();
        UpdateHpData();
        UpdateResourceData();
        UpdateAlliesData();
    }
}