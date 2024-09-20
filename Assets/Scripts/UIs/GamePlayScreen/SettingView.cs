using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GleyLocalization;
using System.Data;
using UnityEngine.SceneManagement;

public class SettingView : BaseView
{
    public TextMeshProUGUI langTxt;

    public Image langImg;

    public GameObject virOn, virOff, alarmOn, alarmOff;

    public Sprite[] langSprList;

    public TextMeshProUGUI settingTxt;

    public TextMeshProUGUI alarmTxt;

    public TextMeshProUGUI soundTxt;

    public TextMeshProUGUI musicTxt;

    public TextMeshProUGUI quitGameTxt;

    public TextMeshProUGUI vibrationTxt;

    public TextMeshProUGUI rateTxt;

    public Slider musicSlider;

    public Slider soundSlider;

    public override void InitView()
    {
        if (LocalizationManager.Instance.GetCurrentLanguage() != SupportedLanguages.ChineseTraditional)
            langTxt.text = LocalizationManager.Instance.GetCurrentLanguage().ToString();
        else
            langTxt.text = "Chinese";
        SetFlag();
    }

    public override void SetLangText()
    {
        settingTxt.text = GleyLocalization.Manager.GetText("SETTING_TITLE");
        alarmTxt.text = GleyLocalization.Manager.GetText("SETTING_ALARM");
        soundTxt.text = GleyLocalization.Manager.GetText("SETTING_SOUND");
        musicTxt.text = GleyLocalization.Manager.GetText("SETTING_MUSIC");
        vibrationTxt.text = GleyLocalization.Manager.GetText("SETTING_VIBRATE");
        rateTxt.text = GleyLocalization.Manager.GetText("SETTING_REVIEW");
        quitGameTxt.text = GleyLocalization.Manager.GetText("QUIT_GAME");
    }

    public override void Start()
    {
        InitView();
    }

    public override void Update()
    {
        
    }

    public override void HideView()
    {
        base.HideView();
    }

    public void SetFlag()
    {
        switch(LocalizationManager.Instance.GetCurrentLanguage())
        {
            case SupportedLanguages.Dutch:
                langImg.sprite = langSprList[0];
                break;

            case SupportedLanguages.English:
                langImg.sprite = langSprList[1];
                break;


            case SupportedLanguages.French:
                langImg.sprite = langSprList[2];
                break;

            case SupportedLanguages.German:
                langImg.sprite = langSprList[3];
                break;

            case SupportedLanguages.Japanese:
                langImg.sprite = langSprList[4];
                break;

            case SupportedLanguages.Korean:
                langImg.sprite = langSprList[5];
                break;

            case SupportedLanguages.Norwegian:
                langImg.sprite = langSprList[6];
                break;

            case SupportedLanguages.Portuguese:
                langImg.sprite = langSprList[7];
                break;

            case SupportedLanguages.Spanish:
                langImg.sprite = langSprList[8];
                break;

            case SupportedLanguages.Vietnamese:
                langImg.sprite = langSprList[9];
                break;

            case SupportedLanguages.ChineseTraditional:
                langImg.sprite = langSprList[10];
                break;
        }
    }

    public void SwitchLang()
    {
        int currentLangIndex = 0;

        for (int i = 0; i < LocalizationManager.Instance.GetLanguageCount(); i++)
        {

            if (LocalizationManager.Instance.GetCurrentLanguage() == LocalizationManager.Instance.GetLanguagueByIndex(i))
                currentLangIndex = i;

        }

        if (currentLangIndex < LocalizationManager.Instance.GetLanguageCount() - 1)
            currentLangIndex++;
        else
            currentLangIndex = 0;

        GleyLocalization.Manager.SetCurrentLanguage(LocalizationManager.Instance.GetLanguagueByIndex(currentLangIndex));
        if (LocalizationManager.Instance.GetCurrentLanguage() != SupportedLanguages.ChineseTraditional)
            langTxt.text = LocalizationManager.Instance.GetCurrentLanguage().ToString();
        else
            langTxt.text = "Chinese";

        SetFlag();

        GameManager.instance.uiManager.UpdateLang();
    }

    public void QuitGame()
    {
        GameManager.instance.SaveAllData();
        if (GameManager.instance.currentState == GameManager.GAME_STATE.PLAYING)
            GameManager.instance.BackToHome();
        else
            Application.Quit();
    }

    public void SetMusic()
    {
        AudioManager.instance.SetMusic(musicSlider.value);
    }

    public void SetSound()
    {
        AudioManager.instance.SetSound(soundSlider.value);
    }

    public void ToggleAlarm()
    {
        AudioManager.instance.btnClickSfx.Play();
        if (AudioManager.instance.isAlarm)
        {
            AudioManager.instance.isAlarm = false;
            alarmOn.SetActive(false);
            alarmOff.SetActive(true);
        }
        else
        {
            AudioManager.instance.isAlarm = true;
            alarmOn.SetActive(true);
            alarmOff.SetActive(false);
        }
    }

    public void ToggleVir()
    {
        AudioManager.instance.btnClickSfx.Play();
        if (AudioManager.instance.isVibration)
        {
            AudioManager.instance.isVibration = false;
            virOn.SetActive(false);
            virOff.SetActive(true);
        }
        else
        {
            AudioManager.instance.isVibration = true;
            virOn.SetActive(true);
            virOff.SetActive(false);
        }
    }

    public void Rate()
    {
#if UNITY_IOS
        Application.OpenURL("");
#endif

#if UNITY_ANDROID
 Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
#endif
    }
}
