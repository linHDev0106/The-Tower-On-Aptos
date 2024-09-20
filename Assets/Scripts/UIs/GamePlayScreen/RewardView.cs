using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardView : BaseView
{
    public int bonusCoinTotal;

    public int bonusGemTotal;

    public TextMeshProUGUI coinValueTxt;

    public TextMeshProUGUI gemValueTxt;

    public TextMeshProUGUI rw1Txt;

    public TextMeshProUGUI rw2Txt;

    public TextMeshProUGUI claimTxt;

    public TextMeshProUGUI claimX2Txt;

    public override void InitView()
    {
       
    }

    public void InitValue(int coins, int gems)
    {
        bonusCoinTotal = coins;
        bonusGemTotal = gems;

       
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
        AudioManager.instance.rwPopupSfx.Play();
        coinValueTxt.text = bonusCoinTotal.ToString();
        gemValueTxt.text = bonusGemTotal.ToString();
    }

    public void Claim()
    {
        
        AudioManager.instance.goldRWSfx.Play();
        GameManager.instance.MoreGold(bonusCoinTotal);
        GameManager.instance.MoreGems(bonusGemTotal);
        HideView();
        GameManager.instance.uiManager.homeTab.ChooseHome();
    }

    public void ClaimX2()
    {
        AdsControl.Instance.ShowRewardedAd(AdsControl.REWARD_TYPE.X2GOLD_CHEST);
    }

    public void ClaimX2CB()
    {
        AudioManager.instance.goldRWSfx.Play();
        GameManager.instance.MoreGold(2 * bonusCoinTotal);
        GameManager.instance.MoreGems(2 * bonusGemTotal);
        HideView();
        GameManager.instance.uiManager.homeTab.ChooseHome();
    }

    public override void SetLangText()
    {
        rw1Txt.text = GleyLocalization.Manager.GetText("ACHIEVE_REWARD_TX");
        rw2Txt.text = GleyLocalization.Manager.GetText("DAILY_GIFT_REWARD_TX");
        claimTxt.text = GleyLocalization.Manager.GetText("ACHIEVE_CLAIM");
        claimX2Txt.text = GleyLocalization.Manager.GetText("DAILY_GIFT_CLAIM_VIDEO");
    }
}
