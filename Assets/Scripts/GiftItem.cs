using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftItem : MonoBehaviour
{
    public enum TYPE
    {
        COIN,
        GOLD
    }

    public TYPE type;

    private SpriteRenderer itemSpr;

    public Sprite[] spriteList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitItem()
    {
        itemSpr = GetComponent<SpriteRenderer>();

        if (type == TYPE.COIN)
            itemSpr.sprite = spriteList[0];
        else if (type == TYPE.GOLD)
            itemSpr.sprite = spriteList[1];

        StartCoroutine(RemoveObj());
    }

    public IEnumerator RemoveObj()
    {
        yield return new WaitForSeconds(0.5f);
        SimplePool.Despawn(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
