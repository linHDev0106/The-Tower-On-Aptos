using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common 
{
    public static float COUNT_DOWN_TIMER = 900;

    public static float CLAIM_TIMER = 1800;

    public static float CLAIM_GOLD = 500;

    public static float CLAIM_GEMS = 5;

    public static float REWARD_TIMER = 1800;

    public static int SumSequence(int n)
    {
        int sum = 0;

        sum = n * (n + 1) / 2;

        return sum;
    }

    public static double CalTurretAttackValue(double startValue, double levelUpBase, int currentLv, double levelUpMulti)
    {
        double currentValue = 0;

        if (currentLv == 0)
            currentValue = startValue;
        else
        currentValue = startValue + currentLv * levelUpBase + (double)Common.SumSequence(currentLv) * levelUpMulti;

        return currentValue;
    }

    public static int CalTurretAttackPrice(int startPrice, int levelUpBasePrice, int currentLv, int levelUpMultiPrice)
    {
        int currentValue = 0;

        if (currentLv == 0)
            currentValue = startPrice;
        else
            currentValue = startPrice + currentLv * levelUpBasePrice + Common.SumSequence(currentLv) * levelUpMultiPrice;

        return currentValue;
    }

    public static double CalTurretHpValue(double startValue, double levelUpBase, int currentLv, double levelUpMulti)
    {
        double currentValue = 0;

        if (currentLv == 0)
            currentValue = startValue;
        else
            currentValue = startValue + currentLv * levelUpBase + (double)Common.SumSequence(currentLv - 1) * levelUpMulti;

        return currentValue;
    }

    public static int CalTurretHpPrice(int startPrice, int levelUpBasePrice, int currentLv, int levelUpMultiPrice)
    {
        int currentValue = 0;

        if (currentLv == 0)
            currentValue = startPrice;
        else
            currentValue = startPrice + currentLv * levelUpBasePrice + Common.SumSequence(currentLv - 1) * levelUpMultiPrice;

        return currentValue;
    }

    public static bool IsRate(float rate)
    {
        bool isRate = false;

       // int roundPercent = Mathf.RoundToInt(rate * 100);

        float randomNum = Random.Range(0.0f, 1.0f);

        if (randomNum <= rate)
            isRate = true;

        return isRate;
    }

    public static string ConvertTimer(float _timer)
    {
        string timeFormat = "";

        int minus = Mathf.FloorToInt(_timer / 60.0f);

        int second = Mathf.FloorToInt(_timer - (float)minus * 60.0f);

        timeFormat = minus + "m" + " " + +second + "s";

        return timeFormat;
    }

    public static string ConvertTimerHour(float _timer)
    {
        string timeFormat = "";

        int hour = Mathf.FloorToInt(_timer / 3600.0f);

        int minus = Mathf.FloorToInt((_timer - (float)hour * 3600.0f)/60.0f);

        timeFormat = hour + "h" + " " + minus + "m";

        return timeFormat;
    }


    public static GameObject FindNearestObject(Collider2D[] objArr, Transform centerPoint)
    {
        GameObject nearestObj = objArr[0].gameObject;

        float nearestDistance = Vector2.Distance(nearestObj.transform.position, centerPoint.position);

        for(int i = 1; i <objArr.Length; i++)
        {
            if (Vector2.Distance(objArr[i].transform.position, nearestObj.transform.position) < nearestDistance)
            {
                nearestObj = objArr[i].gameObject;
                nearestDistance = Vector2.Distance(objArr[i].transform.position, nearestObj.transform.position);
            }
        }

        return nearestObj;
    }
}
