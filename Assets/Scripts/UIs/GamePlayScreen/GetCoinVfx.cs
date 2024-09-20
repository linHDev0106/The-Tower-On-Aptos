using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoinVfx : MonoBehaviour
{
    public FlyCoin[] coinObjectList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCoinVfx()
    {
        StartCoroutine(SpawnCoinVfxIE());
    }

    IEnumerator SpawnCoinVfxIE()
    {
       
        for (int i = 0; i < coinObjectList.Length; i++)
        {
            coinObjectList[i].gameObject.SetActive(true);

            coinObjectList[i].StartFly();

            yield return new WaitForSeconds(0.075f);
        }
    }
}
