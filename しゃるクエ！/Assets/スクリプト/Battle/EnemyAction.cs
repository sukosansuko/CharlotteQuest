using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyAction : MonoBehaviour
{
    //  敵用画像読み込み
    [SerializeField] private Sprite esp1;
    [SerializeField] private Sprite esp2;
    [SerializeField] private Sprite esp3;
    [SerializeField] private Sprite esp4;
    [SerializeField] private Sprite esp5;
    [SerializeField] private Sprite esp6;
    [SerializeField] private Sprite esp7;
    [SerializeField] private Sprite esp8;
    [SerializeField] private Sprite esp9;
    [SerializeField] private Sprite esp10;
    [SerializeField] private Sprite esp11;
    [SerializeField] private Sprite esp12;
    [SerializeField] private Sprite esp13;
    [SerializeField] private Sprite esp14;
    [SerializeField] private Sprite esp15;
    [SerializeField] private Sprite esp16;
    [SerializeField] private Sprite esp17;
    [SerializeField] private Sprite esp18;
    [SerializeField] private Sprite esp19;
    [SerializeField] private Sprite esp20;
    [SerializeField] private Sprite esp21;
    [SerializeField] private Sprite esp22;

    private int charID;
    
    enemy_charaList EC;
    enemy_skillList ES;

    private int skillID;
    private int skillCount;

    private GameObject battleManager;

    void Start()
    {
        battleManager = GameObject.Find("BattleManager");
    }

    void Update()
    {
        
    }

    public void SetCharID(int id)
    {
        charID = id;
    }

    public void CommandSelect(GameObject charaObj)
    {
        System.Random rnd = new System.Random();
        skillCount = 0;

        EC = Resources.Load("ExcelData/enemy_chara") as enemy_charaList;
        ES = Resources.Load("ExcelData/enemySkill") as enemy_skillList;

        //  スキルをいくつ持っているかカウントする
        if(EC.sheets[0].list[charID].Skill1 != 0)
        {
            skillCount++;
        }
        if (EC.sheets[0].list[charID].Skill2 != 0)
        {
            skillCount++;
        }
        if (EC.sheets[0].list[charID].Skill3 != 0)
        {
            skillCount++;
        }
        if (EC.sheets[0].list[charID].Skill4 != 0)
        {
            skillCount++;
        }
        //  持っているスキルの中からランダムでスキルを選ぶ
        int skillNumber = rnd.Next(1, skillCount + 1);

        switch(skillNumber)
        {
            case 1:
                skillID = EC.sheets[0].list[charID].Skill1 - 1;
                break;
            case 2:
                skillID = EC.sheets[0].list[charID].Skill2 - 1;
                break;
            case 3:
                skillID = EC.sheets[0].list[charID].Skill3 - 1;
                break;
            case 4:
                skillID = EC.sheets[0].list[charID].Skill4 - 1;
                break;
            default:
                break;
        }
        //  スキルを決定する
        charaObj.GetComponent<Status>().SaveSkill(skillID);

        EsetReceive(charaObj);
    }

     private void EsetReceive(GameObject characterID)
    {
        System.Random rnd = new System.Random();
        string SkillTarget = ES.sheets[0].list[skillID].target;

        //  ランダムで標的を決める時用
        int targetID;
        GameObject receive;

        if (SkillTarget.StartsWith("P"))
        {
            if (SkillTarget.Contains("0") || SkillTarget.Contains("1"))
            {
                targetID = rnd.Next(1, battleManager.GetComponent<command>().GetEnemyCount() + 1);
                receive = battleManager.GetComponent<command>().GetEnemyChild(targetID - 1);

                characterID.GetComponent<Status>().SaveReceive(receive);
            }
            else if (SkillTarget.Contains("2"))
            {
                //  また後で------------------------------------------------------------
            }
            else
            {
                if (GetComponent<command>().GetEnemyCount() == 3)
                {
                    characterID.GetComponent<Status>().SaveReceive(battleManager.GetComponent<command>().GetEnemyChild(0), 
                        battleManager.GetComponent<command>().GetEnemyChild(1), battleManager.GetComponent<command>().GetEnemyChild(2));
                }
                else if (GetComponent<command>().GetEnemyCount() == 2)
                {
                    characterID.GetComponent<Status>().SaveReceive(battleManager.GetComponent<command>().GetEnemyChild(0), battleManager.GetComponent<command>().GetEnemyChild(1));
                }
                else
                {
                    characterID.GetComponent<Status>().SaveReceive(battleManager.GetComponent<command>().GetEnemyChild(0));
                }
            }
        }
        else
        {
            GameObject receive1 = null;
            GameObject receive2 = null;

            List<GameObject> ReceiveList = new List<GameObject>();

            if (SkillTarget.Contains("1"))
            {
                //  生きてるプレイヤーをリストに追加
                if(battleManager.GetComponent<command>().GetPlayerChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                {
                    ReceiveList.Add(battleManager.GetComponent<command>().GetPlayerChild(0));
                }
                if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                {
                    ReceiveList.Add(battleManager.GetComponent<command>().GetPlayerChild(1));
                }
                if (battleManager.GetComponent<command>().GetPlayerChild(2).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                {
                    ReceiveList.Add(battleManager.GetComponent<command>().GetPlayerChild(2));
                }

                targetID = rnd.Next(1, ReceiveList.Count + 1);

                characterID.GetComponent<Status>().SaveReceive(ReceiveList[targetID - 1]);
            }
            else if (SkillTarget.Contains("2"))
            {
                //  存在しない------------------------------------------------------------
            }
            else
            {
                if (GetComponent<command>().GetPlayerCount() == 3)
                {
                    characterID.GetComponent<Status>().SaveReceive(battleManager.GetComponent<command>().GetPlayerChild(0),
                    battleManager.GetComponent<command>().GetPlayerChild(1), battleManager.GetComponent<command>().GetPlayerChild(2));
                }
                else
                {
                    //  相手が死んでなければ標的にするのを許可する！
                    if (battleManager.GetComponent<command>().GetPlayerChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                    {
                        receive1 = battleManager.GetComponent<command>().GetPlayerChild(0);
                    }
                    else if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                    {
                        receive1 = battleManager.GetComponent<command>().GetPlayerChild(1);
                    }
                    else if (battleManager.GetComponent<command>().GetPlayerChild(2).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                    {
                        receive1 = battleManager.GetComponent<command>().GetPlayerChild(2);
                    }
                    else
                    {
                    }

                    if (GetComponent<command>().GetPlayerCount() == 2)
                    {
                        if (battleManager.GetComponent<command>().GetPlayerChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD
                            && battleManager.GetComponent<command>().GetPlayerChild(0) != receive1)
                        {
                            receive2 = battleManager.GetComponent<command>().GetPlayerChild(0);
                        }
                        else if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD
                             && battleManager.GetComponent<command>().GetPlayerChild(1) != receive1)
                        {
                            receive2 = battleManager.GetComponent<command>().GetPlayerChild(1);
                        }
                        else if (battleManager.GetComponent<command>().GetPlayerChild(2).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD
                             && battleManager.GetComponent<command>().GetPlayerChild(2) != receive1)
                        {
                            receive2 = battleManager.GetComponent<command>().GetPlayerChild(2);
                        }
                        else
                        {
                        }

                        characterID.GetComponent<Status>().SaveReceive(receive1, receive2);
                    }
                    else
                    {
                        characterID.GetComponent<Status>().SaveReceive(receive1);
                    }
                }
            }
        }
    }

    public void EnemyCharaChange(GameObject obj, int id)
    {
        switch (id)
        {
            case 1:
                obj.GetComponent<Image>().sprite = esp1;
                break;
            case 2:
                obj.GetComponent<Image>().sprite = esp2;
                break;
            case 3:
                obj.GetComponent<Image>().sprite = esp3;
                break;
            case 4:
                obj.GetComponent<Image>().sprite = esp4;
                break;
            case 5:
                obj.GetComponent<Image>().sprite = esp5;
                break;
            case 6:
                obj.GetComponent<Image>().sprite = esp6;
                break;
            case 7:
                obj.GetComponent<Image>().sprite = esp7;
                break;
            case 8:
                obj.GetComponent<Image>().sprite = esp8;
                break;
            case 9:
                obj.GetComponent<Image>().sprite = esp9;
                break;
            case 10:
                obj.GetComponent<Image>().sprite = esp10;
                break;
            case 11:
                obj.GetComponent<Image>().sprite = esp11;
                break;
            case 12:
                obj.GetComponent<Image>().sprite = esp12;
                break;
            case 13:
                obj.GetComponent<Image>().sprite = esp13;
                break;
            case 14:
                obj.GetComponent<Image>().sprite = esp14;
                break;
            case 15:
                obj.GetComponent<Image>().sprite = esp15;
                break;
            case 16:
                obj.GetComponent<Image>().sprite = esp16;
                break;
            case 17:
                obj.GetComponent<Image>().sprite = esp17;
                break;
            case 18:
                obj.GetComponent<Image>().sprite = esp18;
                break;
            case 19:
                obj.GetComponent<Image>().sprite = esp19;
                break;
            case 20:
                obj.GetComponent<Image>().sprite = esp20;
                break;
            case 21:
                obj.GetComponent<Image>().sprite = esp21;
                break;
            case 22:
                obj.GetComponent<Image>().sprite = esp22;
                break;
            default:
                break;
        }
    }
}
