using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleScene : MonoBehaviour
{
    Queue<Action> battleQueue;
    [SerializeField] private GameObject attackObj;
    [SerializeField] private GameObject receiveObj1;
    [SerializeField] private GameObject receiveObj2;
    [SerializeField] private GameObject receiveObj3;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject winner;
    public GameObject loser;

    public GameObject PEffect1;
    public GameObject PEffect2;
    public GameObject PEffect3;
    public GameObject EEffect1;
    public GameObject EEffect2;
    public GameObject EEffect3;

    public GameObject PAllEffect;
    public GameObject EAllEffect;

    public GameObject BFinish;

    private int pID1;
    private int pID2;
    private int pID3;
    private int eID1;
    private int eID2;
    private int eID3;


    private command co;

    private GameObject sceneNavigator;

    //  コマンド選択開始用フラグ
    private bool ActiveChoose;

    //  行動中かどうかのフラグ
    private bool ActionFlag;

    public GameObject ActivePlayer;

    public Transform TL;

    //  win!とlose表示時用
    //  透明
    Color fadeColor1 = new Color(1.0f, 1.0f, 1.0f, 0);
    //  薄暗い
    Color fadeColor2 = new Color(0.5f, 0.5f, 0.5f, 0.4f);
    //  通常時の色
    Color fadeColor3 = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    public GameObject Fade;

    private bool SEFlag;

    //  エフェクトの名前
    private string animName;
    //  エフェクトを単体か全体のどちらかを使うかの判断用
    private bool allTarget;

    public struct Action
    {
        //public Performance p;
        public Damage damage;
        public Heal heal;
        public Buff buff;
    }

    void Start()
    {
        sceneNavigator = GameObject.Find("SceneNavigator");
        sceneNavigator.GetComponent<StatusControl>().GetPlayerList(ref pID1, ref pID2, ref pID3);
        sceneNavigator.GetComponent<StatusControl>().GetEnemyList(ref eID1, ref eID2, ref eID3);

        battleQueue = new Queue<Action>();
        foreach (Transform child in TL)
        {
            child.gameObject.SetActive(true);
        }
        Player1.GetComponent<Status>().SetChara();
        Player2.GetComponent<Status>().SetChara();
        Player3.GetComponent<Status>().SetChara();
        Enemy1.GetComponent<Status>().SetChara();
        Enemy2.GetComponent<Status>().SetChara();
        Enemy3.GetComponent<Status>().SetChara();

        SEFlag = false;
        animName = "null";
        allTarget = false;
    }

    void Update()
    {
        //  敵が全滅した時の処理
        if (GetComponent<command>().GetEnemyCount() == 0)
        {
            Fade.GetComponent<Image>().color = fadeColor2;
            winner.GetComponent<Image>().color = fadeColor3;
            if (!SEFlag)
            {
                sceneNavigator.GetComponent<BGMselect>().SetSE("クリア時");
                sceneNavigator.GetComponent<StatusControl>().AcquisitionExp(pID1,sceneNavigator.GetComponent<StatusControl>().GetEXP());
                sceneNavigator.GetComponent<StatusControl>().AcquisitionExp(pID2, sceneNavigator.GetComponent<StatusControl>().GetEXP());
                sceneNavigator.GetComponent<StatusControl>().AcquisitionExp(pID3, sceneNavigator.GetComponent<StatusControl>().GetEXP());
                SEFlag = true;
            }
            BFinish.GetComponent<Image>().enabled = true;
        }
        //  プレイヤーが全滅した時の処理
        else if (!GetComponent<command>().PlayerAlive())
        {
            Fade.GetComponent<Image>().color = fadeColor2;
            loser.GetComponent<Image>().color = fadeColor3;
            BFinish.GetComponent<Image>().enabled = true;
        }
        else
        {
            winner.GetComponent<Image>().color = fadeColor1;
            loser.GetComponent<Image>().color = fadeColor1;
            Fade.GetComponent<Image>().color = fadeColor1;
            BFinish.GetComponent<Image>().enabled = false;
        }
    }

    public void TakeAction(int spcost, int HpFlag, int AttackType, double healPercent,string AName,bool targetFlag)
    {
        animName = AName;
        allTarget = targetFlag;
        attackObj.GetComponent<Status>().SetBuff();
        if(receiveObj1 != null)
        {
            receiveObj1.GetComponent<Status>().SetBuff();
        }
        if (receiveObj2 != null)
        {
            receiveObj2.GetComponent<Status>().SetBuff();
        }
        if (receiveObj3 != null)
        {
            receiveObj3.GetComponent<Status>().SetBuff();
        }

        //  1ならダメージを与える処理
        if (HpFlag == 1)
        {
            Action action1 = new Action()
            {
                damage = new Damage { attackChara = attackObj, receiveChara1 = receiveObj1, receiveChara2 = receiveObj2, receiveChara3 = receiveObj3, spCost = spcost, attackType = AttackType}
            };
            battleQueue.Enqueue(action1);
        }
        //  2ならHPを回復する処理
        else if(HpFlag == 2)
        {
            //  回復時は全体演出ではなく単体演出を使う
            allTarget = false;

            Action action1 = new Action()
            {
                heal = new Heal { attackChara = attackObj, receiveChara1 = receiveObj1, receiveChara2 = receiveObj2, receiveChara3 = receiveObj3, spCost = spcost, HealPercent = healPercent}
            };
            battleQueue.Enqueue(action1);
        }
        //  3ならダメージを与えつつ回復する処理
        else if(HpFlag == 3)
        {
            //Action action1 = new Action()
            //{
            //    damage = new Damage { attackChara = attackObj, receiveChara = receiveObj, spCost = spcost, atk = power }
            //};
            //battleQueue.Enqueue(action1);
        }
        //  バフ、デバフ系の処理
        else
        {
            //  バフ、デバフ時は全体演出ではなく単体演出を使う
            allTarget = false;
            Action action1 = new Action()
            {
                buff = new Buff { attackChara = attackObj, receiveChara1 = receiveObj1, receiveChara2 = receiveObj2, receiveChara3 = receiveObj3, spCost = spcost }
            };
            battleQueue.Enqueue(action1);
        }

        Action action = battleQueue.Dequeue();
        if (HpFlag == 1)
        {
            action.damage.processing();
        }
        else if (HpFlag == 2)
        {
            action.heal.processing();
        }
        else
        {
            action.buff.processing();
        }
    }

    public class Damage
    {
        public GameObject attackChara;
        public GameObject receiveChara1;
        public GameObject receiveChara2;
        public GameObject receiveChara3;
        public int spCost;
        public int attackType;
        public int atk;
        public int def;
        public int TotalDamage1;
        public int TotalDamage2;
        public int TotalDamage3;

        GameObject battleManager;

        public void processing()
        {
            battleManager = GameObject.Find("BattleManager");

            attackChara.GetComponent<Status>().SetSP(attackChara.GetComponent<Status>().GetSP() - spCost);
            attackChara.GetComponent<Status>().SetState(Status.STATE.ST_ATK);

            if (receiveChara1)
            {
                //  物理攻撃
                if (attackType == 1)
                {
                    atk = attackChara.GetComponent<Status>().GetATK();
                    def = receiveChara1.GetComponent<Status>().GetDEF();
                }
                //  魔法攻撃
                else if (attackType == 2)
                {
                    atk = attackChara.GetComponent<Status>().GetMAT();
                    def = receiveChara1.GetComponent<Status>().GetMDF();
                }
                else
                {
                }

                TotalDamage1 = atk - def;
                if (TotalDamage1 <= 0)
                {
                    TotalDamage1 = 1;
                }
                receiveChara1.GetComponent<Status>().SetHP(receiveChara1.GetComponent<Status>().GetHP() - TotalDamage1);

                Debug.Log(attackChara.gameObject.name + "が" + receiveChara1.gameObject.name + "に" + TotalDamage1 + "ダメージ！");
                Debug.Log("残りHP" + receiveChara1.GetComponent<Status>().GetHP());
                receiveChara1.GetComponent<Status>().SetState(Status.STATE.ST_DAMAGE);
                battleManager.GetComponent<BattleScene>().SetEffect(receiveChara1.name);
            }

            if(receiveChara2)
            {
                //  物理攻撃
                if (attackType == 1)
                {
                    atk = attackChara.GetComponent<Status>().GetATK();
                    def = receiveChara2.GetComponent<Status>().GetDEF();
                }
                //  魔法攻撃
                else if (attackType == 2)
                {
                    atk = attackChara.GetComponent<Status>().GetMAT();
                    def = receiveChara2.GetComponent<Status>().GetMDF();
                }
                else
                {
                }

                TotalDamage2 = atk - def;
                if (TotalDamage2 <= 0)
                {
                    TotalDamage2 = 1;
                }
                receiveChara2.GetComponent<Status>().SetHP(receiveChara2.GetComponent<Status>().GetHP() - TotalDamage2);
                Debug.Log(attackChara.gameObject.name + "が" + receiveChara2.gameObject.name + "に" + TotalDamage2 + "ダメージ！");
                Debug.Log("残りHP" + receiveChara2.GetComponent<Status>().GetHP());
                receiveChara2.GetComponent<Status>().SetState(Status.STATE.ST_DAMAGE);
                battleManager.GetComponent<BattleScene>().SetEffect(receiveChara2.name);
            }

            if (receiveChara3)
            {
                //  物理攻撃
                if (attackType == 1)
                {
                    atk = attackChara.GetComponent<Status>().GetATK();
                    def = receiveChara3.GetComponent<Status>().GetDEF();
                }
                //  魔法攻撃
                else if (attackType == 2)
                {
                    atk = attackChara.GetComponent<Status>().GetMAT();
                    def = receiveChara3.GetComponent<Status>().GetMDF();
                }
                else
                {
                }

                TotalDamage3 = atk - def;
                if (TotalDamage3 <= 0)
                {
                    TotalDamage3 = 1;
                }
                receiveChara3.GetComponent<Status>().SetHP(receiveChara3.GetComponent<Status>().GetHP() - TotalDamage3);
                Debug.Log(attackChara.gameObject.name + "が" + receiveChara3.gameObject.name + "に" + TotalDamage3 + "ダメージ！");
                Debug.Log("残りHP" + receiveChara3.GetComponent<Status>().GetHP());
                receiveChara3.GetComponent<Status>().SetState(Status.STATE.ST_DAMAGE);
                battleManager.GetComponent<BattleScene>().SetEffect(receiveChara3.name);
            }

            //  全体攻撃用エフェクト(味方か敵かの判別が必要なのでどれか一つの名前が入ればいい)
            if(receiveChara1)
            {
                battleManager.GetComponent<BattleScene>().SetAllEffect(receiveChara1.name);
            }
            else if (receiveChara2)
            {
                battleManager.GetComponent<BattleScene>().SetAllEffect(receiveChara2.name);
            }
            else
            {
                battleManager.GetComponent<BattleScene>().SetAllEffect(receiveChara3.name);
            }
        }
    }

    public class Heal
    {
        public GameObject attackChara;
        public GameObject receiveChara1;
        public GameObject receiveChara2;
        public GameObject receiveChara3;
        public int spCost;
        public double HealPercent;
        public int TotalHeal1;
        public int TotalHeal2;
        public int TotalHeal3;

        GameObject battleManager;
        
        public void processing()
        {
            battleManager = GameObject.Find("BattleManager");
            attackChara.GetComponent<Status>().SetSP(attackChara.GetComponent<Status>().GetSP() - spCost);
            attackChara.GetComponent<Status>().SetState(Status.STATE.ST_ATK);
            
            if (receiveChara1)
            {
                TotalHeal1 = (int)Math.Round(receiveChara1.GetComponent<Status>().GetMAXHP() * HealPercent);
                if (TotalHeal1 <= 0)
                {
                    TotalHeal1 = 1;
                }
                receiveChara1.GetComponent<Status>().SetHP(receiveChara1.GetComponent<Status>().GetHP() + TotalHeal1);

                Debug.Log(receiveChara1.gameObject.name + "のHPが" + TotalHeal1 + "かいふく！");
                Debug.Log("残りHP" + receiveChara1.GetComponent<Status>().GetHP());
                battleManager.GetComponent<BattleScene>().SetEffect(receiveChara1.name);
            }

            if (receiveChara2)
            {
                TotalHeal2 = (int)Math.Round(receiveChara2.GetComponent<Status>().GetMAXHP() * HealPercent);
                if (TotalHeal2 <= 0)
                {
                    TotalHeal2 = 1;
                }
                receiveChara2.GetComponent<Status>().SetHP(receiveChara2.GetComponent<Status>().GetHP() + TotalHeal2);

                Debug.Log(receiveChara2.gameObject.name + "のHPが" + TotalHeal2 + "かいふく！");
                Debug.Log("残りHP" + receiveChara2.GetComponent<Status>().GetHP());
                battleManager.GetComponent<BattleScene>().SetEffect(receiveChara2.name);
            }

            if (receiveChara3)
            {
                TotalHeal3 = (int)Math.Round(receiveChara3.GetComponent<Status>().GetMAXHP() * HealPercent);
                if (TotalHeal3 <= 0)
                {
                    TotalHeal3 = 1;
                }
                receiveChara3.GetComponent<Status>().SetHP(receiveChara1.GetComponent<Status>().GetHP() + TotalHeal3);

                Debug.Log(receiveChara3.gameObject.name + "のHPが" + TotalHeal3 + "かいふく！");
                Debug.Log("残りHP" + receiveChara3.GetComponent<Status>().GetHP());
                battleManager.GetComponent<BattleScene>().SetEffect(receiveChara3.name);
            }

            //  全体攻撃用エフェクト(味方か敵かの判別が必要なのでどれか一つの名前が入ればいい)
            if (receiveChara1)
            {
                battleManager.GetComponent<BattleScene>().SetAllEffect(receiveChara1.name);
            }
            else if (receiveChara2)
            {
                battleManager.GetComponent<BattleScene>().SetAllEffect(receiveChara2.name);
            }
            else
            {
                battleManager.GetComponent<BattleScene>().SetAllEffect(receiveChara3.name);
            }
        }
    }

    public class Buff
    {
        public GameObject attackChara;
        public GameObject receiveChara1;
        public GameObject receiveChara2;
        public GameObject receiveChara3;
        public int spCost;

        GameObject battleManager;

        public void processing()
        {
            battleManager = GameObject.Find("BattleManager");
            attackChara.GetComponent<Status>().SetState(Status.STATE.ST_DEF);
            if (receiveChara1)
            {
                Debug.Log(receiveChara1.gameObject.name + "の"/* + TotalHeal*/ + "が変化！");
                Debug.Log("残りHP" + receiveChara1.GetComponent<Status>().GetHP());
                battleManager.GetComponent<BattleScene>().SetEffect(receiveChara1.name);
            }
            if (receiveChara2)
            {
                Debug.Log(receiveChara2.gameObject.name + "の"/* + TotalHeal*/ + "が変化！");
                Debug.Log("残りHP" + receiveChara2.GetComponent<Status>().GetHP());
                battleManager.GetComponent<BattleScene>().SetEffect(receiveChara2.name);
            }
            if (receiveChara3)
            {
                Debug.Log(receiveChara3.gameObject.name + "の"/* + TotalHeal*/ + "が変化！");
                Debug.Log("残りHP" + receiveChara3.GetComponent<Status>().GetHP());
                battleManager.GetComponent<BattleScene>().SetEffect(receiveChara3.name);
            }

            //  全体攻撃用エフェクト(味方か敵かの判別が必要なのでどれか一つの名前が入ればいい)
            if (receiveChara1)
            {
                battleManager.GetComponent<BattleScene>().SetAllEffect(receiveChara1.name);
            }
            else if (receiveChara2)
            {
                battleManager.GetComponent<BattleScene>().SetAllEffect(receiveChara2.name);
            }
            else
            {
                battleManager.GetComponent<BattleScene>().SetAllEffect(receiveChara3.name);
            }
        }
    }

    public void ActionSelectEnd(GameObject obj)
    {
        GetComponent<command>().TargetDecided(ActivePlayer,obj);
    }

    public int GetPID(int name)
    {
        if(name == 1)
        {
            return pID1;
        }
        else if (name == 2)
        {
            return pID2;
        }
        else
        {
            return pID3;
        }
    }

    public int GetEID(int name)
    {
        if (name == 1)
        {
            return eID1;
        }
        else if (name == 2)
        {
            return eID2;
        }
        else
        {
            return eID3;
        }
    }

    public void SetActivePlayer(GameObject obj)
    {
        ActivePlayer = obj;
    }

    public int GetActivePlayer()
    {
        return int.Parse(Regex.Replace(ActivePlayer.name, @"[^0-9]", ""));
    }

    public void SetActiveChoose(bool flag)
    {
        ActiveChoose = flag;
    }

    public bool GetActiveChoose()
    {
        return ActiveChoose;
    }

    //  攻撃するキャラをセット
    public void SetAttackObj(GameObject obj)
    {
        attackObj = obj;
    }

    public GameObject GetAttackObj()
    {
        return attackObj;
    }

    //  攻撃を受けるキャラをセット
    public void SetReceiveObj(GameObject obj1)
    {
        receiveObj1 = obj1;
    }

    public void SetReceiveObj(GameObject obj1,GameObject obj2)
    {
        receiveObj1 = obj1;
        receiveObj2 = obj2;
    }

    public void SetReceiveObj(GameObject obj1, GameObject obj2,GameObject obj3)
    {
        receiveObj1 = obj1;
        receiveObj2 = obj2;
        receiveObj3 = obj3;
    }

    public GameObject GetReceiveObj(int number)
    {
        if(number == 1)
        {
            return receiveObj1;
        }
        if(number == 2)
        {
            return receiveObj2;
        }
        if(number == 3)
        {
            return receiveObj3;
        }
        return receiveObj1;
    }

    public void SetActionFlag(bool flag)
    {
        ActionFlag = flag;
    }

    public bool GetActionFlag()
    {
        return ActionFlag;
    }

    //  単体攻撃時
    public void SetEffect(string receive)
    {
        if (!allTarget)
        {
            if (receive.StartsWith("p"))
            {
                if (receive.Contains("1"))
                {
                    PEffect1.GetComponent<EffectControl>().SetAnimName(animName);
                }
                else if (receive.Contains("2"))
                {
                    PEffect2.GetComponent<EffectControl>().SetAnimName(animName);
                }
                else
                {
                    PEffect3.GetComponent<EffectControl>().SetAnimName(animName);
                }
            }
            else
            {
                if (receive.Contains("1"))
                {
                    EEffect1.GetComponent<EffectControl>().SetAnimName(animName);
                }
                else if (receive.Contains("2"))
                {
                    EEffect2.GetComponent<EffectControl>().SetAnimName(animName);
                }
                else
                {
                    EEffect3.GetComponent<EffectControl>().SetAnimName(animName);
                }
            }
        }
    }

    //  全体攻撃時
    public void SetAllEffect(string receive)
    {
        if (allTarget)
        {
            if (receive.StartsWith("p"))
            {
                PAllEffect.GetComponent<EffectControl>().SetAnimName(animName);
            }
            else
            {
                EAllEffect.GetComponent<EffectControl>().SetAnimName(animName);
            }
        }
    }
}
