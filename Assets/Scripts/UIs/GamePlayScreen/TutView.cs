using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutView : BaseView
{
    public TextMeshProUGUI text1, text2;

    public GameObject hand1, hand2, hand3, hand4,dialog;

    [HideInInspector]
    public int finishTut1, finishTut2, finishTut3, finishTut4;

    public override void InitView()
    {
       
    }

    public override void SetLangText()
    {
       
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        
    }

    public void ShowTut1()
    {
        dialog.SetActive(true);
        text1.text = GleyLocalization.Manager.GetText("TUT_TITLE");
        text2.text = GleyLocalization.Manager.GetText("TUT0");
        hand1.SetActive(true);
        hand2.SetActive(false);
        hand3.SetActive(false);
        hand4.SetActive(false);
    }

    public void ShowTut2()
    {
        StartCoroutine(ShowTut2IE());
    }

    public IEnumerator ShowTut2IE()
    {
        yield return new WaitForSeconds(2.0f);
        ShowView();
        dialog.SetActive(true);
        text1.text = GleyLocalization.Manager.GetText("TUT_TITLE");
        text2.text = GleyLocalization.Manager.GetText("TUT1");
        hand1.SetActive(false);
        hand2.SetActive(true);
        hand3.SetActive(false);
        hand4.SetActive(false);
        StartCoroutine(DisTut2IE());
    }

    public void ShowTut3()
    {
        ShowView();
        dialog.SetActive(true);
        text1.text = GleyLocalization.Manager.GetText("TUT_TITLE");
        text2.text = GleyLocalization.Manager.GetText("TUT2");
        hand1.SetActive(false);
        hand2.SetActive(false);
        hand3.SetActive(true);
        hand4.SetActive(false);
        StartCoroutine(DisTut3IE());
    }

    public void ShowTut4()
    {
        ShowView();
        dialog.SetActive(false);
        hand1.SetActive(false);
        hand2.SetActive(false);
        hand3.SetActive(false);
        hand4.SetActive(true);
        StartCoroutine(DisTut4IE());
    }

    public IEnumerator DisTut2IE()
    {
        yield return new WaitForSeconds(3.0f);
        HideView();
        finishTut2 = 1;
        PlayerPrefs.SetInt("Tut2", 1);
    }

    public IEnumerator DisTut3IE()
    {
        yield return new WaitForSeconds(3.0f);
        HideView();
        finishTut3 = 1;
        PlayerPrefs.SetInt("Tut3", 1);
    }

    public IEnumerator DisTut4IE()
    {
        yield return new WaitForSeconds(3.0f);
        HideView();
        finishTut4 = 1;
        PlayerPrefs.SetInt("Tut4", 1);
    }
}
