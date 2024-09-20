using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;
//using static UnityEditor.Progress;

namespace JsonParse
{
    public class JsonParse : MonoBehaviour
    {
        public string fileName = "Assets/Resources/Configs/GameConfig.json";

        public EnemyInWorld mWorld;

        public List<EnemyController> enemyList;

        // Start is called before the first frame update
        void Start()
        {
            mWorld = new EnemyInWorld();
            mWorld.listEnemyWave = new List<Wave>();
            string jsonString = File.ReadAllText(fileName);
            JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonString);

            //Debug.Log(jsonNode["ConfigVer"]);
            //Debug.Log(jsonNode["GameVer"]);
            //Debug.Log(jsonNode["WorldsConfig"]["World1"]["listEnemyWave"].Count);


            JSONNode worldNode = jsonNode["WorldsConfig"]["World2"];
            mWorld.worldStartDelay = worldNode["worldStartDelay"];
            mWorld.startCoin = worldNode["startCoin"];
            mWorld.loopWave = worldNode["loopWave"];

            JSONNode listEnemyWaveNode = jsonNode["WorldsConfig"]["World1"]["listEnemyWave"];
            for (int i = 0; i < listEnemyWaveNode.Count; i++)
            {
                JSONNode waveNode = listEnemyWaveNode[i]["listEnemyTurn"];

                //Debug.Log(waveNode.Count);
                Wave wave = new Wave();
                wave.listEnemyTurn = new List<Turn>();

                for(int j = 0; j < waveNode.Count; j++)
                {
                    Turn turn = new Turn();

                    turn.listUnitRow = new List<string>();

                    turn.listUnit = new List<EnemyController>();

                    JSONNode turnNode = listEnemyWaveNode[i]["listEnemyTurn"][j];

                    JSONNode listUnitNode = listEnemyWaveNode[i]["listEnemyTurn"][j]["listUnit"];

                    for(int k = 0; k < listUnitNode.Count; k++)
                    {
                        turn.listUnitRow.Add(listUnitNode[k].Value.ToString());
                    }

                    for(int m = 0; m < turn.listUnitRow.Count; m++)
                    {
                        
                        for(int n = 0; n <  int.Parse(turn.listUnitRow[m]); n++)
                        {
                            Debug.Log(turn.listUnitRow[m]);
                            turn.listUnit.Add(enemyList[m]);
                        }
                            
                    }

                    turn.minInterval = turnNode["minInterval"];
                    turn.maxInterval = turnNode["maxInterval"];
                    turn.turnStartDelay = turnNode["turnStartDelay"];
                    wave.listEnemyTurn.Add(turn);
                }

                mWorld.listEnemyWave.Add(wave);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                CreateWorldConfig();
        }

        public void CreateWorldConfig()
        {
#if UNITY_EDITOR
            //LevelSetting levelTest = ScriptableObject.CreateInstance<LevelSetting>();
            //levelTest.bottlesInFirstRow = levelInFirstRow;
            //levelTest.bottlesInSecondRow = levelInSecondRow;
            //UnityEditor.AssetDatabase.CreateAsset(levelTest, "Assets/Data/Level" + mLevel.ToString() + ".asset");

            EnemyInWorld exportWorld = ScriptableObject.CreateInstance<EnemyInWorld>();
            exportWorld = mWorld;
            UnityEditor.AssetDatabase.CreateAsset(exportWorld, "Assets/GameConfigs/World2" +".asset");
# endif
        }
    }

    /*

    [System.Serializable]
    public class Turn
    {
        [HideInInspector]
        public List<string> listUnitRow;

        public List<EnemyController> listUnit;

        public string minInterval;

        public string maxInterval;

        public string turnStartDelay;

    }

    [System.Serializable]
    public class Wave
    {
        public List<Turn> listEnemyTurn;
    }

    [System.Serializable]
    public class World
    {
        public List<Wave> listEnemyWave;

        public string worldStartDelay;

        public string startCoin;

        public string loopWave;
    }
    */
}

