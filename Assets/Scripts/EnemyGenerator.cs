using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public List<Transform> enemyGenPosList;

    public EnemyInWorld[] levelData;

    public int currentTurnInWave;

    public int currentWave;

    public int currentWorld;

    private int enemyGenInWave, enemyInWave;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitLevel()
    {
        currentWorld = GameManager.instance.currentWorld;

        GameManager.instance.currentCoin = levelData[currentWorld].startCoin;

        currentWave = 0;
        currentTurnInWave = 0;

        StopAllCoroutines();

        UpdateWave();

        GameManager.instance.uiManager.gameView.RefreshWaveText(currentWave + 1);
        GameManager.instance.uiManager.gameView.RefreshWaveProgress((float)enemyGenInWave / (float)enemyInWave);

        GetEnemyGenPos();

        StartCoroutine(GenerateEnemyInTurn(levelData[currentWorld].listEnemyWave[currentWave].listEnemyTurn[currentTurnInWave]));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetEnemyGenPos()
    {
        enemyGenPosList = new List<Transform>();

        Transform genPosParentRoot = transform.GetChild(0);

        for(int i = 0; i < genPosParentRoot.childCount; i++)
        {
            enemyGenPosList.Add(genPosParentRoot.GetChild(i));
        }
    }

    IEnumerator GenerateEnemyInTurn(Turn currentTurn)
    {

       // if (GameManager.instance.currentState == GameManager.GAME_STATE.GAME_OVER)
            yield return new WaitUntil(() => {
                return GameManager.instance.currentState == GameManager.GAME_STATE.PLAYING;
            });

        if (currentWave == 0)
            yield return new WaitForSeconds(levelData[currentWorld].worldStartDelay);

        yield return new WaitForSeconds(currentTurn.turnStartDelay);

        AudioManager.instance.enemyAppearSfx.Play();

        for (int i = 0; i < currentTurn.listUnit.Count; i++)
        {
            if (GameManager.instance.currentState == GameManager.GAME_STATE.GAME_OVER)
                break;

            int randomPosIndex = Random.Range(0, enemyGenPosList.Count);

            float randomDelaySpawn = Random.Range(currentTurn.minInterval, currentTurn.maxInterval);

            yield return new WaitForSeconds(randomDelaySpawn);

            if(i == 0)
                GameManager.instance.uiManager.gameView.RefreshWaveText(currentWave + 1);

            ///GameObject enemyControllerObj = SimplePool.Spawn(currentTurn.listUnit[i].gameObject, enemyGenPosList[randomPosIndex].position, Quaternion.identity);
            GameObject enemyControllerObj = Instantiate(currentTurn.listUnit[i].gameObject, enemyGenPosList[randomPosIndex].position, Quaternion.identity);
            EnemyController enemyController = enemyControllerObj.GetComponent<EnemyController>();
            enemyController.InitEnemy();
            GameManager.instance.enemyList.Add(enemyControllerObj);

            enemyGenInWave++;
            GameManager.instance.uiManager.gameView.RefreshWaveProgress((float)enemyGenInWave/(float)enemyInWave);
        }

        yield return new WaitUntil(() => {
            return GameManager.instance.currentState == GameManager.GAME_STATE.PLAYING;
        });

        if (currentTurnInWave < levelData[currentWorld].listEnemyWave[currentWave].listEnemyTurn.Count - 1)
        {
            currentTurnInWave++;
            StartCoroutine(GenerateEnemyInTurn(levelData[currentWorld].listEnemyWave[currentWave].listEnemyTurn[currentTurnInWave]));
        }
        else
        {
            //Debug.Log("End Wave");
            if(currentWave < levelData[currentWorld].listEnemyWave.Count - 1)
            {
                currentWave++;
                currentTurnInWave = 0;
                UpdateWave();

                StartCoroutine(GenerateEnemyInTurn(levelData[currentWorld].listEnemyWave[currentWave].listEnemyTurn[currentTurnInWave]));
            }
            else
            {
                currentWave = levelData[currentWorld].listEnemyWave.Count;
                GameManager.instance.currentState = GameManager.GAME_STATE.WIN;
                StartCoroutine(ShowGameOver());
            }
        }
    }

    public void UpdateWave()
    {
        enemyGenInWave = 0;

        enemyInWave = 0;

        for (int i = 0; i < levelData[currentWorld].listEnemyWave[currentWave].listEnemyTurn.Count; i++)
        {

            enemyInWave += levelData[currentWorld].listEnemyWave[currentWave].listEnemyTurn[i].listUnit.Count;

        }

       // GameManager.instance.uiManager.gameView.RefreshWaveText(currentWave + 1);
       // GameManager.instance.uiManager.gameView.RefreshWaveProgress((float)enemyGenInWave / (float)enemyInWave);
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1.0f);
        AudioManager.instance.gameOverSfx.Play();
        GameManager.instance.uiManager.resultView.ShowView();
    }
}
