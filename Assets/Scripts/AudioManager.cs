using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource turretShotSfx;

    public AudioSource enemyDeathSfx;

    public AudioSource enemyAppearSfx;

    public AudioSource btnClickSfx;

    public AudioSource upgradeSfx;

    public AudioSource gameOverSfx;

    public AudioSource goldRWSfx;

    public AudioSource tickSfx;

    public AudioSource rwPopupSfx;

    public AudioSource flyingCoinSfx;

    public AudioSource swordAtkSfx;

    public AudioSource backgroundMusic;

    public AudioSource[] soundList;

    public bool isVibration;

    public bool isAlarm;

    public static AudioManager instance;

    private void Awake()
    {
        /*
        int musicState = PlayerPrefs.GetInt("Music");
        if (musicState == 0)
        {
          
            ToogleMusic(true);
           

        }
        else
        {
           
            ToogleMusic(false);
          

        }

        int soundState = PlayerPrefs.GetInt("Sound");
        if (soundState == 0)
        {
           
            ToogleSound(true);
           
        }
        else
        {
         
            ToogleSound(false);
            
        }
        
        */
        if (FindObjectsOfType(typeof(AudioManager)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

       
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        isVibration = true;
        isAlarm = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToogleMusic(bool toogle)
    {
        if(toogle)
          backgroundMusic.volume = 1.0f;
        else
            backgroundMusic.volume = 0.0f;
    }

    public void ToogleSound(bool toogle)
    {
        if (toogle)
        {

            for (int i = 0; i < soundList.Length; i++)
                soundList[i].volume = 1.0f;

        }

        else
        {
            for (int i = 0; i < soundList.Length; i++)
                soundList[i].volume = 0.0f;


        }
    }

    public void SetMusic(float volume)
    {
        backgroundMusic.volume = volume;
    }

    public void SetSound(float volume)
    {
        for (int i = 0; i < soundList.Length; i++)
            soundList[i].volume = volume;
    }


    public void PlayBoss1()
    {
        backgroundMusic.volume = 0.0f;
        
    }

    public void PlayWin()
    {
        backgroundMusic.volume = 0.0f;
    }
}
