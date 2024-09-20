using System.Collections;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    public int enemyID;

    public float HP;

    public enum STATE
    {
        IDLE,
        ATTACK,
        MOVE,
        DIE
    }

    public STATE currentState;

    public enum TYPE
    {
        MELEE,
        RANGE
    }

    public bool isBoss;

    public TYPE currentType;

    public float moveSpeed;

    public float attackSpeed;

    public float attack;

    public float attackRange;

    public float nextFire;

    public EnemyProjectile enemyProjectile;

    private Animator mAnimator;

    private Vector2 moveDir;

    public Transform firePos;

    public GameObject bloodFx;

    public GameObject giftItem;

    public FloatingText giftFLoatingTxt;

    private float originalScaleX;

    private int baseKillCoin;

    private int killGold;

    private float killGoldRate;

    private float leakHPTimer;

    private BoxCollider2D col;

    private EnemyConfig enemyConfig;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitEnemy()
    {
        LoadEnemyConfig();

        mAnimator = transform.GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        col.enabled = true;
        currentState = STATE.MOVE;
        mAnimator.SetBool("Walk", true);
        // mAnimator.SetBool("Die", false);
        mAnimator.SetBool("Attack", false);
        moveDir = (GameManager.instance.turretController.transform.position - transform.position).normalized;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), 0.0f);
        originalScaleX = transform.localScale.x;
        FaceToTurret();
    }

    void LoadEnemyConfig()
    {
        enemyConfig = GameManager.instance.configManager.enemyConfig;

        HP = enemyConfig.enemyList[enemyID].hp * (1 + GameManager.instance.enemyGenerator.currentWave * GameManager.instance.configManager.enemyCommonConfig.waveUpHP);
        moveSpeed = enemyConfig.enemyList[enemyID].moveSpeed;


        if (GameManager.instance.turretUnlockAtkLevel >= 2)
        {
            if (Common.IsRate((float)GameManager.instance.dataManager.turretAttackUpgradeData[15].currentValue))
            {
                moveSpeed = moveSpeed * (1 - (float)GameManager.instance.dataManager.turretAttackUpgradeData[16].currentValue);
            }

        }




        nextFire = 1.0f / enemyConfig.enemyList[enemyID].atkSpeed;
        attack = enemyConfig.enemyList[enemyID].atk * (1 + GameManager.instance.enemyGenerator.currentWave * GameManager.instance.configManager.enemyCommonConfig.waveUpAtk);

        if (currentType == TYPE.MELEE)
            attackRange = GameManager.instance.enemyAttackRange;
        else if (currentType == TYPE.RANGE)
            attackRange = 1.35f * enemyConfig.enemyList[enemyID].atkRange / 25.0f;

        baseKillCoin = enemyConfig.enemyList[enemyID].baseKillCoin + (int)(GameManager.instance.enemyGenerator.currentWave / GameManager.instance.configManager.enemyCommonConfig.waveCountForUpCoin) * GameManager.instance.configManager.enemyCommonConfig.waveUpCoin;
        killGold = enemyConfig.enemyList[enemyID].killGold;
        killGoldRate = enemyConfig.enemyList[enemyID].killGoldRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState == GameManager.GAME_STATE.HOME_MENU)
            return;

        if (GameManager.instance.currentState == GameManager.GAME_STATE.WIN)
        {
            Idle();
            return;
        }


        if (GameManager.instance.currentState == GameManager.GAME_STATE.GAME_OVER)
        {

            Idle();
        }


        switch (currentState)
        {

            case STATE.IDLE:

                if (GameManager.instance.currentState == GameManager.GAME_STATE.PLAYING)
                {

                    Resume();
                }

                break;

            case STATE.ATTACK:

                if (Time.time >= nextFire)
                {
                    StartToAttack();
                }

                break;

            case STATE.MOVE:

                //transform.position = Vector2.Lerp(transform.position, GameManager.instance.turretController.transform.position , Time.deltaTime * moveSpeed);

                transform.Translate(moveDir * moveSpeed * Time.deltaTime * 0.05f, Space.World);

                if (Vector3.Distance(transform.position, GameManager.instance.turretController.transform.position) <= attackRange)
                {
                    currentState = STATE.ATTACK;

                }

                break;

            case STATE.DIE:

                //to do code

                break;
        }
        //if(Vector2.Distance(transform.position, GameManager.instance.turretController.transform.position) <= 2.0f)
        // HPLeak();

    }


    void HPLeak()
    {
        //leakHPTimer += Time.deltaTime;
        //if (leakHPTimer >= 1.0f)
        // {
        //     leakHPTimer = 0.0f;
        //     Damage((float)GameManager.instance.dataManager.turretHpUpgradeData[6].currentValue * HP);
        GameManager.instance.turretController.FillHP((float)GameManager.instance.dataManager.turretHpUpgradeData[6].currentValue * enemyConfig.enemyList[enemyID].hp);
        // }
    }

    void StartToAttack()
    {
        nextFire = Time.time + attackSpeed;
        StopCoroutine(StopAttackIE());
        mAnimator.SetBool("Attack", true);
        StartCoroutine(StopAttackIE());
    }

    void Idle()
    {
        currentState = STATE.IDLE;
        moveSpeed = 0.0f;
        mAnimator.SetBool("Attack", false);
        mAnimator.SetBool("Walk", false);
    }

    void Resume()
    {
        currentState = STATE.MOVE;
        moveSpeed = enemyConfig.enemyList[enemyID].moveSpeed;

        if (GameManager.instance.turretUnlockAtkLevel >= 2)
        {
            if (Common.IsRate((float)GameManager.instance.dataManager.turretAttackUpgradeData[15].currentValue))
                moveSpeed = moveSpeed * (1 - (float)GameManager.instance.dataManager.turretAttackUpgradeData[16].currentValue);

        }

        mAnimator.SetBool("Attack", false);
        mAnimator.SetBool("Walk", true);
    }

    void Attack()
    {

        if (Common.IsRate((float)GameManager.instance.dataManager.turretHpUpgradeData[3].currentValue))
            attack = 0;
        else
            attack = attack - (float)GameManager.instance.dataManager.turretHpUpgradeData[2].currentValue * attack;
        if (attack == 0)
            Debug.Log("Ne don " + (float)GameManager.instance.dataManager.turretHpUpgradeData[3].currentValue);
        //Debug.Log("DAMAGE RES " + (float)GameManager.instance.dataManager.turretHpUpgradeData[2].currentValue);

        float reflectAtk = 0.0f;

        if (Common.IsRate((float)GameManager.instance.dataManager.turretHpUpgradeData[4].currentValue))
            reflectAtk = attack * (float)GameManager.instance.dataManager.turretHpUpgradeData[5].currentValue;

        if (reflectAtk > 0)
        {
            if ((attack - reflectAtk) > 0)
            {
                attack = attack - attack * (float)GameManager.instance.dataManager.turretHpUpgradeData[5].currentValue;
                Damage(reflectAtk);
                //Debug.Log("Phan don " + (float)GameManager.instance.dataManager.turretHpUpgradeData[5].currentValue);
            }
            else
            {
                attack = 0.0f;
            }
        }

        if (currentType == TYPE.MELEE)
            GameManager.instance.turretController.Damage(attack);

        else if (currentType == TYPE.RANGE)
        {

            Vector2 shootDir = Vector2.zero;
            shootDir = (GameManager.instance.turretController.transform.position - firePos.position).normalized;
            EnemyProjectile projectile = (EnemyProjectile)Instantiate(enemyProjectile, firePos.position, Quaternion.identity);
            projectile.transform.right = shootDir;
            projectile.Initial(shootDir, 0.035f, attack);
        }

        if (AudioManager.instance.isVibration)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);

        int randomAtkSfx = Random.Range(0, 10);
        if (randomAtkSfx == 5)
            AudioManager.instance.swordAtkSfx.Play();

    }

    IEnumerator StopAttackIE()
    {
        yield return new WaitForSeconds(0.15f);
        Attack();
        yield return new WaitForSeconds(0.2f);
        mAnimator.SetBool("Attack", false);
        mAnimator.SetBool("Walk", false);
    }

    public void FaceToTurret()
    {
        if (GameManager.instance.turretController.transform.position.x >= transform.position.x)
        {
            transform.localScale = new Vector3(originalScaleX, originalScaleX, 0);
        }
        else
        {
            transform.localScale = new Vector3(-originalScaleX, originalScaleX, 0);
        }
    }

    public void Damage(float damage)
    {
        if (currentState == STATE.DIE)
            return;

        if (HP - damage > 0.0f)
        {
            HP -= damage;

        }
        else
        {
            HP = 0.0f;
            Die();
        }
    }

    public void Die()
    {
        currentState = STATE.DIE;
        col.enabled = false;


        int randomDieSound = Random.Range(0, 10);

        if (randomDieSound == 5)
            AudioManager.instance.enemyDeathSfx.Play();


        GameManager.instance.currentKilledEnemy++;
        if (isBoss)
            GameManager.instance.currentKilledBoss++;

        //Instantiate(bloodFx, transform.position, Quaternion.identity);
        SimplePool.Spawn(bloodFx, transform.position, Quaternion.identity);
        //mAnimator.SetTrigger("Die");


        bool isGenGold = Common.IsRate(killGoldRate);
        if (isGenGold)
            GameManager.instance.GenGift(baseKillCoin, killGold, transform.position);
        else
            GameManager.instance.GenGift(baseKillCoin, 0, transform.position);
        HPLeak();

        //SimplePool.Despawn(gameObject);
        Destroy(gameObject);
        //StartCoroutine(RemoveObj());
    }

    IEnumerator RemoveObj()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
        //SimplePool.Despawn(gameObject);
    }

}
