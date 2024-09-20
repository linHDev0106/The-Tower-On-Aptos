using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotifyView : BaseView
{
    public TextMeshProUGUI contentTxt;

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

    public override void ShowView()
    {
        base.ShowView();
        StartCoroutine(HideViewIE());
    }

    IEnumerator HideViewIE()
    {
        yield return new WaitForSeconds(1.0f);
        HideView();
    }

    public override void HideView()
    {
        base.HideView();
    }

    public void InitContent(string _value)
    {
        contentTxt.text = _value;
    }
}
