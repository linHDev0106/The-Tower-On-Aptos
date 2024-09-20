using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBoost : MonoBehaviour
{
    // Singleton instance
    public static ResourceBoost Instance { get; private set; }

    // Variable to store the resource boost value
    public int goldBoost = 0;
    public int gemBoost = 0;
    public int boostValue = 1;

    //Reset number when gameOver.

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetBoostValue()
    {
        goldBoost = 0;
        gemBoost = 0;

        Debug.Log("goldBoost: " + goldBoost);
        Debug.Log("gemBoost: " + gemBoost);
        Debug.Log("boostValue: " + boostValue);
    }
}