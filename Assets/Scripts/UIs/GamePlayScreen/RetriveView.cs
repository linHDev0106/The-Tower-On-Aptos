using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GleyLocalization;

public class RetriveView : BaseView
{
    public Button gemBtn, rewardBtn;


    public TextMeshProUGUI deadTxt;

    public TextMeshProUGUI notifyTxt;

    public TextMeshProUGUI continueTxt,continueBtnTxt;

    public override void InitView()
    {
        deadTxt.text = GleyLocalization.Manager.GetText("DEAD_RESULT");
        notifyTxt.text = GleyLocalization.Manager.GetText("REVIVAL_GUIDE");
        continueTxt.text = GleyLocalization.Manager.GetText("REVIVAL_REFRESH_CONTINUE");
        continueBtnTxt.text = GleyLocalization.Manager.GetText("REVIVAL_REFRESH_CONTINUE");
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void ShowView()
    {
        base.ShowView();
    }

    public void RetriveByRewardVideo()
    {
        AdsControl.Instance.ShowRewardedAd(AdsControl.REWARD_TYPE.RETRIVE);
    }

    public void RetriveByRewardVideoCB()
    {
        HideView();
        GameManager.instance.uiManager.gameView.GetRetrive();
        GameManager.instance.Retrive();
    }

    public void RetriveByGems()
    {
        if (GameManager.instance.currentGem >= 5)
        {
            HideView();
            GameManager.instance.uiManager.gameView.GetRetrive();
            GameManager.instance.Retrive();
            GameManager.instance.SubGem(5);
        }
        else
        {
            GameManager.instance.uiManager.notiView.InitContent(GleyLocalization.Manager.GetText("ALERT_LACK_GEM"));
            GameManager.instance.uiManager.notiView.ShowView();
        }
    }

    public void Exit()
    {
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0.0f, 0.5f).SetEase(Ease.Linear)
          .OnComplete(() => {

              canvasGroup.alpha = 0.0f;
              canvasGroup.interactable = false;
              canvasGroup.blocksRaycasts = false;
              isShow = false;
              GameManager.instance.uiManager.resultView.ShowView();
          });

       
    }

    public override void SetLangText()
    {
       
    }
}
