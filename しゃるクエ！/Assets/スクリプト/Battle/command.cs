using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class command : MonoBehaviour
{
    public enum TYPE
    {
        TYPE_NON,
        TYPE_ATTACK,            //  攻撃
        TYPE_SUPPORT,           //  サポート
        TYPE_ITEM,              //  アイテム
        TYPE_MAX
    }

    //  オブジェクトの取得用
    public GameObject Attack;
    public GameObject Support;
    public GameObject Item;
    public GameObject SkillList;
    public GameObject Back;
    public GameObject Description;
    public GameObject DescriptionText;

    public Transform attackText;
    public Transform supportText;
    public Transform itemText;

    public Transform playerTarget;
    public Transform enemyTarget;

    public Transform player;
    public Transform enemy;

    public GameObject actionDescription;
    public GameObject actionDescriptionText;

    private bool commandEnd;

    private int charID;
    private int target;
    private TYPE skillType;
    private int SkillName;

    //  行動保存用
    private int SkillID;

    private string ActiveSkillText;

    player_charaList PC;
    player_skillList PS;

    private GameObject TextTarget;
    private GameObject battleManager;

    private GameObject TargetChar;

    private List<int> SaveSkillIDList = new List<int>();

    private int charLV;

    void Start()
    {
        battleManager = GameObject.Find("BattleManager");
        Init();
    }

    void Update()
    {
        
    }

    private void Init()
    {
        commandEnd = true;

        //  ボタンや画像はとりあえず非表示に
        Attack.GetComponent<Image>().enabled = false;
        Support.GetComponent<Image>().enabled = false;
        Item.GetComponent<Image>().enabled = false;

        SkillList.GetComponent<Image>().enabled = false;
        Back.GetComponent<Image>().enabled = false;
        Description.GetComponent<Image>().enabled = false;
        DescriptionText.GetComponent<Text>().enabled = false;

        actionDescription.GetComponent<Image>().enabled = false;
        actionDescriptionText.GetComponent<Text>().enabled = false;

        foreach (Transform child in attackText)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in supportText)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in itemText)
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in playerTarget)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in enemyTarget)
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in player)
        {
            child.gameObject.GetComponent<Status>().SetRayCast(false);
        }
        foreach (Transform child in enemy)
        {
            child.gameObject.GetComponent<Status>().SetRayCast(false);
        }

    }

    //  行動選択時に表示
    public void commandDisplay()
    {
        commandEnd = false;
        Attack.GetComponent<Image>().enabled = false;
        Support.GetComponent<Image>().enabled = false;
        Item.GetComponent<Image>().enabled = false;

        Attack.GetComponent<Image>().enabled = true;
        Support.GetComponent<Image>().enabled = true;
        Item.GetComponent<Image>().enabled = true;
    }

    //  攻撃テキストの表示
    public void SetAttackText()
    {
        Attack.GetComponent<Image>().enabled = false;
        Support.GetComponent<Image>().enabled = false;
        Item.GetComponent<Image>().enabled = false;

        SkillList.GetComponent<Image>().enabled = true;
        Back.GetComponent<Image>().enabled = true;

        getAttackList();
        skillType = TYPE.TYPE_ATTACK;
    }

    //  支援テキストの表示
    public void SetSupportText()
    {
        Attack.GetComponent<Image>().enabled = false;
        Support.GetComponent<Image>().enabled = false;
        Item.GetComponent<Image>().enabled = false;

        SkillList.GetComponent<Image>().enabled = true;
        Back.GetComponent<Image>().enabled = true;

        getSupportList();
        skillType = TYPE.TYPE_SUPPORT;
    }

    //   アイテムテキストの表示
    public void SetItemText()
    {
        Attack.GetComponent<Image>().enabled = false;
        Support.GetComponent<Image>().enabled = false;
        Item.GetComponent<Image>().enabled = false;

        SkillList.GetComponent<Image>().enabled = true;
        Back.GetComponent<Image>().enabled = true;

        getItemList();
        skillType = TYPE.TYPE_ITEM;
    }

    //  攻撃テキストの読み込み
    private void getAttackList()
    {
        PC = Resources.Load("ExcelData/player_chara") as player_charaList;
        PS = Resources.Load("ExcelData/playerSkill") as player_skillList;
        var IDList = new List<int>();
        IDList.Add((int)PC.sheets[0].list[charID].AttackSkill1);
        IDList.Add((int)PC.sheets[0].list[charID].AttackSkill2);
        IDList.Add((int)PC.sheets[0].list[charID].AttackSkill3);
        IDList.Add((int)PC.sheets[0].list[charID].AttackSkill4);
        IDList.Add((int)PC.sheets[0].list[charID].AttackSkill5);
        IDList.Add((int)PC.sheets[0].list[charID].AttackSkill6);
        IDList.Add((int)PC.sheets[0].list[charID].AttackSkill7);
        IDList.Add((int)PC.sheets[0].list[charID].AttackSkill8);
        for (int count = 0; count < IDList.Count - 1; count++)
        {
            if (IDList[count] != 0)
            {
                if (charLV >= PS.sheets[0].list[IDList[count] - 1].Lv)
                {
                    attackText.GetChild(count).gameObject.SetActive(true);
                    attackText.GetChild(count).gameObject.GetComponent<Text>().text = PS.sheets[0].list[IDList[count] - 1].skillName;
                }
            }
        }
        SkillID = (int)PC.sheets[0].list[charID].AttackSkill1 - 1;
    }

    //  支援テキストの読み込み
    private void getSupportList()
    {
        PC = Resources.Load("ExcelData/player_chara") as player_charaList;
        PS = Resources.Load("ExcelData/playerSkill") as player_skillList;
        var IDList = new List<int>();
        IDList.Add((int)PC.sheets[0].list[charID].SupportSkill1);
        IDList.Add((int)PC.sheets[0].list[charID].SupportSkill2);
        IDList.Add((int)PC.sheets[0].list[charID].SupportSkill3);
        IDList.Add((int)PC.sheets[0].list[charID].SupportSkill4);
        IDList.Add((int)PC.sheets[0].list[charID].SupportSkill5);
        IDList.Add((int)PC.sheets[0].list[charID].SupportSkill6);
        IDList.Add((int)PC.sheets[0].list[charID].SupportSkill7);
        IDList.Add((int)PC.sheets[0].list[charID].SupportSkill8);
        for (int count = 0; count < IDList.Count - 1; count++)
        {
            if (IDList[count] != 0)
            {
                if (charLV >= PS.sheets[0].list[IDList[count] - 1].Lv)
                {
                    supportText.GetChild(count).gameObject.SetActive(true);
                    supportText.GetChild(count).gameObject.GetComponent<Text>().text = PS.sheets[0].list[IDList[count] - 1].skillName;
                }
            }
        }
        SkillID = (int)PC.sheets[0].list[charID].SupportSkill1 - 1;
    }

    //  アイテムテキストの読み込み
    private void getItemList()
    {
        //PC = Resources.Load("ExcelData/player_chara") as player_charaList;
        //PS = Resources.Load("ExcelData/playerSkill") as player_skillList;
        //var IDList = new List<int>();
        //IDList.Add((int)PC.sheets[0].list[charID].AttackSkill1);
        //IDList.Add((int)PC.sheets[0].list[charID].AttackSkill2);
        //IDList.Add((int)PC.sheets[0].list[charID].AttackSkill3);
        //IDList.Add((int)PC.sheets[0].list[charID].AttackSkill4);
        //IDList.Add((int)PC.sheets[0].list[charID].AttackSkill5);
        //IDList.Add((int)PC.sheets[0].list[charID].AttackSkill6);
        //IDList.Add((int)PC.sheets[0].list[charID].AttackSkill7);
        //IDList.Add((int)PC.sheets[0].list[charID].AttackSkill8);
        //for (int count = 0; count < IDList.Count - 1; count++)
        //{
        //    if (IDList[count] != 0)
        //    {
        //        itemText.GetChild(count).gameObject.SetActive(true);
        //        itemText.GetChild(count).gameObject.GetComponent<Text>().text = PS.sheets[0].list[IDList[count] - 1].skillName;
        //    }
        //}
    }

    //  戻るボタンを押したときの処理
    public void BackInput()
    {
        Init();
        Attack.GetComponent<Image>().enabled = true;
        Support.GetComponent<Image>().enabled = true;
        Item.GetComponent<Image>().enabled = true;

        skillType = TYPE.TYPE_NON;
    }


    //  技の説明&ターゲットの表示
    public void SkillDescription()
    {
        //  技の説明表示
        Description.GetComponent<Image>().enabled = false;
        DescriptionText.GetComponent<Text>().enabled = false;
        foreach (Transform child in playerTarget)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in enemyTarget)
        {
            child.gameObject.SetActive(false);
        }

        Description.GetComponent<Image>().enabled = true;
        DescriptionText.GetComponent<Text>().enabled = true;

        int skillNumber = 0;

        //  技ごとにターゲット表示
        PC = Resources.Load("ExcelData/player_chara") as player_charaList;
        PS = Resources.Load("ExcelData/playerSkill") as player_skillList;
        if (skillType == TYPE.TYPE_ATTACK)
        {
            switch(SkillName)
            {
                case 1:
                    skillNumber = (int)PC.sheets[0].list[charID].AttackSkill1;
                    break;
                case 2:
                    skillNumber = (int)PC.sheets[0].list[charID].AttackSkill2;
                    break;
                case 3:
                    skillNumber = (int)PC.sheets[0].list[charID].AttackSkill3;
                    break;
                case 4:
                    skillNumber = (int)PC.sheets[0].list[charID].AttackSkill4;
                    break;
                case 5:
                    skillNumber = (int)PC.sheets[0].list[charID].AttackSkill5;
                    break;
                case 6:
                    skillNumber = (int)PC.sheets[0].list[charID].AttackSkill6;
                    break;
                case 7:
                    skillNumber = (int)PC.sheets[0].list[charID].AttackSkill7;
                    break;
                case 8:
                    skillNumber = (int)PC.sheets[0].list[charID].AttackSkill8;
                    break;
                default:
                    break;
            }
        }
        else if (skillType == TYPE.TYPE_SUPPORT)
        {
            switch(SkillName)
            {
                case 1:
                    skillNumber = (int)PC.sheets[0].list[charID].SupportSkill1;
                    break;
                case 2:
                    skillNumber = (int)PC.sheets[0].list[charID].SupportSkill2;
                    break;
                case 3:
                    skillNumber = (int)PC.sheets[0].list[charID].SupportSkill3;
                    break;
                case 4:
                    skillNumber = (int)PC.sheets[0].list[charID].SupportSkill4;
                    break;
                case 5:
                    skillNumber = (int)PC.sheets[0].list[charID].SupportSkill5;
                    break;
                case 6:
                    skillNumber = (int)PC.sheets[0].list[charID].SupportSkill6;
                    break;
                case 7:
                    skillNumber = (int)PC.sheets[0].list[charID].SupportSkill7;
                    break;
                case 8:
                    skillNumber = (int)PC.sheets[0].list[charID].SupportSkill8;
                    break;
                default:
                    break;
            }
        }
        else
        {

        }

        //  playerSkillのtargetから、ターゲットを取得
        string Name = PS.sheets[0].list[skillNumber - 1].target;
        int ActivePlayer = SetActivePlayer();

        //  味方に対する技
        if (Name.StartsWith("P"))
        {
            if (Name.Contains("0"))
            {
                playerTarget.GetChild(ActivePlayer).gameObject.SetActive(true);
            }
            else if(Name.Contains("1"))
            {
                playerTarget.GetChild(target).gameObject.SetActive(true);
                foreach (Transform child in player)
                {
                    child.gameObject.GetComponent<Status>().SetRayCast(true);
                }
            }
            else
            {
                foreach (Transform child in playerTarget)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        //  敵に対する技  
        else
        {
            if (Name.Contains("1"))
            {
                enemyTarget.GetChild(target).gameObject.SetActive(true);
                foreach (Transform child in enemy)
                {
                    child.gameObject.GetComponent<Status>().SetRayCast(true);
                }
            }
            else
            {
                foreach (Transform child in enemyTarget)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        DescriptionText.GetComponent<Text>().text = PS.sheets[0].list[skillNumber - 1].effect;
        SkillID = skillNumber - 1;
    }

    public void ActionStart()
    {
        actionDescription.GetComponent<Image>().enabled = true;
        actionDescriptionText.GetComponent<Text>().enabled = true;
        actionDescriptionText.GetComponent<Text>().text = ActiveSkillText;
    }

    public void ActionEnd()
    {
        actionDescription.GetComponent<Image>().enabled = false;
        actionDescriptionText.GetComponent<Text>().enabled = false;
    }

    public void SetSkillName(string name)
    {
        //  文字列中の数字を取得
        SkillName = int.Parse(Regex.Replace(name, @"[^0-9]", ""));
    }

    public void SetTarget(string id)
    {
        int IdNumber = int.Parse(Regex.Replace(id, @"[^0-9]", ""));
        //  補正
        int correction = 1;

        if (id.StartsWith("p"))
        {
            //  残っているプレイヤーの数
            int ObjCount = GetPlayerCount();
            switch (IdNumber)
            {
                case 1:
                    break;
                case 2:
                    if (ObjCount == 1)
                    {
                        correction += 1;
                    }
                    else if (ObjCount == 2)
                    {
                        if (player.GetChild(0).GetComponent<Status>().GetState() == Status.STATE.ST_DEAD)
                        {
                            correction += 1;
                        }
                    }
                    else
                    {
                    }
                    break;
                case 3:
                    if (ObjCount == 1)
                    {
                        correction += 2;
                    }
                    else if (ObjCount == 2)
                    {
                        correction += 1;
                    }
                    else
                    {
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            //  残っている敵の数
            int ObjCount = enemy.transform.childCount;
            switch (IdNumber)
            {
                case 1:
                    break;
                case 2:
                    if (ObjCount == 1)
                    {
                        correction += 1;
                    }
                    else if(ObjCount == 2)
                    {
                        if(enemy.GetChild(0).gameObject.name == "enemy2")
                        {
                            correction += 1;
                        }
                    }
                    else
                    {
                    }
                    break;
                case 3:
                    if(ObjCount == 1)
                    {
                        correction += 2;
                    }
                    else if(ObjCount == 2)
                    {
                        correction += 1;
                    }
                    else
                    {
                    }
                    break;
                default:
                    break;
            }
        }
        target = int.Parse(Regex.Replace(id, @"[^0-9]", "")) - correction;
    }

    public void SetTargetStart(string id)
    {
        //int ID = int.Parse(Regex.Replace(id, @"[^0-9]", ""));
        //if (id.StartsWith("p"))
        //{
        //    GameObject.Destroy(playerTarget.GetChild(ID - 1));
        //    GameObject.Destroy(player.GetChild(ID - 1));
        //    playerTarget.GetChild(ID - 1).DetachChildren();
        //    player.GetChild(ID - 1).gameObject.SetActive(true);
        //}
        //else
        //{
        //    GameObject.Destroy(enemyTarget.GetChild(ID - 1));
        //    GameObject.Destroy(enemy.GetChild(ID - 1));
        //    enemyTarget.GetChild(ID - 1).gameObject.SetActive(true);
        //    enemy.GetChild(ID - 1).gameObject.SetActive(true);
        //}
    }


    public void SetCharID(int id)
    {
        charID = id;
    }

    public void SetLV(int lv)
    {
        charLV = lv;
    }

    public void InitTarget()
    {
        target = 0;
    }

    //  ターゲット確定時の処理
    public void TargetDecided(GameObject ID,GameObject receive)
    {
        string Iname = ID.name;
        GameObject characterID;
        if (Iname.StartsWith("p"))
        {
            if (Iname.Contains("1"))
            {
                characterID = GameObject.Find("player1");
            }
            else if (Iname.Contains("2"))
            {
                characterID = GameObject.Find("player2");
            }
            else
            {
                characterID = GameObject.Find("player3");
            }
        }
        else
        {
            if (Iname.Contains("1"))
            {
                characterID = GameObject.Find("enemy1");
            }
            else if (Iname.Contains("2"))
            {
                characterID = GameObject.Find("enemy2");
            }
            else
            {
                characterID = GameObject.Find("enemy3");
            }
        }

        //  使用するスキルの情報とスキルを向ける相手を保存する
        characterID.GetComponent<Status>().SaveSkill(SkillID);
        setReceive(characterID,receive);

        Init();
        battleManager.GetComponent<BattleScene>().SetActiveChoose(false);
        commandEnd = true;
    }

    //  技ごとにターゲット数を増やす
    public void setReceive(GameObject characterID, GameObject receive)
    {
        string SkillTarget = PS.sheets[0].list[SkillID].target;
        if(SkillTarget.StartsWith("P"))
        {
            if (SkillTarget.Contains("0") || SkillTarget.Contains("1"))
            {
                characterID.GetComponent<Status>().SaveReceive(receive);
            }
            else if (SkillTarget.Contains("2"))
            {
                //  また後で------------------------------------------------------------
            }
            else
            {
                if (player.transform.childCount == 3)
                {
                    characterID.GetComponent<Status>().SaveReceive(player.GetChild(0).gameObject, player.GetChild(1).gameObject, player.GetChild(2).gameObject);
                }
                else if (enemy.transform.childCount == 2)
                {
                    characterID.GetComponent<Status>().SaveReceive(player.GetChild(0).gameObject, player.GetChild(1).gameObject);
                }
                else
                {
                    characterID.GetComponent<Status>().SaveReceive(player.GetChild(0).gameObject);
                }
            }
        }
        else
        {
            if (SkillTarget.Contains("1"))
            {
                characterID.GetComponent<Status>().SaveReceive(receive);
            }
            else if (SkillTarget.Contains("2"))
            {
                //  存在しない------------------------------------------------------------
            }
            else
            {
                if(enemy.transform.childCount == 3)
                {
                    characterID.GetComponent<Status>().SaveReceive(enemy.GetChild(0).gameObject, enemy.GetChild(1).gameObject, enemy.GetChild(2).gameObject);
                }
                else if(enemy.transform.childCount == 2)
                {
                    characterID.GetComponent<Status>().SaveReceive(enemy.GetChild(0).gameObject, enemy.GetChild(1).gameObject);
                }
                else
                {
                    characterID.GetComponent<Status>().SaveReceive(enemy.GetChild(0).gameObject);
                }
            }
        }
    }


    public void SetActiveSkillText(string text)
    {
        ActiveSkillText = text;
    }


    public bool GetCommandEnd()
    {
        return commandEnd;
    }

    public int SetActivePlayer()
    {
        int ID = GetComponent<BattleScene>().GetActivePlayer();
        //  残っているプレイヤーの数
        int ObjCount = GetPlayerCount();
        int correction = 1;

        switch (ID)
        {
            case 1:
                break;
            case 2:
                if (ObjCount == 1)
                {
                    correction += 1;
                }
                else if (ObjCount == 2)
                {
                    if (player.GetChild(0).GetComponent<Status>().GetState() == Status.STATE.ST_DEAD)
                    {
                        correction += 1;
                    }
                }
                else
                {
                }
                break;
            case 3:
                if (ObjCount == 1)
                {
                    correction += 2;
                }
                else if (ObjCount == 2)
                {
                    correction += 1;
                }
                else
                {
                }
                break;
            default:
                break;
        }
        return ID - correction;
    }

    //  標的が攻撃前に死んでしまった時用
    public void changeTarget(bool playerProof,string target)
    {
        if (playerProof)
        {
            if (target.StartsWith("P"))
            {
                if(player.GetChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                {
                    battleManager.GetComponent<BattleScene>().SetReceiveObj(player.GetChild(0).gameObject);
                }
                else if (player.GetChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                {
                    battleManager.GetComponent<BattleScene>().SetReceiveObj(player.GetChild(1).gameObject);
                }
                else
                {
                    battleManager.GetComponent<BattleScene>().SetReceiveObj(player.GetChild(2).gameObject);
                }
            }
            else
            {
                battleManager.GetComponent<BattleScene>().SetReceiveObj(enemy.GetChild(0).gameObject);
            }
        }
        else
        {
            if (target.StartsWith("P"))
            {
                battleManager.GetComponent<BattleScene>().SetReceiveObj(enemy.GetChild(0).gameObject);
            }
            else
            {
                if (player.GetChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                {
                    battleManager.GetComponent<BattleScene>().SetReceiveObj(player.GetChild(0).gameObject);
                }
                else if (player.GetChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                {
                    battleManager.GetComponent<BattleScene>().SetReceiveObj(player.GetChild(1).gameObject);
                }
                else
                {
                    battleManager.GetComponent<BattleScene>().SetReceiveObj(player.GetChild(2).gameObject);
                }
            }
        }
    }

    //  行動時の色変更用
    public void changeColor(Color clr)
    {
        foreach (Transform child in player)
        {
            child.gameObject.GetComponent<Image>().color = clr;
        }
        foreach (Transform child in enemy)
        {
            child.gameObject.GetComponent<Image>().color = clr;
        }
    }

    //  プレイヤーの数の取得
    public int GetPlayerCount()
    {
        int count = player.transform.childCount;
        foreach (Transform child in player)
        {
            //  プレイヤー
            if(child.gameObject.GetComponent<Status>().state == Status.STATE.ST_DEAD)
            {
                count--;
            }
        }
        return count;
    }

    //  敵の数の取得
    public int GetEnemyCount()
    {
        return enemy.transform.childCount;
    }


    //  プレイヤーと敵の子オブジェクトの取得
    public GameObject GetPlayerChild(int id)
    {
        if(player.GetChild(id))
        {
            return player.GetChild(id).gameObject;
        }
        return null;
    }

    public GameObject GetEnemyChild(int id)
    {
        return enemy.GetChild(id).gameObject;
    }

    //  プレイヤーが全滅していたらfalseを返す
    public bool PlayerAlive()
    {
        foreach (Transform child in player)
        {
            //  プレイヤー
            if (child.gameObject.GetComponent<Status>().state != Status.STATE.ST_DEAD)
            {
                return true;
            }
        }
        return false;
    }
}
