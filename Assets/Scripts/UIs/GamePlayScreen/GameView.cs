using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameView : BaseView
{


    public MainBoard mainBoard;

    public Slider turretHPSlider;

    public TextMeshProUGUI turretHpText;

    public TextMeshProUGUI gameSpeedText;

    public TextMeshProUGUI coinTxt;

    public TextMeshProUGUI goldTxt;

    public TextMeshProUGUI gemTxt;

    public TextMeshProUGUI crystalTxt;

    public Slider waveProgress;

    public TextMeshProUGUI waveTxt, waveValueTxt;

    public TextMeshProUGUI gameSpeedValueText;

    public TextMeshProUGUI remainRetriveTxt;

    public TextMeshProUGUI retriveTimerTxt;

    public TextMeshProUGUI rwCoinDisableTimerTxt;

    private bool waitToDisable;

    [HideInInspector]
    public float timer;

    [HideInInspector]
    public float timerGoldRW;

    private bool activeTimer;

    private float rwCoinTimerRandom;

    public GameObject coinX4Random;

    public GameObject settingBtn;


    public override void InitView()
    {
        mainBoard.IniteView();
        RefreshSpeedText();
        RefreshCurrencyText();

        timerGoldRW = 0.0f;
        waitToDisable = false;
        rwCoinTimerRandom = Random.RandomRange(90.0f,350.0f);

        if (GameManager.instance.remainRetrive < 5)
        {
            ActiveTimer();
        }
        else
            DisableTimer();

        RefreshRetriveText();

       
    }

    public override void Start()
    {

    }

    public override void Update()
    {
        if (activeTimer)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                retriveTimerTxt.text = Common.ConvertTimer(timer);
            }
            else
            {

                MoreRetrive();
            }
        }

        if(GameManager.instance.currentState == GameManager.GAME_STATE.PLAYING)
        {
            timerGoldRW += Time.deltaTime;

            if(timerGoldRW >= rwCoinTimerRandom && !coinX4Random.activeSelf)
            {
                timerGoldRW = 0.0f;
                coinX4Random.SetActive(true);

                if (!waitToDisable)
                {
                    waitToDisable = true;
                    StartCoroutine(DisableRWCoin());
                }
                   
            }
        }
      
    }

    IEnumerator DisableRWCoin()
    {
        for(int i = 10; i >= 1; i--)
        {
            if (!waitToDisable)
                break;
            else
            {
                AudioManager.instance.tickSfx.Play();
                rwCoinDisableTimerTxt.text = i.ToString();
                yield return new WaitForSeconds(Time.timeScale);
            }
           
        }

        waitToDisable = false;
        timerGoldRW = 0.0f;
        rwCoinTimerRandom += Random.RandomRange(50.0f, 200.0f);
        coinX4Random.SetActive(false);
    }

    public void GetX4Gold()
    {
        AdsControl.Instance.ShowRewardedAd(AdsControl.REWARD_TYPE.X4_COIN);
    }

    public void GetX4GoldCB()
    {
        AudioManager.instance.goldRWSfx.Play();
        StopCoroutine(DisableRWCoin());
        waitToDisable = false;
        timerGoldRW = 0.0f;
        rwCoinTimerRandom += Random.RandomRange(50.0f, 200.0f);
        coinX4Random.SetActive(false);
        GameManager.instance.MoreX4Coin();
    }

    void MoreRetrive()
    {
        GameManager.instance.remainRetrive++;

        if (GameManager.instance.remainRetrive == 5)
        {
            DisableTimer();
        }
        else
            timer = Common.COUNT_DOWN_TIMER;
        RefreshRetriveText();
        PlayerPrefs.SetInt("RemainRetrive", GameManager.instance.remainRetrive);

    }

    public void GetRetrive()
    {
        if (GameManager.instance.remainRetrive == 5)
        {
            timer = Common.COUNT_DOWN_TIMER;
        }
        if (GameManager.instance.remainRetrive > 0)
        {
            GameManager.instance.remainRetrive--;
        }
        ActiveTimer();
        RefreshRetriveText();
        PlayerPrefs.SetInt("RemainRetrive", GameManager.instance.remainRetrive);
    }

    public void ActiveTimer()
    {
        activeTimer = true;
        retriveTimerTxt.gameObject.SetActive(true);
       
    }

    public void DisableTimer()
    {
        activeTimer = false;
        retriveTimerTxt.gameObject.SetActive(false);
       
    }

    public void RefreshRetriveText()
    {
        
         remainRetriveTxt.text = string.Format(GleyLocalization.Manager.GetText("REVIVAL_STATUS_MAX"), GameManager.instance.remainRetrive, 5);
            
        
    }

    public void RefreshCurrencyText()
    {
        coinTxt.text = GameManager.instance.currentCoin.ToString();
        goldTxt.text = (GameManager.instance.currentGold + GameManager.instance.goldInGame).ToString();
        gemTxt.text = (GameManager.instance.currentGem + GameManager.instance.gemsInGame).ToString();
        crystalTxt.text = GameManager.instance.currentCrystal.ToString();
        //Debug.Log("COIN " + GameManager.instance.currentCoin);
    }

    public void UpdateTurretHPSlider(float currentHP, float maxHP)
    {
        turretHpText.text = Mathf.RoundToInt(currentHP) + "/" + Mathf.RoundToInt(maxHP);
        turretHPSlider.value = currentHP / maxHP;
    }

    public void ChangeGameSpeed()
    {
        AudioManager.instance.btnClickSfx.Play();
        GameManager.instance.ChangeGameSpeed();
        RefreshSpeedText();
    }

    public void RefreshSpeedText()
    {
        string currentSpeed = "X1";

        if (Time.timeScale == 1)
        {
            currentSpeed = "X1";
        }
        else if (Time.timeScale == 1.5f)
        {
            currentSpeed = "X1.5";
        }
        else
        {
            currentSpeed = "X2";
        }

        gameSpeedText.text = GleyLocalization.Manager.GetText("LAB_GAME_SPEED");
        gameSpeedValueText.text = currentSpeed;
    }
    public void RefreshWaveText(int mWave)
    {
        waveTxt.text = GleyLocalization.Manager.GetText("LB_WAVE") + " ";
        waveValueTxt.text = mWave.ToString();
    }

    public void RefreshWaveProgress(float progress)
    {
        waveProgress.value = progress;
    }

    public override void ShowView()
    {
        base.ShowView();
    }

    public override void SetLangText()
    {
        gameSpeedText.text = GleyLocalization.Manager.GetText("LAB_GAME_SPEED");
        waveTxt.text = GleyLocalization.Manager.GetText("LB_WAVE") + " ";
        remainRetriveTxt.text = string.Format(GleyLocalization.Manager.GetText("REVIVAL_STATUS_MAX"), GameManager.instance.remainRetrive, 5);
    }

    public void SetLangUpgradeText()
    {
        mainBoard.SetLangUpgradeText();
    }

    public void ShowSetting()
    {
        AudioManager.instance.btnClickSfx.Play();
        GameManager.instance.uiManager.settingView.ShowView();
       
    }
}
