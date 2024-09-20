using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using static UnityEditor.Progress;

public class MainBoard : MonoBehaviour
{
    [HideInInspector]
    public GameObject atkTabOn, atkTabOff;
    [HideInInspector]
    public GameObject defenseTabOn, defenseTabOff;
    [HideInInspector]
    public GameObject resourceTabOn, resourceTabOff;
    [HideInInspector]
    public GameObject alliesTabOn, alliesTabOff;
    [HideInInspector]
    public UpgradeItem itemAttackObject, itemHpObject, itemResourceObject, itemAlliesObject;

    public RectTransform attackObjectRoot, hpObjectRoot, coinObjectRoot, allieseObjectRoot;
    [HideInInspector]
    public CanvasGroup attackView, hpView, resourceView, alliesView;
    [HideInInspector]
    public List<UpgradeItem> attackItemList;
    [HideInInspector]
    public List<UpgradeItem> hpItemList;
    [HideInInspector]
    public List<UpgradeItem> resourceItemList;
    [HideInInspector]
    public List<UpgradeItem> allieseItemList;

    public TextMeshProUGUI attackValueTxt, hpValueTxt, attackSpeedValueTxt;

   public void IniteView()
   {
        UpdateTextInBar();
        ClickAtkTab();
    }

    public void ClickAtkTab()
    {
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

        attackObjectRoot.localPosition = new Vector3(attackObjectRoot.localPosition.x,
            0, 0);
    }

    public void UpdateTextInBar()
    {
        attackValueTxt.text = GameManager.instance.turretController.attack.ToString();
        hpValueTxt.text = GameManager.instance.turretController.HPMax.ToString();
        attackSpeedValueTxt.text = ((float)GameManager.instance.dataManager.turretAttackUpgradeData[2].currentValue).ToString();
    }

   
    public void ClickDefenseTab()
    {
        AudioManager.instance.btnClickSfx.Play();
        hpObjectRoot.localPosition = Vector3.zero;
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
        ActiveView(resourceView,false);
        ActiveView(alliesView, false);

        LoadHPItem();

        hpObjectRoot.localPosition = new Vector3(attackObjectRoot.localPosition.x,
            0, 0);
    }

    public void ClickResourceTab()
    {
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

        coinObjectRoot.localPosition = new Vector3(coinObjectRoot.localPosition.x,
            0, 0);
    }

    void ActiveView(CanvasGroup group, bool isActive)
    {
        if(isActive)
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

    public void ClickAlliesTab()
    {
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

        allieseObjectRoot.localPosition = new Vector3(allieseObjectRoot.localPosition.x,
            0, 0);
    }

    public void LoadAttackItem()
    {
        if(attackItemList.Count <= 0)
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

    }

    public void UpdateAttackData()
    {
        for (int i = 0; i < attackItemList.Count; i++)
        {

            attackItemList[i].InitView(GameManager.instance.dataManager.turretAttackUpgradeData[i]);

            
        }
    }

    public void LoadHPItem()
    {
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
    }

    public void UpdateHpData()
    {
        for (int i = 0; i < hpItemList.Count; i++)
        {

            hpItemList[i].InitView(GameManager.instance.dataManager.turretHpUpgradeData[i]);

        }
    }

    public void LoadResourceItem()
    {
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
    }

    public void UpdateResourceData()
    {
        for (int i = 0; i < resourceItemList.Count; i++)
        {

            resourceItemList[i].InitView(GameManager.instance.dataManager.turretCoinUpgradeData[i]);

        }
    }

  

    public void LoadAlliesItem()
    {
        if (allieseItemList.Count <= 0)
        {
            allieseItemList.Add(itemAlliesObject);

            itemAlliesObject.transform.localPosition = Vector3.zero;
            itemAlliesObject.transform.localScale = Vector3.one;

            itemAlliesObject.itemID = 0;
            itemAlliesObject.currentData.type = UpgradeItemModel.TYPE.SATELLITE;

            for (int i = 0; i < GameManager.instance.configManager.turretConfig.SATELLITE.Length - 1; i++)
            {
                UpgradeItem item = Instantiate(itemAlliesObject, Vector3.zero, Quaternion.identity);
                item.itemID = i + 1;
                item.transform.parent = allieseObjectRoot.transform;


                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                allieseItemList.Add(item);
            }
        }

        UpdateAlliesData();

        for (int i = 0; i < allieseItemList.Count; i++)
        {

            allieseItemList[i].gameObject.SetActive(false);
            allieseItemList[i].transform.parent = null;

        }


        for (int i = 0; i < GameManager.instance.turretUnlockAlliesLevel + 1; i++)
        {
            for (int j = 0; j < GameManager.instance.configManager.turretLockConfig.ALLIES[i].attSet.Count; j++)
            {
                for (int k = 0; k < allieseItemList.Count; k++)
                {

                    if (GameManager.instance.configManager.turretConfig.SATELLITE[k].nameID == GameManager.instance.configManager.turretLockConfig.ALLIES[i].attSet[j])
                    {
                        allieseItemList[k].gameObject.SetActive(true);
                        allieseItemList[k].transform.parent = allieseObjectRoot.transform;
                        allieseItemList[k].transform.localPosition = Vector3.zero;
                        allieseItemList[k].transform.localScale = Vector3.one;
                    }

                }
            }
        }
    }

    public void UpdateAlliesData()
    {
        for (int i = 0; i < allieseItemList.Count; i++)
        {

            allieseItemList[i].InitView(GameManager.instance.dataManager.turretSatelliteUpgradeData[i]);

        }
    }

    public void SetLangUpgradeText()
    {
        UpdateAttackData();
        UpdateHpData();
        UpdateResourceData();
        UpdateAlliesData();
    }
}
