using UnityEngine;
using System.Collections;

/// <summary>
/// Define type process sortin layer ( Update or One time)
/// </summary>
public enum TypeSorting
{
    Update,
    OneTime,
    Initial
}

/// <summary>
/// This class make sorting layer change base on y axist
/// </summary>
public class SortingLayerBaseOnYAxis : MonoBehaviour {
    public TypeSorting type = TypeSorting.Initial;

    private SpriteRenderer[] elementSprites;
    private int[] defaultSortingLayers;
    private float lastPosYAxist;

    private IEnumerator coroutineUpdateSprite;
    private readonly int offset = 10000   ;

    void Awake()
    {
        elementSprites = GetComponentsInChildren<SpriteRenderer>(true);
        defaultSortingLayers = new int[elementSprites.Length];
        for (int i = 0; i < elementSprites.Length; i++)
            defaultSortingLayers[i] = elementSprites[i].sortingOrder;
    }

    void OnEnable()
    {
        if (type == TypeSorting.Update)
        {
            // Initial sorting layer
            for (int i = 0; i < elementSprites.Length; i++)
                elementSprites[i].sortingOrder = -(int)(transform.position.y * offset) + defaultSortingLayers[i];
            // Save last postion
            lastPosYAxist = transform.position.y;
            // Start update frequently
            coroutineUpdateSprite = UpdateSprite();
            StartCoroutine(coroutineUpdateSprite);
        }
        else if( type == TypeSorting.OneTime)
        {
            // Only initial sorting layer when oject active
            for (int i = 0; i < elementSprites.Length; i++)
                elementSprites[i].sortingOrder = -(int)(transform.position.y * offset) + defaultSortingLayers[i];
        }
    }

    void OnDisable()
    {
        if (coroutineUpdateSprite != null)
            StopCoroutine(coroutineUpdateSprite);
    }

    /// <summary>
    /// Method call inital sorting layer that will overrite initial OnEnable() method
    /// </summary>
    /// <param name="yAxis">Postion initial</param>
    public void Initial(float yAxis, TypeSorting typeSorting = TypeSorting.Initial)
    {
        this.type = typeSorting;
        // Disable coroutine when new initial
        if (coroutineUpdateSprite != null)
            StopCoroutine(coroutineUpdateSprite);

        // If not have data components, get components
        if (elementSprites == null)
        {
            elementSprites = GetComponentsInChildren<SpriteRenderer>(true);
            defaultSortingLayers = new int[elementSprites.Length];
            for (int i = 0; i < elementSprites.Length; i++)
                defaultSortingLayers[i] = elementSprites[i].sortingOrder;
        }

        // Inital sorting layer
        for (int i = 0; i < elementSprites.Length; i++)
            elementSprites[i].sortingOrder = -(int)(yAxis * offset) + defaultSortingLayers[i];

        switch (typeSorting) {
            case TypeSorting.Initial:                     
                break;
            case TypeSorting.Update:
               // Initial sorting layer
                for (int i = 0; i < elementSprites.Length; i++)
                    elementSprites[i].sortingOrder = -(int)(transform.position.y * offset) + defaultSortingLayers[i];
                // Save last postion
                lastPosYAxist = transform.position.y;
                // Start update frequently
                coroutineUpdateSprite = UpdateSprite();
                StartCoroutine(coroutineUpdateSprite);
                break;
    }
    }

    IEnumerator UpdateSprite()
    {
        while (true)
        {
            if (Mathf.Abs(lastPosYAxist - transform.position.y) > 0.001f)
            {
                for (int i = 0; i < elementSprites.Length; i++) 
                    elementSprites[i].sortingOrder = -((int)(transform.position.y * offset)) + defaultSortingLayers[i];
            }

            lastPosYAxist = transform.position.y;
            yield return null;
        }
    }

}
