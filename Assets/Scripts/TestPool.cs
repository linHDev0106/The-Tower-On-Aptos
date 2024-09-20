using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    public GameObject testObject1, testObject2;

    public List<GameObject> currentObject1 = new List<GameObject>();

    public List<GameObject> currentObject2 = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //SimplePool.Preload(testObject, 10);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            currentObject1.Add(SimplePool.Spawn(testObject1, Vector3.zero, Quaternion.identity));

        if (Input.GetKeyDown(KeyCode.D))
        {
            if(currentObject1.Count >= 1)
            {
                SimplePool.Despawn(currentObject1[currentObject1.Count - 1]);
                currentObject1.Remove(currentObject1[currentObject1.Count - 1]);
            }
              

        }

        if (Input.GetKeyDown(KeyCode.F))
            currentObject2.Add(SimplePool.Spawn(testObject2, Vector3.zero, Quaternion.identity));

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentObject2.Count >= 1)
            {
                SimplePool.Despawn(currentObject2[currentObject2.Count - 1]);
                currentObject2.Remove(currentObject2[currentObject2.Count - 1]);
            }


        }
    }
}
