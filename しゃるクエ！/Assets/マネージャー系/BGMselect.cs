using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMselect : SingletonMonoBehaviour<BGMselect>
{
    public static BGMselect instance = null;


    string activScene;

    //  前回のシーン
    string prevScene;

    string worldNum;

    // Start is called before the first frame update

    private void Awake()
    {
        // インスタンスが存在するとき
        if (instance != null)
        {
            // これを破棄する
            Destroy(this.gameObject);
            return;
        }
        // インスタンスが存在しない時
        else if (instance == null)
        {
            // これをインスタンスとする
            instance = this;
        }
        // シーンをまたいでも消去されないようにする
        DontDestroyOnLoad(this.gameObject);

    }
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        activScene = SceneManager.GetActiveScene().name;
        worldNum = WorldMass.stageName;
        if (activScene != prevScene)
        {
            //if(activScene != prevScene)
            //{
            //    AudioManager.Instance.StopBGM();
            //}
            if (activScene == "タイトル")
            {
                AudioManager.Instance.PlayBGM("タイトル");
            }
            if (activScene == "ホーム")
            {
                if (prevScene != "編成")
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayBGM("ホーム");
                }
            }
            if (activScene == "編成")
            {
                if(prevScene != "ホーム")
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayBGM("ホーム");
                }
            }
            if (activScene == "ワールドマップ")
            {
                if ((prevScene != "W1") && (prevScene != "W2") && (prevScene != "W3") && (prevScene != "W4"))
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayBGM("木2");
                }
            }
            if (activScene == "BattleScene")
            {
                SetBattleBGM(GetComponent<StatusControl>().GetBGMID());
            }
            if (activScene == "W1")
            {
                if (prevScene != "ワールドマップ")
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayBGM("木2");
                }
            }
            if (activScene == "W2")
            {
                if (prevScene != "ワールドマップ")
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayBGM("木2");
                }
            }
            if (activScene == "W3")
            {
                if (prevScene != "ワールドマップ")
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayBGM("木2");
                }
            }
            if (activScene == "W4")
            {
                if (prevScene != "ワールドマップ")
                {
                    AudioManager.Instance.StopBGM();
                    AudioManager.Instance.PlayBGM("木2");
                }
            }
            //if (worldNum == "W1")
            //{
            //    if (activScene == "BattleScene")
            //    {
            //        AudioManager.Instance.PlayBGM("");
            //    }
            //}
            //if (worldNum == "W2")
            //{
            //    if (activScene == "BattleScene")
            //    {
            //        AudioManager.Instance.PlayBGM("");
            //    }
            //}
            //if (worldNum == "W3")
            //{
            //    if (activScene == "BattleScene")
            //    {
            //        AudioManager.Instance.PlayBGM("");
            //    }
            //}
            //if (worldNum == "W4")
            //{
            //    if (activScene == "BattleScene")
            //    {
            //        AudioManager.Instance.PlayBGM("");
            //    }
            //}
        }
        prevScene = activScene;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void SetBattleBGM(int id)
    {
        AudioManager.Instance.StopBGM();
        switch (id)
        {
            case 1:
                AudioManager.Instance.PlayBGM("戦闘1");
                break;
            case 2:
                AudioManager.Instance.PlayBGM("戦闘2");
                break;
            default:
                break;
        }
    }

    public void SetSE(string name)
    {
        AudioManager.Instance.PlaySE(name);
    }
}
