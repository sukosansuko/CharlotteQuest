using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Status : MonoBehaviour
{
    [SerializeField] private string CharName;   //  キャラクターの名前
    [SerializeField] private int LV;            //  レベル
    [SerializeField] private int HP;            //  体力
    [SerializeField] private int SP;            //  技の発動に必要
    [SerializeField] private int ATK;           //  物理攻撃力
    [SerializeField] private int DEF;           //  物理防御力
    [SerializeField] private int SPD;           //  素早さ
    [SerializeField] private int MAT;           //  魔法攻撃力
    [SerializeField] private int MDF;           //  魔法防御力
    [SerializeField] private int LUK;           //  幸運値

    private int WAIT_SPD;                       //  技発動までの待機用の素早さ

    //  プレイヤー用画像読み込み
    [SerializeField] private Sprite psp1;
    [SerializeField] private Sprite psp2;
    [SerializeField] private Sprite psp3;
    [SerializeField] private Sprite psp4;
    [SerializeField] private Sprite psp5;
    [SerializeField] private Sprite psp6;

    //  プレイヤー用アイコン
    [SerializeField] private Sprite pIcon1;
    [SerializeField] private Sprite pIcon2;
    [SerializeField] private Sprite pIcon3;
    [SerializeField] private Sprite pIcon4;
    [SerializeField] private Sprite pIcon5;
    [SerializeField] private Sprite pIcon6;

    public Button ContinueBtn;

    //  通常時の色
    Color color1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //  薄暗い
    Color color2 = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    private int TURN;                           //  行動回数
    private string AResistance;                 //  物理耐性(基本は敵のみ)
    private string MResistance;                 //  魔法耐性(基本は敵のみ)
    private string weakAttribute;               //  弱点属性(基本は敵のみ)
    private int MAX_HP;                         //  HPの最大値
    private int MAX_SP;                         //  SPの最大値

    public GameObject MyTarget;
    public GameObject TLIcon;
    public GameObject turnPointer;
    //  敵用HP
    public GameObject EHPDisplay;
    //  プレイヤー用ステータス表示
    public GameObject charName;
    public GameObject PHPDisplay;
    public GameObject PSPDisplay;
    public GameObject PCondition;


    player_charaList PC;
    player_skillList PS;

    enemy_charaList EC;
    enemy_skillList ES;

    private List<int> SaveSkillIDList = new List<int>();

    //  TLのアイコン位置計算用
    private Vector3 tmp;

    //  キャラクターの状態(アニメーションなどで使う)
    public enum STATE
    {
        ST_NON,
        ST_STAND,                   //  通常時
        ST_ATK,                     //  攻撃中
        ST_DEF,                     //  防御中
        ST_DAMAGE,                  //  ダメージを受けた
        ST_DEAD,                    //  死亡
        ST_MAX
    }

    //  バフ、デバフの管理用
    struct strData
    {
        public int TIME;            //  バフ、デバフが有効なターン数
        public int ATK;             //  物理攻撃力
        public int DEF;             //  物理防御力
        public int SPD;             //  素早さ
        public int MAT;             //  魔法攻撃力
        public int MDF;             //  魔法防御力
        public int LUK;             //  幸運値
    }

    //  攻撃を受けるキャラの保管用
    struct receiveChar
    {
        public GameObject RECEIVE1;
        public GameObject RECEIVE2;
        public GameObject RECEIVE3;
    }
    private List<receiveChar> SaveReceiveList = new List<receiveChar>();


    //  バフ、デバフ管理用のリスト
    private List<strData> changeStatus = new List<strData>();

    Image image;

    private GameObject battleManager;
    private GameObject sceneNavigator;

    //  行動選択時に攻撃または支援を与える相手格納用
    private GameObject TargetChar;

    //  プレイヤーかどうか判断するために使用する(プレイヤーならtrue)
    private bool playerProof = false;

    //  アタッチしているオブジェクトの名前
    public string Name;
    public int charID;

    //  TL上の進行度
    public float TLProgress;
    private bool progressEnd;

    public STATE state;

    private bool actionFlag = false;
    private int waitTime = 0;
    private bool skillInputFlag = false;

    private int spCost;
    private int HPCtlFlag;
    private int AttackType;
    private double HealPercent;

    private GameObject ReceiveChara1;
    private GameObject ReceiveChara2;
    private GameObject ReceiveChara3;

    private Animator anim;

    //  技のエフェクト用
    private string animName;
    //  全体技かどうかの判断用
    private bool targetFlag;

    void Start()
    {
        Name = this.gameObject.name;
        Debug.Log(Name);

        battleManager = GameObject.Find("BattleManager");
        sceneNavigator = GameObject.Find("SceneNavigator");

        //  技リストと被ダメージキャラリストの初期化
        for (int count = 0; count < 6; count++)
        {
            SaveSkillIDList.Add(0);
            SaveReceiveList.Add(new receiveChar { RECEIVE1 = null, RECEIVE2 = null, RECEIVE3 = null });
        }

        //TLIcon.GetComponent<Image>().enabled = false;
        //turnPointer.GetComponent<Image>().enabled = false;

        tmp = TLIcon.transform.position;
        TLIcon.transform.position = new Vector3(tmp.x,tmp.y,tmp.z);
        progressEnd = false;
        animName = "null";
        //SetChara();
    }

    void Update()
    {
        //  プレイヤーの場合はアニメーションを入れる
        if (playerProof)
        {
            SetAnim();
        }

        if (state == STATE.ST_DEAD)
        {
            Destroy(TLIcon);
            ContinueBtn.interactable = false;
            Destroy(MyTarget);
            if (!playerProof)
            {
                Destroy(EHPDisplay);
                Destroy(this.gameObject);
            }
        }
        else
        {
            state = STATE.ST_STAND;
            //  戦闘が続いている時の処理
            if (battleManager)
            {
                if (battleManager.GetComponent<command>().GetEnemyCount() != 0 && battleManager.GetComponent<command>().PlayerAlive())
                {
                    TLManager();
                }
            }
        }
    }

    public void Dead()
    {
        SetState(STATE.ST_DEAD);
        //animator.SetTrigger("Dead");
    }

    public void SetChara()
    {
        battleManager = GameObject.Find("BattleManager");
        sceneNavigator = GameObject.Find("SceneNavigator");

        //  データベースからステータスを取得
        if (Name.StartsWith("p"))
        {
            playerProof = true;
        }

        if (playerProof)
        {
            if (Name.Contains("1"))
            {
                charID = battleManager.GetComponent<BattleScene>().GetPID(1);
                TLIcon.GetComponent<Image>().enabled = true;
                TLProgress = 40;
            }
            else if (Name.Contains("2"))
            {
                charID = battleManager.GetComponent<BattleScene>().GetPID(2);
                TLIcon.GetComponent<Image>().enabled = true;
                TLProgress = 20;
            }
            else
            {
                charID = battleManager.GetComponent<BattleScene>().GetPID(3);
                TLIcon.GetComponent<Image>().enabled = true;
                TLProgress = 0;
            }

            //  charIDが0(存在しない)ならデリートする
            if (charID == 0)
            {
                Destroy(TLIcon);
                Destroy(MyTarget);
                Destroy(this.gameObject);
            }
            else
            {
                sceneNavigator.GetComponent<StatusControl>().SetStatus(charID - 1, ref CharName, ref LV, ref HP, ref SP, ref ATK, ref DEF, ref SPD, ref MAT, ref MDF, ref LUK);
                charName.GetComponent<StatusWindow>().SetName();

                MAX_HP = HP;
                MAX_SP = SP;
                TURN = 0;

                switch (charID)
                {
                    case 1:
                        GetComponent<Image>().sprite = psp1;
                        TLIcon.GetComponent<Image>().sprite = pIcon1;
                        anim = gameObject.AddComponent<Animator>();
                        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("戦闘時アニメ/シャル/Anim");
                        break;
                    case 2:
                        GetComponent<Image>().sprite = psp2;
                        TLIcon.GetComponent<Image>().sprite = pIcon2;
                        anim = gameObject.AddComponent<Animator>();
                        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("戦闘時アニメ/カーラ/Anim");
                        break;
                    case 3:
                        GetComponent<Image>().sprite = psp3;
                        TLIcon.GetComponent<Image>().sprite = pIcon3;
                        anim = gameObject.AddComponent<Animator>();
                        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("戦闘時アニメ/豪鬼/Anim");
                        break;
                    case 4:
                        GetComponent<Image>().sprite = psp4;
                        TLIcon.GetComponent<Image>().sprite = pIcon4;
                        anim = gameObject.AddComponent<Animator>();
                        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("戦闘時アニメ/リリー/Anim");
                        break;
                    case 5:
                        GetComponent<Image>().sprite = psp5;
                        TLIcon.GetComponent<Image>().sprite = pIcon5;
                        anim = gameObject.AddComponent<Animator>();
                        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("戦闘時アニメ/エリザ/Anim");
                        break;
                    case 6:
                        GetComponent<Image>().sprite = psp6;
                        TLIcon.GetComponent<Image>().sprite = pIcon6;
                        anim = gameObject.AddComponent<Animator>();
                        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("戦闘時アニメ/パンドラ/Anim");
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            if (gameObject.name.Contains("1"))
            {
                charID = battleManager.GetComponent<BattleScene>().GetEID(1);
                TLIcon.GetComponent<Image>().enabled = true;
                EHPDisplay = GameObject.Find("enemyHP1");
                EHPDisplay.GetComponent<Text>().enabled = true;
                TLProgress = 40;
            }
            else if (gameObject.name.Contains("2"))
            {
                charID = battleManager.GetComponent<BattleScene>().GetEID(2);
                TLIcon.GetComponent<Image>().enabled = true;
                EHPDisplay = GameObject.Find("enemyHP2");
                EHPDisplay.GetComponent<Text>().enabled = true;
                TLProgress = 20;
            }
            else
            {
                charID = battleManager.GetComponent<BattleScene>().GetEID(3);
                TLIcon.GetComponent<Image>().enabled = true;
                EHPDisplay = GameObject.Find("enemyHP3");
                EHPDisplay.GetComponent<Text>().enabled = true;
                TLProgress = 0;
            }

            //  charIDが0(存在しない)ならデリートする
            if (charID == 0)
            {
                Destroy(TLIcon);
                Destroy(MyTarget);
                Destroy(EHPDisplay);
                Destroy(this.gameObject);
            }
            else
            {
                SetEnemyStatus(charID - 1, ref CharName, ref LV, ref HP, ref SP, ref ATK, ref DEF, ref SPD, ref MAT, ref MDF, ref LUK);
                MAX_HP = HP;
                MAX_SP = SP;
                TURN = 0;

                battleManager.GetComponent<EnemyAction>().EnemyCharaChange(this.gameObject, charID);
            }
        }

        state = STATE.ST_STAND;
    }

    private void TLManager()
    {
        Vector3 pos = TLIcon.transform.position;
        pos = new Vector3(tmp.x + TLProgress / 40, tmp.y, tmp.z);
        TLIcon.transform.position = pos;

        //  ActiveChooseがtrueの間は進行度は増えない
        if (battleManager.GetComponent<BattleScene>().GetComponent<BattleScene>().GetActiveChoose() == false)
        {
            if (battleManager.GetComponent<BattleScene>().GetComponent<BattleScene>().GetActionFlag() == false)
            {
                //  移動
                float ProgressSPD = SPD / 20;
                if (ProgressSPD <= 0)
                {
                    //ProgressSPD = 0;
                    ProgressSPD = 1;
                }
                TLProgress += ProgressSPD;
            }

            //  行動選択開始
            if (TLProgress >= 200 && !progressEnd)
            {
                TLProgress = 200;

                if (playerProof)
                {
                    battleManager.GetComponent<command>().SetCharID(charID - 1);
                    battleManager.GetComponent<command>().SetLV(LV);
                    battleManager.GetComponent<BattleScene>().SetActiveChoose(true);
                    battleManager.GetComponent<command>().commandDisplay();
                    //  ポインターの表示
                    turnPointer.GetComponent<Image>().enabled = true;
                }
                else
                {
                    battleManager.GetComponent<EnemyAction>().SetCharID(charID - 1);
                    battleManager.GetComponent<EnemyAction>().CommandSelect(this.gameObject);
                }
                battleManager.GetComponent<BattleScene>().SetActivePlayer(this.gameObject);
                progressEnd = true;
            }

            //  行動開始
            if (TLProgress >= 300)
            {
                waitTime++;
                if (!actionFlag)
                {
                    battleManager.GetComponent<BattleScene>().SetAttackObj(this.gameObject);
                    LoadSkill();
                    battleManager.GetComponent<command>().ActionStart();

                    battleManager.GetComponent<BattleScene>().TakeAction(spCost, HPCtlFlag, AttackType, HealPercent,animName,targetFlag);

                    //  色の変更
                    battleManager.GetComponent<command>().changeColor(color2);
                    image.color = color1;
                    if (ReceiveChara1 != null)
                    {
                        ReceiveChara1.GetComponent<Image>().color = color1;
                    }
                    if (ReceiveChara2 != null)
                    {
                        ReceiveChara2.GetComponent<Image>().color = color1;
                    }
                    if (ReceiveChara3 != null)
                    {
                        ReceiveChara3.GetComponent<Image>().color = color1;
                    }

                    battleManager.GetComponent<BattleScene>().SetActionFlag(true);

                    actionFlag = true;
                }


                if (waitTime >= 80)
                {
                    battleManager.GetComponent<command>().ActionEnd();
                    if (ReceiveChara1 != null && ReceiveChara1.GetComponent<Status>().GetHP() <= 0)
                    {
                        ReceiveChara1.GetComponent<Status>().Dead();
                    }
                    if (ReceiveChara2 != null && ReceiveChara2.GetComponent<Status>().GetHP() <= 0)
                    {
                        ReceiveChara2.GetComponent<Status>().Dead();
                    }
                    if (ReceiveChara3 != null && ReceiveChara3.GetComponent<Status>().GetHP() <= 0)
                    {
                        ReceiveChara3.GetComponent<Status>().Dead();
                    }

                    TLProgress = 0;
                    actionFlag = false;
                    progressEnd = false;
                    battleManager.GetComponent<BattleScene>().SetActionFlag(false);
                    waitTime = 0;
                    CheckBuff();
                    battleManager.GetComponent<command>().changeColor(color1);

                    battleManager.GetComponent<BattleScene>().SetReceiveObj(null, null, null);
                }
            }

            if (battleManager.GetComponent<command>().GetCommandEnd())
            {
                if (playerProof)
                {
                    //  ポインターの表示
                    turnPointer.GetComponent<Image>().enabled = false;
                }
            }
        }
    }

    //  ターゲットの切り替え用
    public void SetTarget()
    {
        battleManager.GetComponent<command>().SetTarget(gameObject.name);
        battleManager.GetComponent<command>().SkillDescription();
    }

    public void SetRayCast(bool flag)
    {
        image = GetComponent<Image>();
        image.raycastTarget = flag;
    }

    private void SetEnemyStatus(int id,ref string charName,ref int lv, ref int hp, ref int sp, ref int atk, ref int def, ref int spd, ref int mat, ref int mdf, ref int luk)
    {
        EC = Resources.Load("ExcelData/enemy_chara") as enemy_charaList;
        lv = 1;
        ////  レベルによってステータスを加算する
        charName = EC.sheets[0].list[id].Name;
        hp = (int)EC.sheets[0].list[id].HP;
        sp = (int)EC.sheets[0].list[id].SP;
        atk = (int)EC.sheets[0].list[id].ATK;
        def = (int)EC.sheets[0].list[id].DEF;
        spd = (int)EC.sheets[0].list[id].SPD;
        mat = (int)EC.sheets[0].list[id].MAT;
        mdf = (int)EC.sheets[0].list[id].MDF;
        luk = (int)EC.sheets[0].list[id].LUK;
    }

    //  行動選択が完了したら、行動するまで行動の内容を保存しておく
    public void SaveSkill(int SkillID)
    {
        if (playerProof)
        {
            if (Name.Contains("1"))
            {
                SaveSkillIDList.Insert(0, SkillID);
            }
            else if (Name.Contains("2"))
            {
                SaveSkillIDList.Insert(1, SkillID);
            }
            else
            {
                SaveSkillIDList.Insert(2, SkillID);
            }
        }
        else
        {
            if (Name.Contains("1"))
            {
                SaveSkillIDList.Insert(3, SkillID);
            }
            else if (Name.Contains("2"))
            {
                SaveSkillIDList.Insert(4, SkillID);
            }
            else
            {
                SaveSkillIDList.Insert(5, SkillID);
            }
        }
    }

    //  攻撃を受ける側を保存
    public void SaveReceive(GameObject receive1)
    {
        int attackID;
        attackID = GetAttackID();

        string ReceiveName = receive1.name;
        GameObject target1;
        
        if (ReceiveName.StartsWith("p"))
        {
            if (ReceiveName.Contains("1"))
            {
                target1 = GameObject.Find("player1");
            }
            else if (ReceiveName.Contains("2"))
            {
                target1 = GameObject.Find("player2");
            }
            else
            {
                target1 = GameObject.Find("player3");
            }
        }
        else
        {
            if (ReceiveName.Contains("1"))
            {
                target1 = GameObject.Find("enemy1");
            }
            else if (ReceiveName.Contains("2"))
            {
                target1 = GameObject.Find("enemy2");
            }
            else
            {
                target1 = GameObject.Find("enemy3");
            }
            
        }
        SaveReceiveList.Insert(attackID, new receiveChar { RECEIVE1 = target1,RECEIVE2 = null,RECEIVE3 = null });
    }

    public void SaveReceive(GameObject receive1, GameObject receive2)
    {
        int attackID;
        attackID = GetAttackID();
        string ReceiveName;
        GameObject target1 = null;
        GameObject target2 = null;
        GameObject Finder;

        for (int count = 0;count < 2;count++)
        {
            if(count == 0)
            {
                ReceiveName = receive1.name;
            }
            else
            {
                ReceiveName = receive2.name;
            }

            if (ReceiveName.StartsWith("p"))
            {
                if (ReceiveName.Contains("1"))
                {
                    Finder = GameObject.Find("player1");
                }
                else if (ReceiveName.Contains("2"))
                {
                    Finder = GameObject.Find("player2");
                }
                else
                {
                    Finder = GameObject.Find("player3");
                }
            }
            else
            {
                if (ReceiveName.Contains("1"))
                {
                    Finder = GameObject.Find("enemy1");
                }
                else if (ReceiveName.Contains("2"))
                {
                    Finder = GameObject.Find("enemy2");
                }
                else
                {
                    Finder = GameObject.Find("enemy3");
                }

            }

            if(count == 0)
            {
                target1 = Finder;
            }
            else
            {
                target2 = Finder;
            }
        }
        SaveReceiveList.Insert(attackID, new receiveChar { RECEIVE1 = target1, RECEIVE2 = target2, RECEIVE3 = null });
    }

    public void SaveReceive(GameObject receive1, GameObject receive2, GameObject receive3)
    {
        int attackID;
        attackID = GetAttackID();
        string ReceiveName;
        GameObject target1 = null;
        GameObject target2 = null;
        GameObject target3 = null;
        GameObject Finder;

        for (int count = 0; count < 3; count++)
        {
            if (count == 0)
            {
                ReceiveName = receive1.name;
            }
            else if(count == 1)
            {
                ReceiveName = receive2.name;
            }
            else
            {
                ReceiveName = receive3.name;
            }

            if (ReceiveName.StartsWith("p"))
            {
                if (ReceiveName.Contains("1"))
                {
                    Finder = GameObject.Find("player1");
                }
                else if (ReceiveName.Contains("2"))
                {
                    Finder = GameObject.Find("player2");
                }
                else
                {
                    Finder = GameObject.Find("player3");
                }
            }
            else
            {
                if (ReceiveName.Contains("1"))
                {
                    Finder = GameObject.Find("enemy1");
                }
                else if (ReceiveName.Contains("2"))
                {
                    Finder = GameObject.Find("enemy2");
                }
                else
                {
                    Finder = GameObject.Find("enemy3");
                }

            }

            if (count == 0)
            {
                target1 = Finder;
            }
            else if(count == 1)
            {
                target2 = Finder;
            }
            else
            {
                target3 = Finder;
            }
        }
        SaveReceiveList.Insert(attackID, new receiveChar { RECEIVE1 = target1, RECEIVE2 = target2, RECEIVE3 = target3 });
    }

    //  SaveReceive用attackID取得
    private int GetAttackID()
    {
        if (playerProof)
        {
            if (Name.Contains("1"))
            {
                return 0;
            }
            else if (Name.Contains("2"))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        else
        {
            if (Name.Contains("1"))
            {
                return 3;
            }
            else if (Name.Contains("2"))
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
    }


    //  保存しておいた行動を呼び出す
    public void LoadSkill()
    {
        int skillID;
        if (playerProof)
        {
            if (Name.Contains("1"))
            {
                skillID = SaveSkillIDList[0];
                ReceiveChara1 = SaveReceiveList[0].RECEIVE1;
                ReceiveChara2 = SaveReceiveList[0].RECEIVE2;
                ReceiveChara3 = SaveReceiveList[0].RECEIVE3;
            }
            else if (Name.Contains("2"))
            {
                skillID = SaveSkillIDList[1];
                ReceiveChara1 = SaveReceiveList[1].RECEIVE1;
                ReceiveChara2 = SaveReceiveList[1].RECEIVE2;
                ReceiveChara3 = SaveReceiveList[1].RECEIVE3;
            }
            else
            {
                skillID = SaveSkillIDList[2];
                ReceiveChara1 = SaveReceiveList[2].RECEIVE1;
                ReceiveChara2 = SaveReceiveList[2].RECEIVE2;
                ReceiveChara3 = SaveReceiveList[2].RECEIVE3;
            }
        }
        else
        {
            if (Name.Contains("1"))
            {
                skillID = SaveSkillIDList[3];
                ReceiveChara1 = SaveReceiveList[3].RECEIVE1;
                ReceiveChara2 = SaveReceiveList[3].RECEIVE2;
                ReceiveChara3 = SaveReceiveList[3].RECEIVE3;
            }
            else if (Name.Contains("2"))
            {
                skillID = SaveSkillIDList[4];
                ReceiveChara1 = SaveReceiveList[4].RECEIVE1;
                ReceiveChara2 = SaveReceiveList[4].RECEIVE2;
                ReceiveChara3 = SaveReceiveList[4].RECEIVE3;
            }
            else
            {
                skillID = SaveSkillIDList[5];
                ReceiveChara1 = SaveReceiveList[5].RECEIVE1;
                ReceiveChara2 = SaveReceiveList[5].RECEIVE2;
                ReceiveChara3 = SaveReceiveList[5].RECEIVE3;
            }
        }


        int count = 0;
        GameObject rec1 = null;
        GameObject rec2 = null;

        if(ReceiveChara1 != null && ReceiveChara1.GetComponent<Status>().GetState() != STATE.ST_DEAD)
        {
            rec1 = ReceiveChara1;
            count++;
        }
        if (ReceiveChara2 != null && ReceiveChara1.GetComponent<Status>().GetState() != STATE.ST_DEAD)
        {
            if(!rec1)
            {
                rec1 = ReceiveChara2;
            }
            else
            {
                rec2 = ReceiveChara2;
            }
            count++;
        }
        if (ReceiveChara3 != null && ReceiveChara1.GetComponent<Status>().GetState() != STATE.ST_DEAD)
        {
            if(!rec1)
            {
                rec1 = ReceiveChara3;
            }
            else if(!rec2)
            {
                rec2 = ReceiveChara3;
            }
            else
            {
            }
            count++;
        }


        if(count == 1)
        {
            battleManager.GetComponent<BattleScene>().SetReceiveObj(rec1);
        }
        else if(count == 2)
        {
            battleManager.GetComponent<BattleScene>().SetReceiveObj(rec1,rec2);
        }
        else if(count == 3)
        {
            battleManager.GetComponent<BattleScene>().SetReceiveObj(ReceiveChara1, ReceiveChara2, ReceiveChara3);
        }
        else
        {
        }

        SkillInfluence(skillID);

        spCost = (int)PS.sheets[0].list[skillID].sp;
    }

    //  スキルの効果の取得
    public void SkillInfluence(int skillID)
    {
        PC = Resources.Load("ExcelData/player_chara") as player_charaList;
        PS = Resources.Load("ExcelData/playerSkill") as player_skillList;

        EC = Resources.Load("ExcelData/enemy_chara") as enemy_charaList;
        ES = Resources.Load("ExcelData/enemySkill") as enemy_skillList;

        GameObject AttackObj = battleManager.GetComponent<BattleScene>().GetAttackObj();

        //  前の行動で敵が死亡してたら標的を変える
        if (battleManager.GetComponent<BattleScene>().GetReceiveObj(1) == null
            && battleManager.GetComponent<BattleScene>().GetReceiveObj(2) == null
            && battleManager.GetComponent<BattleScene>().GetReceiveObj(3) == null)
        {
            //　プレイヤーが敵に向かって行動する時用
            if (playerProof)
            {
                if (PS.sheets[0].list[skillID].target.StartsWith("E"))
                {
                    battleManager.GetComponent<command>().changeTarget(playerProof, PS.sheets[0].list[skillID].target);
                    ReceiveChara1 = battleManager.GetComponent<BattleScene>().GetReceiveObj(1);
                }
            }
            //  敵が敵に向かって行動する時用
            else
            {
                if (ES.sheets[0].list[skillID].target.StartsWith("E"))
                {
                    battleManager.GetComponent<command>().changeTarget(playerProof, ES.sheets[0].list[skillID].target);
                    ReceiveChara1 = battleManager.GetComponent<BattleScene>().GetReceiveObj(1);
                }
            }
        }

        //  前の行動でプレイヤーが死亡してたら標的を変える
        if ((battleManager.GetComponent<BattleScene>().GetReceiveObj(1) && battleManager.GetComponent<BattleScene>().GetReceiveObj(1).GetComponent<Status>().GetState() == STATE.ST_DEAD)
            || (battleManager.GetComponent<BattleScene>().GetReceiveObj(2) && battleManager.GetComponent<BattleScene>().GetReceiveObj(2).GetComponent<Status>().GetState() == STATE.ST_DEAD)
            || (battleManager.GetComponent<BattleScene>().GetReceiveObj(3) && battleManager.GetComponent<BattleScene>().GetReceiveObj(3).GetComponent<Status>().GetState() == STATE.ST_DEAD))
        {
            //　プレイヤーがプレイヤーに向かって行動する時用
            if (playerProof)
            {
                if (PS.sheets[0].list[skillID].target.StartsWith("P"))
                {
                    if (PS.sheets[0].list[skillID].target.Contains("1"))
                    {
                        if (battleManager.GetComponent<command>().GetPlayerChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                        {
                            battleManager.GetComponent<BattleScene>().SetReceiveObj(battleManager.GetComponent<command>().GetPlayerChild(0));
                        }
                        else if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                        {
                            battleManager.GetComponent<BattleScene>().SetReceiveObj(battleManager.GetComponent<command>().GetPlayerChild(1));
                        }
                        else if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                        {
                            battleManager.GetComponent<BattleScene>().SetReceiveObj(battleManager.GetComponent<command>().GetPlayerChild(2));
                        }
                        ReceiveChara1 = battleManager.GetComponent<BattleScene>().GetReceiveObj(1);
                    }

                    //  3人に対する行動
                    else
                    {
                        NonTarget3();
                    }
                }
            }
            //  敵がプレイヤーに向かって行動する時用
            else
            {
                if (ES.sheets[0].list[skillID].target.StartsWith("P"))
                {
                    if (ES.sheets[0].list[skillID].target.Contains("1"))
                    {
                        if (battleManager.GetComponent<command>().GetPlayerChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                        {
                            battleManager.GetComponent<BattleScene>().SetReceiveObj(battleManager.GetComponent<command>().GetPlayerChild(0));
                        }
                        else if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                        {
                            battleManager.GetComponent<BattleScene>().SetReceiveObj(battleManager.GetComponent<command>().GetPlayerChild(1));
                        }
                        else if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
                        {
                            battleManager.GetComponent<BattleScene>().SetReceiveObj(battleManager.GetComponent<command>().GetPlayerChild(2));
                        }
                        ReceiveChara1 = battleManager.GetComponent<BattleScene>().GetReceiveObj(1);
                    }
                    else
                    {
                        NonTarget3();
                    }
                }
            }
        }

        GameObject UseObj = AttackObj;

        if(playerProof)
        {
            battleManager.GetComponent<command>().SetActiveSkillText(PS.sheets[0].list[skillID].skillName);
            animName = PS.sheets[0].list[skillID].effectName;
            if(int.Parse(Regex.Replace(PS.sheets[0].list[skillID].target, @"[^0-9]", "")) == 3)
            {
                targetFlag = true;
            }
        }
        else
        {
            battleManager.GetComponent<command>().SetActiveSkillText(ES.sheets[0].list[skillID].skillName);
            animName = ES.sheets[0].list[skillID].effectName;
            if (int.Parse(Regex.Replace(ES.sheets[0].list[skillID].target, @"[^0-9]", "")) == 3)
            {
                targetFlag = true;
            }
        }

        string charaStatus;
        string useStatus;
        string useStatus2;
        double magnification;
        int STime;

        //  ステータスを弄るキャラ
        charaStatus = PS.sheets[0].list[skillID].useChara;
        //  使用するステータス
        useStatus = PS.sheets[0].list[skillID].influence1;
        useStatus2 = PS.sheets[0].list[skillID].influence2;
        //  技の倍率
        magnification = PS.sheets[0].list[skillID].power;
        //  HPに影響を与える技かどうか
        HPCtlFlag = (int)PS.sheets[0].list[skillID].hpCtl;
        //  物理攻撃か魔法攻撃か
        AttackType = (int)PS.sheets[0].list[skillID].attackType;
        //  持続時間
        STime = (int)PS.sheets[0].list[skillID].period;

        //  行動するのが敵の場合は敵のステータスに変更
        if (!playerProof)
        {
            charaStatus = ES.sheets[0].list[skillID].useChara;
            useStatus = ES.sheets[0].list[skillID].influence1;
            useStatus2 = ES.sheets[0].list[skillID].influence2;
            magnification = ES.sheets[0].list[skillID].power;
            HPCtlFlag = (int)ES.sheets[0].list[skillID].hpCtl;
            AttackType = (int)ES.sheets[0].list[skillID].attackType;
            STime = (int)ES.sheets[0].list[skillID].period;
        }

        DeleteBuff(AttackObj);
        DeleteBuff(ReceiveChara1);
        DeleteBuff(ReceiveChara2);
        DeleteBuff(ReceiveChara3);

        HealPercent = 0;

        if (charaStatus == "Attack")
        {
            UseObj = AttackObj;
        }
        else if(charaStatus == "Receive")
        {
            UseObj = ReceiveChara1;
        }
        else
        {
        }

        for (int count = 0; count < 3; count++)
        {
            strData str1 = new strData();
            switch (useStatus)
            {
                case ("HP"):
                    HealPercent = magnification;
                    break;
                case ("ATK"):
                    str1.ATK = (int)Math.Round(UseObj.GetComponent<Status>().GetATK() * magnification);
                    str1.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str1);
                    break;
                case ("DEF"):
                    str1.DEF = (int)Math.Round(UseObj.GetComponent<Status>().GetDEF() * magnification);
                    str1.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str1);
                    break;
                case ("SPD"):
                    str1.SPD = (int)Math.Round(UseObj.GetComponent<Status>().GetSPD() * magnification);
                    str1.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str1);
                    break;
                case ("MAT"):
                    str1.MAT = (int)Math.Round(UseObj.GetComponent<Status>().GetMAT() * magnification);
                    str1.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str1);
                    break;
                case ("MDF"):
                    str1.MDF = (int)Math.Round(UseObj.GetComponent<Status>().GetMDF() * magnification);
                    str1.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str1);
                    break;
                case ("LUK"):
                    str1.LUK = (int)Math.Round(UseObj.GetComponent<Status>().GetLUK() * magnification);
                    str1.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str1);
                    break;
                default:
                    break;
            }

            strData str2 = new strData();
            switch (useStatus2)
            {
                case ("HP"):
                    HealPercent = magnification;
                    break;
                case ("ATK"):
                    str2.ATK = (int)Math.Round(UseObj.GetComponent<Status>().GetATK() * magnification);
                    str2.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str2);
                    break;
                case ("DEF"):
                    str2.DEF = (int)Math.Round(UseObj.GetComponent<Status>().GetDEF() * magnification);
                    str2.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str2);
                    break;
                case ("SPD"):
                    str2.SPD = (int)Math.Round(UseObj.GetComponent<Status>().GetSPD() * magnification);
                    str2.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str2);
                    break;
                case ("MAT"):
                    str2.MAT = (int)Math.Round(UseObj.GetComponent<Status>().GetMAT() * magnification);
                    str2.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str2);
                    break;
                case ("MDF"):
                    str2.MDF = (int)Math.Round(UseObj.GetComponent<Status>().GetMDF() * magnification);
                    str2.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str2);
                    break;
                case ("LUK"):
                    str2.LUK = (int)Math.Round(UseObj.GetComponent<Status>().GetLUK() * magnification);
                    str2.TIME = STime + TURN;
                    UseObj.GetComponent<Status>().changeStatus.Add(str2);
                    break;
                default:
                    break;
            }

            if(charaStatus == "Attack")
            {
                break;
            }

            if(count == 0)
            {
                UseObj = ReceiveChara2;
            }
            else if(count == 1)
            {
                UseObj = ReceiveChara3;
            }
            else
            {
            }

            if(!UseObj)
            {
                break;
            }
        }
    }

    //  バフ、デバフを反映させる
    public void SetBuff()
    {
        if (changeStatus != null)
        {
            for (int count = 0; count < changeStatus.Count; count++)
            {
                    SetATK(GetATK() + changeStatus[count].ATK);
                    SetDEF(GetDEF() + changeStatus[count].DEF);
                    SetSPD(GetSPD() + changeStatus[count].SPD);
                    SetMAT(GetMAT() + changeStatus[count].MAT);
                    SetMDF(GetMDF() + changeStatus[count].MDF);
                    SetLUK(GetLUK() + changeStatus[count].LUK);
            }
        }
    }

    //  バフ、デバフを解除する
    public void DeleteBuff(GameObject objchara)
    {
        if (objchara != null)
        {
            if (objchara.GetComponent<Status>().changeStatus != null)
            {
                for (int count = 0; count < objchara.GetComponent<Status>().changeStatus.Count; count++)
                {
                    objchara.GetComponent<Status>().SetATK(objchara.GetComponent<Status>().GetATK() - objchara.GetComponent<Status>().changeStatus[count].ATK);
                    objchara.GetComponent<Status>().SetDEF(objchara.GetComponent<Status>().GetDEF() - objchara.GetComponent<Status>().changeStatus[count].DEF);
                    objchara.GetComponent<Status>().SetSPD(objchara.GetComponent<Status>().GetSPD() - objchara.GetComponent<Status>().changeStatus[count].SPD);
                    objchara.GetComponent<Status>().SetMAT(objchara.GetComponent<Status>().GetMAT() - objchara.GetComponent<Status>().changeStatus[count].MAT);
                    objchara.GetComponent<Status>().SetMDF(objchara.GetComponent<Status>().GetMDF() - objchara.GetComponent<Status>().changeStatus[count].MDF);
                    objchara.GetComponent<Status>().SetLUK(objchara.GetComponent<Status>().GetLUK() - objchara.GetComponent<Status>().changeStatus[count].LUK);
                }
            }
        }
    }


    //  ターン終了時にバフ、デバフの効果時間が終わっていないか確認する
    public void CheckBuff()
    {
        this.TURN++;

        if (changeStatus != null)
        {
            //  バフ、デバフがかかっている数だけループ
            for (int count = 0; count < changeStatus.Count; count++)
            {
                //  もし効果時間が終わっていたらバフ、デバフを解除する
                if(changeStatus[count].TIME <= this.TURN)
                {
                    SetATK(GetATK() - changeStatus[count].ATK);
                    SetDEF(GetDEF() - changeStatus[count].DEF);
                    SetSPD(GetSPD() - changeStatus[count].SPD);
                    SetMAT(GetMAT() - changeStatus[count].MAT);
                    SetMDF(GetMDF() - changeStatus[count].MDF);
                    SetLUK(GetLUK() - changeStatus[count].LUK);

                    changeStatus.RemoveAt(count);
                }

                if(changeStatus == null)
                {
                    break;
                }
            }
        }
    }

    public string GetCharName()
    {
        return CharName;
    }

    public int GetLV()
    {
        return LV;
    }

    public void SetHP(int hp)
    {
        this.HP = hp;
        if(HP >= MAX_HP)
        {
            HP = MAX_HP;
        }
        if(HP <= 0)
        {
            HP = 0;
        }
    }

    public int GetHP()
    {
        return HP;
    }

    public int GetMAXHP()
    {
        return MAX_HP;
    }

    public void SetSP(int sp)
    {
        this.SP = sp;
        if (SP >= MAX_SP)
        {
            SP = MAX_SP;
        }
        if (SP <= 0)
        {
            SP = 0;
        }
    }

    public int GetSP()
    {
        return SP;
    }

    public int GetMAXSP()
    {
        return MAX_SP;
    }

    public void SetATK(int atk)
    {
        this.ATK = atk;
        if (ATK <= 0)
        {
            ATK = 1;
        }
    }

    public int GetATK()
    {
        return ATK;
    }

    public void SetDEF(int def)
    {
        this.DEF = def;
        if (DEF <= 0)
        {
            DEF = 1;
        }
    }

    public int GetDEF()
    {
        return DEF;
    }

    public void SetSPD(int spd)
    {
        this.SPD = spd;
        if (SPD <= 0)
        {
            SPD = 1;
        }
    }

    public int GetSPD()
    {
        return SPD;
    }

    public void SetMAT(int mat)
    {
        this.MAT = mat;
        if (MAT <= 0)
        {
            MAT = 1;
        }
    }

    public int GetMAT()
    {
        return MAT;
    }

    public void SetMDF(int mdf)
    {
        this.MDF = mdf;
        if (MDF <= 0)
        {
            MDF = 1;
        }
    }

    public int GetMDF()
    {
        return MDF;
    }

    public void SetLUK(int luk)
    {
        this.LUK = luk;
        if (LUK <= 0)
        {
           LUK = 1;
        }
    }

    public int GetLUK()
    {
        return LUK;
    }

    public int GetTurn()
    {
        return TURN;
    }

    //  state取得
    public STATE GetState()
    {
        return state;
    }

    public void SetState(STATE st)
    {
        this.state = st;
    }


    //  プレイヤー用のアニメーション処理
    private void SetAnim()
    {
        if(state == STATE.ST_STAND)
        {
            anim.SetBool("isIdol", true);
        }
        else
        {
            anim.SetBool("isIdol", false);
        }

        if (state == STATE.ST_ATK)
        {
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);
        }

        if (state == STATE.ST_DEF)
        {
            anim.SetBool("isDefence", true);
        }
        else
        {
            anim.SetBool("isDefence", false);
        }

        if (state == STATE.ST_DAMAGE)
        {
            anim.SetBool("isDamage", true);
        }
        else
        {
            anim.SetBool("isDamage", false);
        }

        if (state == STATE.ST_DEAD)
        {
            anim.SetBool("isDeath", true);
        }
        else
        {
            anim.SetBool("isDeath", false);
        }
    }

    private void NonTarget3()
    {
        GameObject rec1 = null;
        GameObject rec2 = null;

        //  相手が死んでなければ標的にするのを許可する！
        if (battleManager.GetComponent<command>().GetPlayerChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
        {
            rec1 = battleManager.GetComponent<command>().GetPlayerChild(0);
        }
        else if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
        {
            rec1 = battleManager.GetComponent<command>().GetPlayerChild(1);
        }
        else if (battleManager.GetComponent<command>().GetPlayerChild(2).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD)
        {
            rec1 = battleManager.GetComponent<command>().GetPlayerChild(2);
        }
        else
        {
        }

        if (battleManager.GetComponent<command>().GetPlayerChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD
            && battleManager.GetComponent<command>().GetPlayerChild(0) == rec1)
        {
            rec2 = battleManager.GetComponent<command>().GetPlayerChild(0);
        }
        else if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD
             && battleManager.GetComponent<command>().GetPlayerChild(1) == rec1)
        {
            rec2 = battleManager.GetComponent<command>().GetPlayerChild(1);
        }
        else if (battleManager.GetComponent<command>().GetPlayerChild(2).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD
             && battleManager.GetComponent<command>().GetPlayerChild(2) == rec1)
        {
            rec2 = battleManager.GetComponent<command>().GetPlayerChild(2);
        }
        else
        {
        }


        if (GetComponent<command>().GetPlayerCount() == 2)
        {
            if (battleManager.GetComponent<command>().GetPlayerChild(0).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD
                && battleManager.GetComponent<command>().GetPlayerChild(0) != rec1)
            {
                rec2 = battleManager.GetComponent<command>().GetPlayerChild(0);
            }
            else if (battleManager.GetComponent<command>().GetPlayerChild(1).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD
                 && battleManager.GetComponent<command>().GetPlayerChild(1) != rec1)
            {
                rec2 = battleManager.GetComponent<command>().GetPlayerChild(1);
            }
            else if (battleManager.GetComponent<command>().GetPlayerChild(2).GetComponent<Status>().GetState() != Status.STATE.ST_DEAD
                 && battleManager.GetComponent<command>().GetPlayerChild(2) != rec1)
            {
                rec2 = battleManager.GetComponent<command>().GetPlayerChild(2);
            }
            else
            {
            }

            battleManager.GetComponent<BattleScene>().SetReceiveObj(rec1, rec2);
            ReceiveChara1 = battleManager.GetComponent<BattleScene>().GetReceiveObj(1);
            ReceiveChara2 = battleManager.GetComponent<BattleScene>().GetReceiveObj(2);
            ReceiveChara3 = null;
        }
        else
        {
            battleManager.GetComponent<BattleScene>().SetReceiveObj(rec1);
            ReceiveChara1 = battleManager.GetComponent<BattleScene>().GetReceiveObj(1);
            ReceiveChara2 = null;
            ReceiveChara3 = null;
        }
    }
}