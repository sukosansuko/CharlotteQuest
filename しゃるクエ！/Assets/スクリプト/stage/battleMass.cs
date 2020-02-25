using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleMass : MonoBehaviour {

    string activScene;      // 現在のｼｰﾝ名
    private GameObject sceneNavigator;
    private stageData SD;


    // Use this for initialization
    void Start()
    {
        activScene = mapChar.activScene;
        sceneNavigator = GameObject.Find("SceneNavigator");
    }

    // Update is called once per frame
    void Update ()
    {
        //  ステージのカウント用
        int count = 0;
        //  ワールドのID
        int WorldID = 0;

        SD = Resources.Load("ExcelData/stageData") as stageData;

        if (mapChar.activScene == "W1")
        {
            WorldID = 1;
            if (stageMass.MassName == "battle1")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle2")
            {
                Create(count, WorldID);
            }
        }
        
        if (mapChar.activScene == "W2")
        {
            count = 3;
            WorldID = 2;
            if (stageMass.MassName == "battle1")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle2")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle3")
            {
                Create(count, WorldID);
            }
        }

        if (mapChar.activScene == "W3")
        {
            count = 7;
            WorldID = 3;
            if (stageMass.MassName == "battle1")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle2")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle3")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle4")
            {
                Create(count, WorldID);
            }
        }

        if (mapChar.activScene == "W4")
        {
            count = 12;
            WorldID = 4;
            if (stageMass.MassName == "battle1")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle2")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle3")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle4")
            {
                Create(count, WorldID);
            }
            count++;
            if (stageMass.MassName == "battle5")
            {
                Create(count, WorldID);
            }
        }
    }

    private void Create(int count,int stageID)
    {
        int enemy1 = 0;
        int enemy2 = 0;
        int enemy3 = 0;

        if (SD.sheets[0].list[count].enemy_count == 1)
        {
            enemy1 = 0;
            enemy2 = SD.sheets[0].list[count].enemy1;
            enemy3 = 0;
        }
        if (SD.sheets[0].list[count].enemy_count == 3)
        {
            for (int i = 0; i < 3;i++)
            {
                int randNumber = UnityEngine.Random.Range(1, SD.sheets[0].list[count].enemy_typeCount + 1);
                int num = 0;
                switch (randNumber)
                {
                    case 1:
                        num = SD.sheets[0].list[count].enemy1;
                        break;
                    case 2:
                        num = SD.sheets[0].list[count].enemy2;
                        break;
                    case 3:
                        num = SD.sheets[0].list[count].enemy3;
                        break;
                    case 4:
                        num = SD.sheets[0].list[count].enemy4;
                        break;
                    case 5:
                        num = SD.sheets[0].list[count].enemy5;
                        break;
                    case 6:
                        num = SD.sheets[0].list[count].enemy6;
                        break;
                    case 7:
                        num = SD.sheets[0].list[count].enemy7;
                        break;
                    case 8:
                        num = SD.sheets[0].list[count].enemy8;
                        break;
                    default:
                        break;
                }
                Debug.Log("ああああああああああああああああああああ" + num);
                if (i == 0)
                {
                    enemy1 = num;
                }
                else if(i == 1)
                {
                    enemy2 = num;
                }
                else
                {
                    enemy3 = num;
                }
            }

            //enemy1 = SD.sheets[0].list[count].enemy1;
            //enemy2 = SD.sheets[0].list[count].enemy2;
            //enemy3 = SD.sheets[0].list[count].enemy3;
        }
        sceneNavigator.GetComponent<StatusControl>().SetEnemyList(enemy1, enemy2, enemy3);
        sceneNavigator.GetComponent<StatusControl>().SetBGMID(SD.sheets[0].list[count].BGM);
        sceneNavigator.GetComponent<StatusControl>().SetEXP((int)SD.sheets[0].list[count].EXP);
        sceneNavigator.GetComponent<StatusControl>().SetStageID(stageID);
        Debug.Log("カウント" + count);
        SceneNavigator.Instance.Change("BattleScene");
    }

}
