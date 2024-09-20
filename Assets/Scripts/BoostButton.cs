using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoostButton : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float timer;

    public float cowntDownTimer;


    private bool activeTimer;

    private void Awake()
    {
        activeTimer = false;
        timer = 0.0f;
        timerText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!activeTimer)
            return;

        if(timer > 0.0f)
        {
            timer -= Time.deltaTime;
            timerText.text = ConvertTimer(timer);
        }
        else
        {
            timer = 0.0f;
            timerText.gameObject.SetActive(false);
           // LemonStandManager.instance.gameData.multiCoin = 0;
            activeTimer = false;
        }
    }

    public void AddMoreMinutes()
    {
        timer += 300.0f;
    }

    public void ActiveTimer()
    {
        timerText.gameObject.SetActive(true);
      //  LemonStandManager.instance.gameData.multiCoin = 1;
        if (!activeTimer)
        {
            activeTimer = true;
            timer = cowntDownTimer;
        }
        else
            AddMoreMinutes();
    }

    string ConvertTimer(float _timer)
    {
        string timeFormat = "";

        int minus = Mathf.FloorToInt(_timer / 60.0f);

        int second = Mathf.FloorToInt(_timer - (float)minus * 60.0f);

        timeFormat = minus + "m" + " " + + second + "s";

        return timeFormat;
    }
}
