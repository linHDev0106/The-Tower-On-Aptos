using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    private Vector3 moveDirection;

    private float moveSpeed;

    public GameObject projectileFx;

    public float attack;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed, Space.World);
    }

    public void Initial(Vector3 direction, float speed, float mAttack)
    {
        moveDirection = direction;
        moveSpeed = speed;
        attack = mAttack;
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tower")
        {
            collision.GetComponent<TurretController>().Damage(attack);

            if (collision.GetComponent<TurretController>().HP > 0)
            {
                //Instantiate(projectileFx, transform.position, Quaternion.identity);
                SimplePool.Spawn(projectileFx, transform.position, Quaternion.identity);
            }
                

            Destroy(gameObject);
        }
    }
}
