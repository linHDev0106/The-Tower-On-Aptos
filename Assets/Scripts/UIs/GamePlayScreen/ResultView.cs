using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultView : BaseView
{
    public TextMeshProUGUI finishTxt, bestWaveTxt, receiveRewardTxt, receivex2RewardTxt;

    public TextMeshProUGUI goldValueTxt,gemValueTxt;

    public TextMeshProUGUI worldIDxt;

    public TextMeshProUGUI waveProgressTxt;

    public Slider waveProgressSlider;

    public override void InitView()
    {
        waveProgressTxt.text = GameManager.instance.enemyGenerator.currentWave + "/" + GameManager.instance.enemyGenerator.levelData[GameManager.instance.currentWorld].listEnemyWave.Count;
        waveProgressSlider.value = (float)GameManager.instance.enemyGenerator.currentWave/(float)GameManager.instance.enemyGenerator.levelData[GameManager.instance.currentWorld].listEnemyWave.Count;
        worldIDxt.text = GameManager.instance.enemyGenerator.levelData[GameManager.instance.currentWorld].worldID.ToString();

        int bestWavevalue = PlayerPrefs.GetInt("BestWave");

        if (GameManager.instance.enemyGenerator.currentWave >= bestWavevalue)
        {
           
            bestWavevalue = GameManager.instance.enemyGenerator.currentWave;
            PlayerPrefs.SetInt("BestWave", bestWavevalue);
        }

        GameManager.instance.bestWave = bestWavevalue;


        finishTxt.text = GleyLocalization.Manager.GetText("LAB_FINISH");
        bestWaveTxt.text = string.Format(GleyLocalization.Manager.GetText("GAME_OVER_BEST_WAVE"), bestWavevalue);
        receiveRewardTxt.text = GleyLocalization.Manager.GetText("GO_CLAIM_REWARD");
        receivex2RewardTxt.text = "X2 " + GleyLocalization.Manager.GetText("GO_CLAIM_REWARD");
        GameManager.instance.CalBonusReward();
        goldValueTxt.text = GameManager.instance.goldInGame.ToString();
        gemValueTxt.text = GameManager.instance.gemsInGame.ToString();

        //FirebaseManager.instance.LogScreen("WORLD " + GameManager.instance.currentWorld);
        //FirebaseManager.instance.LogScreen("BEST WAVE " + GameManager.instance.bestWave);

        if (GameManager.instance.currentState == GameManager.GAME_STATE.WIN)
        {
            GameManager.instance.currentWorld++;
            PlayerPrefs.SetInt("World", GameManager.instance.currentWorld);
        }

        StartCoroutine(ShowAds());
    }

    IEnumerator ShowAds()
    {
        yield return new WaitForSeconds(1.0f);
        AdsControl.Instance.ShowInterstital();
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
        InitView();
    }

    public override void HideView()
    {
        base.HideView();
    }

    public void Claim()
    {
        GameManager.instance.GetRWGameOver();
        GameManager.instance.BackToHome();
    }

    public void ClaimX2CB()
    {
        GameManager.instance.GetRWGameOverX2();
        GameManager.instance.BackToHome();
    }

    public void ClaimX2()
    {
        AdsControl.Instance.ShowRewardedAd(AdsControl.REWARD_TYPE.X2GOLD_GAME_OVER);
    }

    public override void SetLangText()
    {
        
    }
}
