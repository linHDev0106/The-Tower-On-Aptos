using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    public Transform turretCenter;

    public Transform firePoint;

    public Transform attackRangeSpr;

    public float attackRange;

    public float shootingRate;

    public float nextFire;

    public LayerMask enemyMask;

    public enum STATE
    {
        WAIT,
        ATTACK,
        DEFEAT
    }

    public STATE currentState;

    public EnemyController currentTarget;

    public TurretProjectile turretProjectile;

    public TurretProjectile turretProjectileCrit;

    [HideInInspector]
    public int ATK_LV;

    [HideInInspector]
    public int HP_LV;

    public TurretConfig turretConfig;

    [HideInInspector]
    public float HP;

    [HideInInspector]
    public float HPMax;

    [HideInInspector]
    public float ShieldHP;

    [HideInInspector]
    public float ShieldHPMax;

    [HideInInspector]
    public float RegenHP;

    [HideInInspector]
    public float RegenShield;

    [HideInInspector]
    private float regenTimer;

    [HideInInspector]
    private float regenShieldTimer;

    [HideInInspector]
    public float attack;

    public ParticleSystem shieldFx;

    public ParticleSystem dustFx;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitTurret()
    {
        LoadTurretConfig();
    }

    void LoadTurretConfig()
    {
        // ATK_LV = PlayerPrefs.GetInt("ATK");
        // HP_LV = PlayerPrefs.GetInt("HP");
        currentState = STATE.WAIT;
        ATK_LV = 5;
        HP_LV = 5;

        turretConfig = GameManager.instance.configManager.turretConfig;

        HPMax = Mathf.RoundToInt((float)GameManager.instance.dataManager.turretHpUpgradeData[0].currentValue);
        HP = HPMax;
        GameManager.instance.uiManager.gameView.UpdateTurretHPSlider(HP, HPMax);

        attack = (float)GameManager.instance.dataManager.turretAttackUpgradeData[0].currentValue;

        RegenHP = (float)GameManager.instance.dataManager.turretHpUpgradeData[1].currentValue;

        shootingRate = 1.0f / (float)GameManager.instance.dataManager.turretAttackUpgradeData[2].currentValue;

        attackRange = (float)GameManager.instance.dataManager.turretAttackUpgradeData[1].currentValue * 1.35f/25.0f;

        ShieldHP = (float)GameManager.instance.dataManager.turretHpUpgradeData[7].currentValue;

        ShieldHPMax = (float)GameManager.instance.dataManager.turretHpUpgradeData[7].currentValue;

        if(ShieldHP > 0)
        {
            shieldFx.Play();
        }

        /*
        if (HP_LV == 0)
            HP = turretConfig.HP.startValue;
        else
           HP = turretConfig.HP.startValue + HP_LV * turretConfig.HP.levelUpBase + (float)Common.SumSequence(HP_LV - 1) * turretConfig.HP.levelUpMulti;

        if (ATK_LV == 0)
            attack = turretConfig.ATK.startValue;
        else
            attack = turretConfig.ATK.startValue + ATK_LV * turretConfig.ATK.levelUpBase + (float)Common.SumSequence(ATK_LV - 1) * turretConfig.ATK.levelUpMulti;
        */
    }

    public void UpdateHP()
    {
        HPMax = Mathf.RoundToInt((float)GameManager.instance.dataManager.turretHpUpgradeData[0].currentValue);
        GameManager.instance.uiManager.gameView.UpdateTurretHPSlider(HP, HPMax);
    }
    public void UpdateAttackRange()
    {
        attackRange = (float)GameManager.instance.dataManager.turretAttackUpgradeData[1].currentValue * 1.35f / 25.0f;
        attackRangeSpr.transform.localScale = new Vector3(0.5f,0.5f,0.5f) * (float)GameManager.instance.dataManager.turretAttackUpgradeData[1].currentValue / 25.0f;
    }

    public void UpdateShieldHP()
    {
        ShieldHP = (float)GameManager.instance.dataManager.turretHpUpgradeData[7].currentValue;
        ShieldHPMax = (float)GameManager.instance.dataManager.turretHpUpgradeData[7].currentValue;

        if (ShieldHP <= 0.0f)
            shieldFx.Stop();
        else
            if(shieldFx.isStopped)
            shieldFx.Play();
    }

    public void UpdateRegenHP()
    {
       

        RegenHP = (float)GameManager.instance.dataManager.turretHpUpgradeData[1].currentValue;
        if (HP < HPMax)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= 1.0f)
            {
                regenTimer = 0.0f;
                HP += RegenHP;
            }
        }
        else
        {
            HP = HPMax;
        }

        
        //UpdateAttack();
        
    }

    public void UpdateRegenShield()
    {


        RegenShield = (float)GameManager.instance.dataManager.turretHpUpgradeData[8].currentValue;

        if (ShieldHP < ShieldHPMax)
        {
            regenShieldTimer += Time.deltaTime;

            if (regenShieldTimer >= 1.0f)
            {
                regenShieldTimer = 0.0f;
                ShieldHP += 0.1f * RegenShield;
            }
        }
        else
        {
            ShieldHP = ShieldHPMax;
        }


        //UpdateAttack();

    }

    public void UpdateAttack()
    {
        attack = (float)GameManager.instance.dataManager.turretAttackUpgradeData[0].currentValue;
        shootingRate = 1.0f / (float)GameManager.instance.dataManager.turretAttackUpgradeData[2].currentValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == STATE.DEFEAT)
            return;

        if (GameManager.instance.currentState != GameManager.GAME_STATE.PLAYING)
            return;

        UpdateRegenHP();
        UpdateRegenShield();
        UpdateHP();
        UpdateAttackRange();

        switch (currentState)
        {
            case STATE.WAIT:

                if (IsFindEnemy())
                {
                    currentState = STATE.ATTACK;
                }

                break;

            case STATE.ATTACK:

                if (currentTarget.HP <= 0)
                {
                    currentState = STATE.WAIT;
                }
                else
                {
                    if (Time.time >= nextFire)
                        Fire();
                }
                break;

            case STATE.DEFEAT:

                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(turretCenter.position, attackRange);
    }


    private bool IsFindEnemy()
    {
        bool isTrack = false;

        Collider2D[] enemyCol = Physics2D.OverlapCircleAll(turretCenter.position, attackRange, enemyMask);

        if (enemyCol != null)
        {
            
            if(enemyCol.Length > 0)
            {
                isTrack = true;
                GameObject nearestEnemy = Common.FindNearestObject(enemyCol, turretCenter);
                currentTarget = nearestEnemy.GetComponent<EnemyController>();
            }    
           
        }
        // Debug.Log("" + playerCol.gameObject.name);
        return isTrack;
    }

    void Fire()
    {
        nextFire = Time.time + shootingRate;

        //Debug.Log("MULTI SHOT " + (float)GameManager.instance.dataManager.turretAttackUpgradeData[5].currentValue);
        if (Common.IsRate((float)GameManager.instance.dataManager.turretAttackUpgradeData[5].currentValue))
        {
            MultiFire();
            return;
        }
          

        
        if (Common.IsRate((float)GameManager.instance.dataManager.turretAttackUpgradeData[7].currentValue))
            Crit();
        else
        {
            float temAtk = attack;
            if (Vector2.Distance(transform.position, currentTarget.transform.position) <= 0.6f)
                temAtk = attack * (1 + (float)GameManager.instance.dataManager.turretAttackUpgradeData[12].currentValue);
            //Debug.Log("RANAGE DAMAGE " + (1 + (float)GameManager.instance.dataManager.turretAttackUpgradeData[12].currentValue));
            Vector2 shootDir = Vector2.zero;
            shootDir = (currentTarget.transform.position - firePoint.position).normalized;

            //var bullet = Instantiate(turretProjectile, firePoint.position, Quaternion.identity);
            GameObject bulletObj = SimplePool.Spawn(turretProjectile.gameObject, firePoint.position, Quaternion.identity);
            TurretProjectile bullet = bulletObj.GetComponent<TurretProjectile>();
            bullet.transform.right = shootDir;
            bullet.Initial(shootDir, 0.1f, temAtk, false);

            /// Debug.Log("DOUBLE RATE " + (float)GameManager.instance.dataManager.turretAttackUpgradeData[3].currentValue);
            if (Common.IsRate((float)GameManager.instance.dataManager.turretAttackUpgradeData[3].currentValue))
                SecondFire();

        }

        

        AudioManager.instance.turretShotSfx.Play();
    }

    void MultiFire()
    {
        //Debug.Log("MULTI NUMBER TARTGET " + (float)GameManager.instance.dataManager.turretAttackUpgradeData[6].currentValue);
        int numberTarget = Mathf.RoundToInt((float)GameManager.instance.dataManager.turretAttackUpgradeData[6].currentValue);

        Collider2D[] enemyColList = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyMask);

        for(int i = 0; i < enemyColList.Length;i++)
        {
            if(i < numberTarget)
            {
                float temAtk = attack;
                if (Vector2.Distance(transform.position, enemyColList[i].transform.position) <= 0.6f)
                    temAtk = attack * (1 + (float)GameManager.instance.dataManager.turretAttackUpgradeData[12].currentValue);

                Vector2 shootDir = Vector2.zero;
                shootDir = (enemyColList[i].transform.position - firePoint.position).normalized;

                //var bullet = (TurretProjectile)Instantiate(turretProjectile, firePoint.position, Quaternion.identity);
                GameObject bulletObj = SimplePool.Spawn(turretProjectile.gameObject, firePoint.position, Quaternion.identity);
                TurretProjectile bullet = bulletObj.GetComponent<TurretProjectile>();
                bullet.transform.right = shootDir;
                bullet.Initial(shootDir, 0.1f, temAtk, false);
            }
           
        }
    }

    void Crit()
    {
        Vector2 shootDir = Vector2.zero;
        shootDir = (currentTarget.transform.position - firePoint.position).normalized;

        //var bullet = (TurretProjectile)Instantiate(turretProjectileCrit, firePoint.position, Quaternion.identity);
        GameObject bulletObj = SimplePool.Spawn(turretProjectile.gameObject, firePoint.position, Quaternion.identity);
        TurretProjectile bullet = bulletObj.GetComponent<TurretProjectile>();
        bullet.transform.right = shootDir;
        bullet.Initial(shootDir, 0.15f, attack * (1.0f + (float)GameManager.instance.dataManager.turretAttackUpgradeData[8].currentValue), true);

        //Debug.Log("CRIT PER" + (float)GameManager.instance.dataManager.turretAttackUpgradeData[8].currentValue);

    }

    void SecondFire()
    {
        StartCoroutine(SecondFireIE());
    }

    IEnumerator SecondFireIE()
    {
        yield return new WaitForSeconds(0.1f);

        if(currentTarget != null)
        {
            Vector2 shootDir = Vector2.zero;
            shootDir = (currentTarget.transform.position - firePoint.position).normalized;

            //var bullet = (TurretProjectile)Instantiate(turretProjectile, firePoint.position, Quaternion.identity);
            GameObject bulletObj = SimplePool.Spawn(turretProjectile.gameObject, firePoint.position, Quaternion.identity);
            TurretProjectile bullet = bulletObj.GetComponent<TurretProjectile>();
            bullet.transform.right = shootDir;
            bullet.Initial(shootDir, 0.1f, attack * (float)GameManager.instance.dataManager.turretAttackUpgradeData[4].currentValue, false);
        }

        

        // Debug.Log("DOUBLE PER " + (float)GameManager.instance.dataManager.turretAttackUpgradeData[4].currentValue);
    }

    public void Damage(float damage)
    {
        if (GameManager.instance.currentState == GameManager.GAME_STATE.GAME_OVER)
            return;
        if (GameManager.instance.currentState == GameManager.GAME_STATE.WIN)
            return;

        if (ShieldHP - damage >= 0)
        {
            ShieldHP -= damage;

            if (ShieldHP <= 0.0f)
                shieldFx.Stop();

            return;
        }

        if (HP - damage > 0)
        {
            HP -= damage;
            
        }
        else
        {
            HP = 0.0f;
            Die();
        }

        if(HP <= 0.3f * HPMax)
        {
            int randomFx = Random.Range(0, 3);
            if (randomFx == 1)
                dustFx.Play();
        }

        UpdateHP();
    }

    public void FillHP(float moreHP)
    {
        if (HP + moreHP < HPMax)
        {
            HP += moreHP;
        }

        else
            HP = HPMax;
    }

    public void Die()
    {
        currentState = STATE.DEFEAT;
        GameManager.instance.currentState = GameManager.GAME_STATE.GAME_OVER;

        if(GameManager.instance.remainRetrive > 0)
           StartCoroutine(ShowRetrie());
        else
            StartCoroutine(ShowGameOver());
    }

    IEnumerator ShowRetrie()
    {
        yield return new WaitForSeconds(1.0f);
        AudioManager.instance.gameOverSfx.Play();
        GameManager.instance.uiManager.retriveView.InitView();
        GameManager.instance.uiManager.retriveView.ShowView();
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1.0f);
        AudioManager.instance.gameOverSfx.Play();
        GameManager.instance.uiManager.resultView.ShowView();
    }

    public void RemoveAllEnemiesInRange()
    {
        Collider2D[] enemyCol = Physics2D.OverlapCircleAll(transform.position, attackRange * 3.0f, enemyMask);


        for(int i = 0; i < enemyCol.Length; i++)
        {
            enemyCol[i].GetComponent<EnemyController>().Die();
        }
       
    }
}
