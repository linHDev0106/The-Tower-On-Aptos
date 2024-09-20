using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreView : BaseView
{
    public RectTransform moreGemBtn;

    public TextMeshProUGUI retriveTimerTxt;

    public TextMeshProUGUI freeGemsTxt;

    public TextMeshProUGUI gemsTxt;

    public TextMeshProUGUI getTxt;

    public TextMeshProUGUI specialTxt;

    public TextMeshProUGUI luckyChestTxt;

    [HideInInspector]
    public float timer;

    private float remainTimer;

    private bool activeTimer;

    public override void InitView()
    {
        remainTimer = PlayerPrefs.GetFloat("RewardTimer");

        if(remainTimer <= 0.0f)
        {
            DisableTimer();
        }
        else
        {
            ActiveTimer();
        }

        timer = remainTimer;
    }

    public override void Start()
    {
        InitView();
    }

    public override void Update()
    {
        if (activeTimer)
        {
            if (timer - Time.deltaTime > 0.0f)
            {
                timer -= Time.deltaTime;
                retriveTimerTxt.text = "Resets In: " + Common.ConvertTimer(timer);
            }
            else
            {
                timer = 0.0f;
                if (activeTimer)
                   DisableTimer();
            }
        }

    }

    public void GetFreeGems()
    {
        AdsControl.Instance.ShowRewardedAd(AdsControl.REWARD_TYPE.GEMS_REWARD);
    }

    public void GetFreeGemsCB()
    {
        AudioManager.instance.goldRWSfx.Play();
        GameManager.instance.MoreGems(10);
        moreGemBtn.gameObject.SetActive(false);
        ActiveTimer();
    }

    public void OpenChest()
    {
        if (GameManager.instance.currentGem < 20)
        {
            GameManager.instance.uiManager.notiView.InitContent(GleyLocalization.Manager.GetText("ALERT_LACK_GEM"));
            GameManager.instance.uiManager.notiView.ShowView();
            return;
        }
            
        GameManager.instance.SubGem(20);
        GameManager.instance.uiManager.rewardView.InitValue(Random.Range(50,1000), Random.Range(1,9));
        GameManager.instance.uiManager.rewardView.ShowView();
        HideView();
    }

    public void SaveTimer()
    {
       // Debug.Log("SAVE TIMER");
        remainTimer = timer;
        PlayerPrefs.SetFloat("RewardTimer", remainTimer);
    }

    public void ActiveTimer()
    {
        activeTimer = true;
        timer = Common.REWARD_TIMER;
        retriveTimerTxt.gameObject.SetActive(true);
        moreGemBtn.gameObject.SetActive(false);
    }

    public void DisableTimer()
    {
        activeTimer = false;
        retriveTimerTxt.gameObject.SetActive(false);
        moreGemBtn.gameObject.SetActive(true);
    }

    public override void SetLangText()
    {
        freeGemsTxt.text = GleyLocalization.Manager.GetText("STORE_FREE_GEM");
        gemsTxt.text = GleyLocalization.Manager.GetText("GEM");
        getTxt.text = GleyLocalization.Manager.GetText("CLAIM_TX");
        specialTxt.text = GleyLocalization.Manager.GetText("STORE_SPECIAL_OFFER_TITLE");
        luckyChestTxt.text = GleyLocalization.Manager.GetText("LUCKY_CHEST");
    }
}
