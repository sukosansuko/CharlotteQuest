using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossMass : MonoBehaviour
{
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
    void Update()
    {
        int WorldID = 1;

        SD = Resources.Load("ExcelData/stageData") as stageData;

        if (activScene == "W1")
        {
            if (stageMass.MassName == "boss")
            {
                Create(2, WorldID);
            }
        }
        WorldID++;
        if (activScene == "W2")
        {
            if (stageMass.MassName == "boss")
            {
                Create(6, WorldID);
            }
        }
        WorldID++;
        if (activScene == "W3")
        {
            if (stageMass.MassName == "boss")
            {
                Create(11, WorldID);
            }
        }
        WorldID++;
        if (activScene == "W4")
        {
            if (stageMass.MassName == "boss")
            {
                Create(17, WorldID);
            }
        }
    }

    private void Create(int count, int stageID)
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
            enemy1 = SD.sheets[0].list[count].enemy1;
            enemy2 = SD.sheets[0].list[count].enemy2;
            enemy3 = SD.sheets[0].list[count].enemy3;
        }
        sceneNavigator.GetComponent<StatusControl>().SetEnemyList(enemy1, enemy2, enemy3);
        sceneNavigator.GetComponent<StatusControl>().SetBGMID(SD.sheets[0].list[count].BGM);
        sceneNavigator.GetComponent<StatusControl>().SetEXP((int)SD.sheets[0].list[count].EXP);
        sceneNavigator.GetComponent<StatusControl>().SetStageID(stageID);
        SceneNavigator.Instance.Change("BattleScene");
    }
}
