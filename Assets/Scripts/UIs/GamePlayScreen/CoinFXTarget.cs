using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFXTarget : MonoBehaviour
{
    public float side;
    // Use this for initialization
    void Start()
    {
        /*
        transform.position = new Vector3(side * (Camera.main.orthographicSize * Camera.main.aspect), side * (Camera.main.orthographicSize * Camera.main.aspect), transform.position.z);

        var bottomLeft = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane));
        var topLeft = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Camera.main.farClipPlane));
        var topRight = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.farClipPlane));
        var bottomRight = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.farClipPlane));
        */

        //var bottomLeft = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        // var topLeft = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Camera.main.nearClipPlane));
        // var topRight = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.nearClipPlane));
        //var bottomRight = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.nearClipPlane));

        UpdatePos();
    }


    public void UpdatePos()
    {
        
        var topLeft = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Camera.main.farClipPlane));
        transform.position = topLeft + new Vector2(0.5f, -0.5f);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
