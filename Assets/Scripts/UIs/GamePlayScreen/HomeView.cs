using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GleyLocalization;

public class HomeView : BaseView
{
    public TextMeshProUGUI battleTxt;

    public TextMeshProUGUI homeWorldTxt;

    public TextMeshProUGUI bestWaveTxt;

    public TextMeshProUGUI unLockTxt;

    public TextMeshProUGUI rwTimerTxt;

    public TextMeshProUGUI claimTxt;

    public TextMeshProUGUI goldTxt,gemsTxt;

    private float claimTimer;

    public Slider timerSlider;

    private bool isFull;

    private int currentGoldClaim, currentGemsClaim;

    private float currentClaimTimer;

    public override void InitView()
    {
        currentClaimTimer = PlayerPrefs.GetFloat("ClaimTimer");
        currentGoldClaim = (int)(Common.CLAIM_GOLD * (currentClaimTimer / Common.CLAIM_TIMER));
        currentGemsClaim = (int)(Common.CLAIM_GEMS * (currentClaimTimer / Common.CLAIM_TIMER));
        timerSlider.value = currentClaimTimer / Common.CLAIM_TIMER;
        goldTxt.text = currentGoldClaim.ToString();
        gemsTxt.text = currentGemsClaim.ToString();

        if (currentClaimTimer < Common.CLAIM_TIMER)
        {
            isFull = false;
        }
            
        else
        {
            isFull = true;
            rwTimerTxt.text = GleyLocalization.Manager.GetText("FULL");
        }
            
    }

    public override void Start()
    {
       
    }

    public override void Update()
    {
        if(currentClaimTimer + Time.deltaTime < Common.CLAIM_TIMER)
        {
            currentClaimTimer += Time.deltaTime;
            rwTimerTxt.text = Common.ConvertTimer(Common.CLAIM_TIMER - currentClaimTimer);
            timerSlider.value = currentClaimTimer / Common.CLAIM_TIMER;
            currentGoldClaim = (int)(Common.CLAIM_GOLD * (currentClaimTimer / Common.CLAIM_TIMER));
            currentGemsClaim = (int)(Common.CLAIM_GEMS * (currentClaimTimer / Common.CLAIM_TIMER));
            goldTxt.text = currentGoldClaim.ToString();
            gemsTxt.text = currentGemsClaim.ToString();
        }
        else
        {
            currentClaimTimer = Common.CLAIM_TIMER;
            if (!isFull)
            {
                rwTimerTxt.text = GleyLocalization.Manager.GetText("FULL");
                isFull = true;
            }
           
        }
    }


    public override void ShowView()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        SetLangText();
        isShow = true;
    }

    public override void HideView()
    {
        base.HideView();
    }

    public void GoBattle()
    {
        GameManager.instance.uiManager.homeView.HideView();
        GameManager.instance.uiManager.homeTab.HideView();
        GameManager.instance.uiManager.gameView.InitView();
        GameManager.instance.uiManager.gameView.ShowView();
        GameManager.instance.InitGame();

        if (GameManager.instance.uiManager.tutView.finishTut1 == 0)
        {
            GameManager.instance.uiManager.tutView.HideView();
            GameManager.instance.uiManager.tutView.finishTut1 = 1;
            PlayerPrefs.SetInt("Tut1", 1);
        }

        if (GameManager.instance.uiManager.tutView.finishTut2 == 0)
        {
            GameManager.instance.uiManager.tutView.ShowTut2();
            PlayerPrefs.SetInt("Tut2", 1);
        }
        
    }

    public override void SetLangText()
    {
        battleTxt.text = GleyLocalization.Manager.GetText("HOME_BATTLE");

        homeWorldTxt.text = string.Format(GleyLocalization.Manager.GetText("HOME_WORLD"), GameManager.instance.currentWorld + 1);

        bestWaveTxt.text = string.Format(GleyLocalization.Manager.GetText("ACHIEVE_TITLE_COMPLETE_WAVE"), GameManager.instance.bestWave);

        unLockTxt.text = GleyLocalization.Manager.GetText("HOME_UNLOCK_INFO");

        claimTxt.text = GleyLocalization.Manager.GetText("IDLE_BAR_FULL");

        if (isFull)
        {
            rwTimerTxt.text = GleyLocalization.Manager.GetText("FULL");
            
        }
    }

    public void ClaimRW()
    {
        GameManager.instance.MoreGold(currentGoldClaim);
        GameManager.instance.MoreGems(currentGemsClaim);
        currentClaimTimer = 0.0f;
        timerSlider.value = currentClaimTimer / Common.CLAIM_TIMER;
        currentGoldClaim = (int)(Common.CLAIM_GOLD * (currentClaimTimer / Common.CLAIM_TIMER));
        currentGemsClaim = (int)(Common.CLAIM_GEMS * (currentClaimTimer / Common.CLAIM_TIMER));
        goldTxt.text = currentGoldClaim.ToString();
        gemsTxt.text = currentGemsClaim.ToString();
        isFull = false;
    }

    public void SaveTimer()
    {
        PlayerPrefs.SetFloat("ClaimTimer", currentClaimTimer);
    }

    public void ShowTWUpView()
    {
        GameManager.instance.uiManager.twUpView.ShowView();
    }
}
