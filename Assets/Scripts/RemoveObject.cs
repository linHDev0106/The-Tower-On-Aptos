using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Remove()
    {
        SimplePool.Despawn(gameObject);
    }
}
