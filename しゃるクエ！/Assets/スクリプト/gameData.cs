using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameData : MonoBehaviour
{
    //  それぞれのキャラクターのレベル
    public int GetSaveLV(int id)
    {
        return PlayerPrefs.GetInt("SaveLV" + id, 1);
    }

    public void SetSaveLV(int id, int number)
    {
        PlayerPrefs.SetInt("SaveLV" + id, number);
    }


    //  それぞれのキャラクターの経験値
    public int GetSaveEXP(int id)
    {
        return PlayerPrefs.GetInt("SaveEXP" + id, 1);
    }

    public void SetSaveEXP(int id, int number)
    {
        PlayerPrefs.SetInt("SaveEXP" + id, number);
    }


    //  そのキャラクターが加入しているかどうか
    public int GetSaveChara(int id)
    {
        return PlayerPrefs.GetInt("SaveChara" + id, -1);
    }

    public void SetSaveChara(int id, int number)
    {
        PlayerPrefs.SetInt("SaveChara" + id, number);
    }


    //  そのステージをクリアしているかどうか
    public int GetSaveStage(int id)
    {
        return PlayerPrefs.GetInt("SaveStage" + id,-1);
    }

    public void SetSaveStage(int id, int number)
    {
        PlayerPrefs.SetInt("SaveStage" + id, number);
    }



    public void DataSave()
    {

    }

    public void DataLoad()
    {

    }
}
