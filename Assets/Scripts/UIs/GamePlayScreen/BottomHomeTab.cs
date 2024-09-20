using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BottomHomeTab : BaseView
{
    public Image factoryTab, homeTab, storeTab;

    public Sprite tabOn, tabOff;

    public TextMeshProUGUI factoryTxt, battleTxt, storeTxt;

    public void ChooseFactory()
    {
        AudioManager.instance.btnClickSfx.Play();
        GameManager.instance.uiManager.factoryView.ShowView();
        GameManager.instance.uiManager.homeView.HideView();
        GameManager.instance.uiManager.storeView.HideView();
        factoryTab.sprite = tabOn;
        homeTab.sprite = tabOff;
        storeTab.sprite = tabOff;

        if (GameManager.instance.uiManager.tutView.finishTut4 == 0)
            GameManager.instance.uiManager.tutView.ShowTut4();
    }

    public void ChooseHome()
    {
        AudioManager.instance.btnClickSfx.Play();
        GameManager.instance.uiManager.factoryView.HideView();
        GameManager.instance.uiManager.homeView.ShowView();
        GameManager.instance.uiManager.storeView.HideView();
        factoryTab.sprite = tabOff;
        homeTab.sprite = tabOn;
        storeTab.sprite = tabOff;
    }

    public void ChooseStore()
    {
        AudioManager.instance.btnClickSfx.Play();
        GameManager.instance.uiManager.factoryView.HideView();
        GameManager.instance.uiManager.homeView.HideView();
        GameManager.instance.uiManager.storeView.ShowView();
        factoryTab.sprite = tabOff;
        homeTab.sprite = tabOff;
        storeTab.sprite = tabOn;
    }

    public override void Start()
    {
        ChooseHome();
    }

    public override void Update()
    {
       
    }

    public override void InitView()
    {
       
    }

    public override void SetLangText()
    {
        factoryTxt.text = GleyLocalization.Manager.GetText("TAB_FACTORY");
        battleTxt.text = GleyLocalization.Manager.GetText("TAB_HOME");
        storeTxt.text = GleyLocalization.Manager.GetText("TAB_STORE");
    }
}
