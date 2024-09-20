using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCoin : MonoBehaviour
{
    public Transform[] path;

    private int pathIndex;

    private Vector3 nextPos,currentPos;

    public float speed;

    public bool stopFly;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (stopFly)
            return;

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, nextPos) <= 0.1f)
        {
            pathIndex++;
            
            if (pathIndex == path.Length - 1)
            {
                pathIndex = 0;
                speed = 0.0f;
                stopFly = true;
                gameObject.SetActive(false);
            }
            else
            {
                transform.position = path[pathIndex].position;
                nextPos = path[pathIndex + 1].position;
            }
           
            
        }
        
    }

    public void StartFly()
    {
        pathIndex = 0;
        transform.position = path[0].position;
        nextPos = path[1].position;
        speed = 20.0f;
        stopFly = false;
    }
}
