using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretProjectile : MonoBehaviour
{
    private Vector3 moveDirection;

    private float moveSpeed;

    public GameObject projectileFx;

    public float attack;

    public bool isCrit;

    public List<EnemyController> nextTargetList = new List<EnemyController>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed, Space.World);
    }

    public void Initial(Vector3 direction, float speed, float mAttack, bool mIsCrit)
    {
        moveDirection = direction;
        moveSpeed = speed;
        attack = mAttack;
        isCrit = mIsCrit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().Damage(attack);

            if(collision.GetComponent<EnemyController>().HP > 0)
                SimplePool.Spawn(projectileFx, transform.position, Quaternion.identity);

            if (isCrit)
                GameManager.instance.SpawnEffectTxt(GleyLocalization.Manager.GetText("LAB_CRIT_PER"), FloatingText.TYPE.CRIT, collision.transform.position);

            //Destroy(gameObject);
            SimplePool.Despawn(gameObject);
        }

        if(collision.tag == "OutMap")
        {
           /// Debug.Log("OUT MAP");
            SimplePool.Despawn(gameObject);
        }
            
    }

   
}
