using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballManager : MonoBehaviour
{
    public List<Fireball> fireBallList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitFireBall()
    {

    }

    public void ShowFireBall(int mNum)
    {
        for(int i = 0; i < fireBallList.Count; i++)
        {
            fireBallList[i].Stunt();
        }

        if (mNum > fireBallList.Count)
        {
            mNum = fireBallList.Count;
        }

        for (int i = 0; i < mNum; i++)
        {
            fireBallList[i].Live();
        }

    }

    public void UpdateFireBall()
    {
        for (int i = 0; i < fireBallList.Count; i++)
        {
           
        }
    }
}
