using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameData : MonoBehaviour
{
    private string savePass = "data";

    //  キャラクターのレベル
    //  キャラクターの経験値
    //  加入している仲間
    //  クリアしているステージ


    public int GetSaveLV(int id)
    {
        return PlayerPrefs.GetInt("SaveLV" + id, 1);
    }

    public void SetSaveLV(int id, int number)
    {
        PlayerPrefs.SetInt("SaveLV" + id, number);
    }

    public int GetSaveEXP(int id)
    {
        return PlayerPrefs.GetInt("SaveEXP" + id, 1);
    }

    public void SetSaveEXP(int id, int number)
    {
        PlayerPrefs.SetInt("SaveEXP" + id, number);
    }

    public int GetSaveChara()
    {
        return PlayerPrefs.GetInt("SaveChara", -1);
    }

    public void SetSaveChara(int id)
    {
        PlayerPrefs.SetInt("SaveChara", id);
    }

    public int GetSaveStageID()
    {
        return PlayerPrefs.GetInt("SaveStageID",-1);
    }

    public void SetSaveStageID(int id)
    {
        PlayerPrefs.SetInt("SaveStageID", id);
    }



    public void DataSave()
    {

    }

    public void DataLoad()
    {

    }
}
