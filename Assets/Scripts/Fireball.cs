using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float degreesPerSecond = 45;

    public GameObject projectileFx;

    public SpriteRenderer mRen;

    public CircleCollider2D mCol;

    private void Awake()
    {
        mRen = GetComponent<SpriteRenderer>();
        mCol = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.forward, degreesPerSecond * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {

            collision.GetComponent<EnemyController>().Damage((float)GameManager.instance.dataManager.turretSatelliteUpgradeData[1].currentValue * 0.01f);

            if (collision.GetComponent<EnemyController>().HP > 0)
                Instantiate(projectileFx, transform.position, Quaternion.identity);

            StartCoroutine(StuntIE());
        }
    }

    IEnumerator StuntIE()
    {
        Stunt();
        yield return new WaitForSeconds(1.0f / (float)GameManager.instance.dataManager.turretSatelliteUpgradeData[2].currentValue);
        Live();
    }

    public void Stunt()
    {
        mRen.enabled = false;
        mCol.enabled = false;
    }

    public void Live()
    {
        mRen.enabled = true;
        mCol.enabled = true;
    }
}
