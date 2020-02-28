using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class StatusControl : MonoBehaviour
{
    struct statusData
    {
        public string CharName;    //  キャラクターの名前
        public int LV;             //  レベル
        public int HP;             //  体力
        public int SP;             //  技の発動に必要
        public int ATK;            //  物理攻撃力
        public int DEF;            //  物理防御力
        public int SPD;            //  素早さ
        public int MAT;            //  魔法攻撃力
        public int MDF;            //  魔法防御力
        public int LUK;            //  幸運値
        public int EXP;            //  経験値
    }

    private player_charaList PC;
    private playerGrow PG;
    private EXP_List EL;

    private gameData gData;
    

    //  ステータス格納用
    statusData[] StatusList = new statusData[6];

    //  レベルアップ時の上昇ステータス格納用リスト
    statusData[] StatusGrowList = new statusData[6];

    //  プレイヤー格納用リスト
    private List<int> playerList = new List<int>();
    //  敵格納用リスト
    private List<int> enemyList = new List<int>();

    //  ステージのID
    private int stageID;
    //  BGMのID
    private int BGMID;
    //  そのステージでもらえる経験値
    private int Exp;

    //  仲間になるキャラクターのID(0なら無し)
    private int addCharID;

    void Start()
    {
        gData = new gameData();

        StatusInit();

        addCharID = 0;

        //  セーブデータが存在する場合
        if (PlayerPrefs.HasKey("SaveChara1"))
        {
            Debug.Log("つづきから");
            for (int charID = 0; charID < 6; charID++)
            {
                StatusList[charID].LV = this.GetComponent<gameData>().GetSaveLV(charID);
                StatusList[charID].EXP = this.GetComponent<gameData>().GetSaveEXP(charID);

                if(this.GetComponent<gameData>().GetSaveChara(charID) == 1)
                {
                    playerList.Add(charID + 1);
                }
            }
        }
        //  セーブデータが存在しない場合
        else
        {
            Debug.Log("はじめから");

            for (int charID = 1; charID < 7; charID++)
            {
                if(charID <= 3)
                {
                    playerList.Add(charID);
                    this.GetComponent<gameData>().SetSaveChara(charID - 1, 1);
                }
                else
                {
                    this.GetComponent<gameData>().SetSaveChara(charID - 1, 0);
                }
            }

            SaveChara();
        }
        stageID = 1;
        BGMID = 1;
    }

    void StatusInit()
    {
        PC = Resources.Load("ExcelData/player_chara") as player_charaList;
        PG = Resources.Load("ExcelData/playerGrow") as playerGrow;

        //  レベル1時の初期ステータスを取得
        for (int charID = 0; charID < 6; charID++)
        {
            StatusList[charID] = new statusData()
            {
                CharName = PC.sheets[0].list[charID].Name,
                LV = 3,
                HP = (int)PC.sheets[0].list[charID].HP,
                SP = (int)PC.sheets[0].list[charID].SP,
                ATK = (int)PC.sheets[0].list[charID].ATK,
                DEF = (int)PC.sheets[0].list[charID].DEF,
                SPD = (int)PC.sheets[0].list[charID].SPD,
                MAT = (int)PC.sheets[0].list[charID].MAT,
                MDF = (int)PC.sheets[0].list[charID].MDF,
                LUK = (int)PC.sheets[0].list[charID].LUK,
                EXP = 220
            };

            StatusGrowList[charID] = new statusData()
            {
                LV = 1,
                HP = (int)PG.sheets[0].list[charID].GROWHP,
                SP = (int)PG.sheets[0].list[charID].GROWSP,
                ATK = (int)PG.sheets[0].list[charID].GROWATK,
                DEF = (int)PG.sheets[0].list[charID].GROWDEF,
                SPD = (int)PG.sheets[0].list[charID].GROWSPD,
                MAT = (int)PG.sheets[0].list[charID].GROWMAT,
                MDF = (int)PG.sheets[0].list[charID].GROWMDF,
                LUK = (int)PG.sheets[0].list[charID].GROWLUK,
                EXP = 0
            };
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlaySE("SE03");
        }
    }

    //  外部にステータスを渡す時用
    public void SetStatus(int id,ref string charName,ref int lv, ref int hp, ref int sp, ref int atk, ref int def, ref int spd, ref int mat,ref int mdf, ref int luk)
    {
        charName = StatusList[id].CharName;
        lv = StatusList[id].LV;
        ////  レベルによってステータスを加算する
        hp = StatusList[id].HP + StatusGrowList[id].HP * (lv - 1);
        sp = StatusList[id].SP + StatusGrowList[id].SP * (lv - 1);
        atk = StatusList[id].ATK + StatusGrowList[id].ATK * (lv - 1);
        def = StatusList[id].DEF + StatusGrowList[id].DEF * (lv - 1);
        spd = StatusList[id].SPD + StatusGrowList[id].SPD * (lv - 1);
        mat = StatusList[id].MAT + StatusGrowList[id].MAT * (lv - 1);
        mdf = StatusList[id].MDF + StatusGrowList[id].MDF * (lv - 1);
        luk = StatusList[id].LUK + StatusGrowList[id].LUK * (lv - 1);
    }

    public void GetNowCharExp(int id,int exp)
    {
        exp = StatusList[id - 1].EXP;
    }

    //  経験値の獲得&レベルアップ処理
    public bool AcquisitionExp(int id,int exp)
    {
        bool LevelUpFlag = false;
        if (StatusList[id - 1].LV < 99)
        {
            StatusList[id - 1].EXP += exp;

            EL = Resources.Load("ExcelData/EXP_List") as EXP_List;
            //  レベルアップするために必要な総経験値
            int levelUpExp = EL.sheets[0].list[StatusList[id - 1].LV - 1].TotalExp;

            while (StatusList[id - 1].EXP >= levelUpExp)
            {
                StatusList[id - 1].LV++;
                levelUpExp = EL.sheets[0].list[StatusList[id - 1].LV - 1].TotalExp;

                //  レベルアップしたらtrueを返してレベルアップしたことを伝える
                LevelUpFlag = true;
            }
        }
        return LevelUpFlag;
    }

    public void SetPlayerList(int char1,int char2,int char3)
    {
        playerList.Add(char1);
        playerList.Add(char2);
        playerList.Add(char3);
    }

    public void SetEnemyList(int char1,int char2,int char3)
    {
        enemyList.Clear();
        enemyList.Add(char1);
        enemyList.Add(char2);
        enemyList.Add(char3);
    }

    //  戦闘用
    public void GetPlayerList(ref int char1,ref int char2,ref int char3)
    {
        char1 = playerList[0];
        char2 = playerList[1];
        char3 = playerList[2];
    }

    //  編成用
    public void GetPlayerList(ref int char1, ref int char2, ref int char3, ref int char4, ref int char5, ref int char6)
    {
        char1 = playerList[0];
        char2 = playerList[1];
        char3 = playerList[2];

        //  仲間がいるか確認する
        if (playerList.Count >= 4)
        {
            char4 = playerList[3];
        }
        else
        {
            char4 = 0;
        }

        if (playerList.Count >= 5)
        {
            char5 = playerList[4];
        }
        else
        {
            char5 = 0;
        }

        if (playerList.Count >= 6)
        {
            char6 = playerList[5];
        }
        else
        {
            char6 = 0;
        }
    }

    //  戦闘のキャラの取得(ホームやワールドマップ用)
    public int Get1Player()
    {
        return playerList[0];
    }

    //  編成用
    public int GetLV(int id)
    {
        return StatusList[id - 1].LV;
    }

    public void GetEnemyList(ref int char1, ref int char2, ref int char3)
    {
        char1 = enemyList[0];
        char2 = enemyList[1];
        char3 = enemyList[2];
    }

    //  編成画面用(numberは編成の何番目か)
    public void ChangePlayerList(int number1,int number2)
    {
        Debug.Log(number1);
        Debug.Log(number2);

        int id1 = playerList[number1 - 1];
        int id2 = playerList[number2 - 1];

        playerList.Remove(id1);
        playerList.Remove(id2);

        if (number1 > number2)
        {
            playerList.Insert(number2 - 1, id1);
            playerList.Insert(number1 - 1, id2);
        }
        else
        {
            playerList.Insert(number1 - 1, id2);
            playerList.Insert(number2 - 1, id1);
        }
    }

    public void SetStageID(int id)
    {
        stageID = id;
    }

    public int GetStageID()
    {
        return stageID;
    }

    public void SetBGMID(int id)
    {
        BGMID = id;
    }

    public int GetBGMID()
    {
        return BGMID;
    }

    public void SetEXP(int number)
    {
        Exp = number;
    }

    public int GetEXP()
    {
        return Exp;
    }

    //  レベルアップまでに必要な経験値
    public int GetNextLevelExp(int charID)
    {
        EL = Resources.Load("ExcelData/EXP_List") as EXP_List;
        return (EL.sheets[0].list[StatusList[charID - 1].LV - 1].TotalExp) - (StatusList[charID - 1].EXP);
    }

    //  任意のキャラクターが仲間になっているか検索する
    public bool FindChar(int _charID)
    {
        if(playerList.IndexOf(_charID) == -1)
        {
            return false;
        }
        return true;
    }

    //  任意のキャラクターを仲間にする(既に仲間になっていたらfalseを返す)
    public bool AddChar()
    {
        if (addCharID != 0)
        {
            if (!FindChar(addCharID))
            {
                playerList.Add(addCharID);
                this.GetComponent<gameData>().SetSaveChara(addCharID,1);
                PlayerPrefs.Save();
                return true;
            }
        }

        //  初期化
        addCharID = 0;
        return false;
    }

    public void SetAddCharID(int _charID)
    {
        addCharID = _charID;
    }

    //  セーブ用
    public void SaveChara()
    {
        for (int charID = 0; charID < 6; charID++)
        {
            this.GetComponent<gameData>().SetSaveLV(charID, StatusList[charID].LV);
            this.GetComponent<gameData>().SetSaveEXP(charID, StatusList[charID].EXP);
        }
        this.GetComponent<gameData>().SetSaveStage(1,1);
        PlayerPrefs.Save();
    }
}
