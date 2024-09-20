using System.Collections;
using System.Collections.Generic;
using System.Data;
using GleyLocalization;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameView gameView;

    public RetriveView retriveView;

    public ResultView resultView;

    public HomeView homeView;

    public FactoryView factoryView;

    public StoreView storeView;

    public RewardView rewardView;

    public BottomHomeTab homeTab;

    public SettingView settingView;

    public NotifyView notiView;

    public TutView tutView;

    public TWUpView twUpView;

    public void InitView()
    {
        // gameView.InitView();
        homeView.SetLangText();
        homeTab.SetLangText();
        factoryView.SetLangText();
        storeView.SetLangText();
        rewardView.SetLangText();
        gameView.SetLangText();
        settingView.SetLangText();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



    }

    public void UpdateLang()
    {
        homeView.SetLangText();
        homeTab.SetLangText();
        factoryView.SetLangText();
        storeView.SetLangText();
        rewardView.SetLangText();
        gameView.SetLangText();
        factoryView.SetLangUpgradeText();
        gameView.SetLangUpgradeText();
        settingView.SetLangText();
    }
}
