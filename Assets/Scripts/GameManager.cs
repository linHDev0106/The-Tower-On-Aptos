using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
//using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ConfigManager configManager;

    public DataManager dataManager;

    public TurretController turretController;

    public EnemyGenerator enemyGenerator;

    public GameObject[] mapList;

    public FireballManager fireBallManager;

    public float enemyAttackRange;

    public GiftItem giftItem;

    public FloatingText giftFLoatingTxt;

    public FloatingText effectFLoatingTxt;

    [HideInInspector]
    public List<GameObject> enemyList;

    public int currentCoin;

    public int currentGold;

    public int goldInGame;

    public int gemsInGame;

    public int currentGem;

    [HideInInspector]
    public int currentKilledEnemy;

    [HideInInspector]
    public int currentKilledBoss;

    [HideInInspector]
    public int currentCrystal;

    [HideInInspector]
    public int currentWorld;

    [HideInInspector]
    public int bestWave;

    [HideInInspector]
    public int remainRetrive;

    [HideInInspector]
    public int turretUnlockAtkLevel;

    [HideInInspector]
    public int turretUnlockHpLevel;

    [HideInInspector]
    public int turretUnlockResourceLevel;

    [HideInInspector]
    public int turretUnlockAlliesLevel;

    [HideInInspector]
    public ParticleSystem upgradeTurretFx;

    [HideInInspector]
    public ParticleSystem clickFx;

    [HideInInspector]
    public ParticleSystem gemsFx;

    [HideInInspector]
    public GetCoinVfx getCoinVfx;

    public UIManager uiManager;

    public enum GAME_STATE
    {
        HOME_MENU,
        PLAYING,
        PAUSING,
        GAME_OVER,
        WIN
    }

    public GAME_STATE currentState;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;

        
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = GAME_STATE.HOME_MENU;
        remainRetrive = 5;
        GetFirstData();
        LoadData();
        //GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Vietnamese);
        CheckSystemLang();
        dataManager.InitData();
        turretController.InitTurret();
        uiManager.InitView();
        uiManager.homeView.InitView();
        uiManager.homeView.ShowView();
        uiManager.gameView.RefreshCurrencyText();
        enemyList = new List<GameObject>();

        if (uiManager.tutView.finishTut1 == 0)
        {
            uiManager.tutView.ShowView();
            uiManager.tutView.ShowTut1();
            uiManager.homeTab.HideView();
            uiManager.gameView.settingBtn.SetActive(false);
        }
        else
        {
            uiManager.gameView.settingBtn.SetActive(true);
            uiManager.homeTab.ShowView();
        }
            
    }

    void CheckSystemLang()
    {
        if (Application.systemLanguage == SystemLanguage.English)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.English);
        }
        else if (Application.systemLanguage == SystemLanguage.Dutch)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Dutch);
        }
        else if (Application.systemLanguage == SystemLanguage.French)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.French);
        }
        else if (Application.systemLanguage == SystemLanguage.German)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.German);
        }
        else if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Japanese);
        }
        else if (Application.systemLanguage == SystemLanguage.Korean)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Korean);
        }
        else if (Application.systemLanguage == SystemLanguage.Norwegian)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Norwegian);
        }
        else if (Application.systemLanguage == SystemLanguage.Portuguese)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Portuguese);
        }
        else if (Application.systemLanguage == SystemLanguage.Spanish)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Spanish);
        }
        else if (Application.systemLanguage == SystemLanguage.Vietnamese)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Vietnamese);
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.ChineseTraditional);
        }
        else if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.ChineseTraditional);
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.ChineseTraditional);
        }
        else
            GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.English);
    }

    public void InitGame()
    {
        currentState = GAME_STATE.PLAYING;
        goldInGame = 0;
        gemsInGame = 0;
        currentKilledEnemy = 0;
        currentKilledBoss = 0;
        RemoveAllEnemies();
        turretController.InitTurret();
        fireBallManager.ShowFireBall(Mathf.RoundToInt((float)GameManager.instance.dataManager.turretSatelliteUpgradeData[0].currentValue));
        enemyGenerator.InitLevel();
        uiManager.gameView.InitView();
        uiManager.gameView.mainBoard.IniteView();
        LoadWorld();
    }

    public void LoadWorld()
    {
        for (int i = 0; i < mapList.Length; i++)
            mapList[i].SetActive(false);

        mapList[currentWorld].SetActive(true);
    }

    void RemoveAllEnemies()
    {
        if (enemyList.Count == 0)
            return;

        for(int i = 0; i < enemyList.Count; i++)
        {
            Destroy(enemyList[i]);
            //SimplePool.Despawn(enemyList[i]);
        }

        enemyList.Clear();
    }

    public void BackToHome()
    {
        DefaultSpeed();
        RemoveAllEnemies();
        enemyGenerator.StopAllCoroutines();
        uiManager.settingView.HideView();
        uiManager.homeView.ShowView();
        uiManager.homeTab.ShowView();

        uiManager.gameView.HideView();
        
        uiManager.resultView.HideView();
        currentState = GAME_STATE.HOME_MENU;

        currentCoin = 0;
        goldInGame = 0;
        gemsInGame = 0;
        currentKilledEnemy = 0;
        currentKilledBoss = 0;
        dataManager.Refresh();
        uiManager.gameView.RefreshCurrencyText();

        if (uiManager.tutView.finishTut1 == 1)
        {
            uiManager.gameView.settingBtn.SetActive(true);
            
        }

        if (uiManager.tutView.finishTut3 == 0)
        {
            uiManager.tutView.ShowTut3();

        }

    }

    public void Retrive()
    {
        turretController.RemoveAllEnemiesInRange();
        currentState = GAME_STATE.PLAYING;
        turretController.InitTurret();
       
      
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
            InitGame();
        if (Input.GetKeyDown(KeyCode.R))
            Retrive();

        if (Input.GetKeyDown(KeyCode.G))
            MoreGold(10000);
        if (Input.GetKeyDown(KeyCode.Q))
            MoreGems(40);

        if (Input.GetKeyDown(KeyCode.S))
            SaveAllData();

    }

    public void PlayClickFx()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousPos2D = new Vector2(mousePos.x, mousePos.y);
        clickFx.transform.position = mousPos2D;
        clickFx.Play();
    }

    //to get if the palyer click on an ui element or on an object
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


  

    public void ChangeGameSpeed()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 1.5f;
        }
        else if (Time.timeScale == 1.5f)
        {
            Time.timeScale = 2.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void DefaultSpeed()
    {
        Time.timeScale = 1.0f;
        uiManager.gameView.RefreshSpeedText();
    }

    IEnumerator GenGiftIE(int coinNum, int goldNum, Vector3 genPos)
    {
        yield return new WaitForSeconds(0.4f);

        //GiftItem coinItem = Instantiate(giftItem, genPos, Quaternion.identity);
        GameObject coinItemObj = SimplePool.Spawn(giftItem.gameObject, genPos, Quaternion.identity);
        GiftItem coinItem = coinItemObj.GetComponent<GiftItem>();
        coinItem.type = GiftItem.TYPE.COIN;
        coinItem.InitItem();

        //FloatingText mTextCoin = Instantiate(giftFLoatingTxt, genPos, Quaternion.identity);
        GameObject mTextCoinObj = SimplePool.Spawn(giftFLoatingTxt.gameObject, genPos, Quaternion.identity);
        FloatingText mTextCoin = mTextCoinObj.GetComponent<FloatingText>();
        mTextCoin.InitText("+" + coinNum, FloatingText.TYPE.COIN);
        currentCoin += coinNum * ResourceBoost.Instance.boostValue;

        if (goldNum > 0)
        {
            yield return new WaitForSeconds(0.5f);

            Vector3 randomPos = genPos + new Vector3(Random.Range(0.0f, 0.1f), Random.Range(0.0f, 0.1f));

            //GiftItem goldItem = Instantiate(giftItem, randomPos, Quaternion.identity);
            GameObject goldItemObj = SimplePool.Spawn(giftItem.gameObject, randomPos, Quaternion.identity);
            GiftItem goldItem = goldItemObj.GetComponent<GiftItem>();
            goldItem.type = GiftItem.TYPE.GOLD;
            goldItem.InitItem();
            goldInGame += goldNum * ResourceBoost.Instance.boostValue;

            // FloatingText mTextGold = Instantiate(giftFLoatingTxt, randomPos, Quaternion.identity);
            GameObject mTextGoldObj = SimplePool.Spawn(giftFLoatingTxt.gameObject, randomPos, Quaternion.identity);
            FloatingText mTextGold = mTextGoldObj.GetComponent<FloatingText>();
            mTextGold.InitText("+" + goldNum, FloatingText.TYPE.GOLD);
        }

       
        
        uiManager.gameView.RefreshCurrencyText();

    }

    public void GenGift(int coinNum, int goldNum, Vector3 genPos)
    {
        StartCoroutine(GenGiftIE(coinNum,goldNum,genPos));
    }

    public void SpawnFLoatingTxt(string mContent, FloatingText.TYPE mType, Vector3 genPos)
    {
        //FloatingText mTextGold = Instantiate(giftFLoatingTxt, genPos, Quaternion.identity);
        GameObject mTextGoldObj = SimplePool.Spawn(giftFLoatingTxt.gameObject, genPos, Quaternion.identity);
        FloatingText mTextGold = mTextGoldObj.GetComponent<FloatingText>();
        mTextGold.InitText(mContent, mType);
    }

    public void SpawnEffectTxt(string mContent, FloatingText.TYPE mType, Vector3 genPos)
    {
        //FloatingText mTextGold = Instantiate(giftFLoatingTxt, genPos, Quaternion.identity);
        GameObject mTextGoldObj = SimplePool.Spawn(effectFLoatingTxt.gameObject, genPos, Quaternion.identity);
        FloatingText mTextGold = mTextGoldObj.GetComponent<FloatingText>();
        mTextGold.InitText(mContent, mType);
    }

    public void CalBonusReward()
    {
        int goldBonus1 = Mathf.RoundToInt((float)GameManager.instance.dataManager.turretCoinUpgradeData[0].currentValue) * (enemyGenerator.currentWave + 1);

        //Debug.Log("GOLD 1 " + goldBonus1);

        int gemBonus = Mathf.RoundToInt((float)GameManager.instance.dataManager.turretCoinUpgradeData[1].currentValue) * (currentKilledEnemy/50);

        //Debug.Log("GOLD 2 " + gemBonus);

        int goldBonus2 = Mathf.RoundToInt((float)GameManager.instance.dataManager.turretCoinUpgradeData[2].currentValue) * (currentKilledEnemy / 100);

        //Debug.Log("GOLD 1 " + goldBonus2);

        int goldBonus3 = Mathf.RoundToInt((float)GameManager.instance.dataManager.turretCoinUpgradeData[3].currentValue) * currentKilledBoss;

        //Debug.Log("GOLD 1 " + goldBonus3);

        goldInGame = goldInGame + goldBonus1 + goldBonus2 + goldBonus3;
        gemsInGame = gemsInGame + gemBonus;
    }

    public void GetRWGameOver()
    {
        MoreGold(goldInGame);

        MoreGems(gemsInGame);

        
    }

    public void GetRWGameOverX2()
    {
        MoreGold(2*goldInGame);

        MoreGems(2*gemsInGame);


    }


    public void GetFirstData()
    {
        if(PlayerPrefs.GetInt("FirstData") == 0)
        {
            PlayerPrefs.SetInt("RemainRetrive", 5);
            PlayerPrefs.SetInt("FirstData", 1);
        }


    }

    public void LoadData()
    {
        currentGold = PlayerPrefs.GetInt("Gold") + ResourceBoost.Instance.goldBoost;
        currentGem = PlayerPrefs.GetInt("Gem") + ResourceBoost.Instance.gemBoost;
        ResourceBoost.Instance.ResetBoostValue();
        currentCrystal = PlayerPrefs.GetInt("Crystal");
        turretUnlockAtkLevel = PlayerPrefs.GetInt("TurretUnlockAtkLevel");
        turretUnlockHpLevel = PlayerPrefs.GetInt("TurretUnlockHpLevel");
        turretUnlockResourceLevel = PlayerPrefs.GetInt("TurretUnlockResourceLevel");
        turretUnlockAlliesLevel = PlayerPrefs.GetInt("TurretUnlockAlliesLevel");
        remainRetrive = PlayerPrefs.GetInt("RemainRetrive");
        uiManager.gameView.timer = Common.COUNT_DOWN_TIMER;
        currentWorld = PlayerPrefs.GetInt("World");
        bestWave = PlayerPrefs.GetInt("BestWave");

        uiManager.tutView.finishTut1 = PlayerPrefs.GetInt("Tut1");
        uiManager.tutView.finishTut2 = PlayerPrefs.GetInt("Tut2");
        uiManager.tutView.finishTut3 = PlayerPrefs.GetInt("Tut3");
        uiManager.tutView.finishTut4 = PlayerPrefs.GetInt("Tut4");
    }

    public void SaveAllData()
    {
        SaveCurrency();
        PlayerPrefs.SetInt("RemainRetrive", remainRetrive);
        PlayerPrefs.SetInt("World", currentWorld);
        uiManager.storeView.SaveTimer();
        uiManager.homeView.SaveTimer();
    }

    public void MoreGold(int moreGold)
    {
        

        //uiManager.gameView.coinIconInBoard.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        //uiManager.shopView.coinIconInBoard.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        AudioManager.instance.flyingCoinSfx.Play();
        getCoinVfx.SpawnCoinVfx();
        //uiManager.gameView.coinIconInBoard.DOScale(1f, 0.25f).SetDelay(0.75f).SetEase(Ease.Linear);

        int currentMoreCoin = 0;

        DOTween.To(() => currentMoreCoin, x => currentMoreCoin = x, moreGold, 1.0f).SetEase(Ease.Linear)

            .OnUpdate(() =>
            {

                //uiManager.gameView.coinTxt.text = (GameManager.instance.currentCoin + currentMoreCoin).ToString();
                //uiManager.shopView.coinTxt.text = (GameManager.instance.currentCoin + currentMoreCoin).ToString();

            })
         .OnComplete(() =>
         {
             //GameManager.instance.currentCoin = GameManager.instance.currentCoin + currentMoreCoin;
             //uiManager.gameView.coinTxt.text = GameManager.instance.currentCoin.ToString();
             // uiManager.shopView.coinTxt.text = GameManager.instance.currentCoin.ToString();
             //GameManager.instance.SaveCoin();
             currentGold += moreGold;
             PlayerPrefs.SetInt("Gold", currentGold);
             uiManager.gameView.RefreshCurrencyText();
         });
    }

    public void MoreGems(int moreGems)
    {
        currentGem += moreGems;
        PlayerPrefs.SetInt("Gem", currentGem);
        gemsFx.Play();
        uiManager.gameView.RefreshCurrencyText();
    }

    public void SubGold(int subGold)
    {
        currentGold -= subGold;
        PlayerPrefs.SetInt("Gold", currentGold);
        uiManager.gameView.RefreshCurrencyText();
    }

    public void MoreX4Coin()
    {
        currentCoin *= 4;
        uiManager.gameView.RefreshCurrencyText();
    }

    public void SubGem(int subGem)
    {
        currentGem -= subGem;
        PlayerPrefs.SetInt("Gem", currentGem);
        uiManager.gameView.RefreshCurrencyText();
    }

    public void SaveCurrency()
    {
        PlayerPrefs.SetInt("Gold", currentGold);
        PlayerPrefs.SetInt("Gem", currentGem);
    }

    void OnApplicationQuit()
    {
        SaveAllData();
    }
    bool isPaused = false;
    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
        if (isPaused)
            SaveAllData();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
        if (isPaused)
            SaveAllData();
    }

  
}
