using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    public Color[] colorArr;

    public TextMeshPro contentText;

    public enum TYPE
    {
        COIN,
        GOLD,
        CRIT
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitText(string mValue, TYPE currentType)
    {
       
        if (currentType == TYPE.GOLD)
            contentText.color = colorArr[0];
        else if (currentType == TYPE.COIN)
            contentText.color = colorArr[1];
        else if (currentType == TYPE.CRIT)
            contentText.color = colorArr[2];

        contentText.text = mValue;

        transform.localScale = Vector3.one;
        
        transform.DOMoveY(0.25f, 0.25f).SetRelative().SetEase(Ease.Linear).OnComplete
            (
            () =>
            {
                transform.DOScale(Vector3.zero, 0.25f).SetDelay(0.35f).SetEase(Ease.Linear).OnComplete
                (
                    () =>
                    {
                        // Destroy(transform.parent.gameObject);
                        //Debug.Log("Despawn Text");
                        SimplePool.Despawn(gameObject);
                        
                    }
                );
            }
            );
        
        //StartCoroutine(RemoveObj());
    }

    IEnumerator RemoveObj()
    {
        yield return new WaitForSeconds(2.0f);
        SimplePool.Despawn(gameObject);
    }
  
}
