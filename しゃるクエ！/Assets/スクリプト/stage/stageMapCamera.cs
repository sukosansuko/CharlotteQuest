using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stageMapCamera : MonoBehaviour
{
    private Vector3 touchStartPos;      // タップ開始位置
    private Vector3 touchEndPos;        // タップ終了位置
    private Vector3 nowPos;             // 現在ののカメラ位置
    private Vector3 afterPos;           // 移動終了後のカメラ位置

    string activScene;                  // 現在のｼｰﾝ名
    public static int moveCnt = 0;      // カメラの移動制御
    int movePos = 10;                   // moveCntが1動くたびにカメラが移動する大きさ
    
    int moveMax;                        // 移動範囲最大値
    // Start is called before the first frame update
    void Start()
    {
        nowPos.x = moveCnt * movePos;
        nowPos.z = -10;
        activScene = SceneManager.GetActiveScene().name;

        if (activScene == "W1")
        {
            moveMax = 1;
        }
        if(activScene == "W2")
        {
            moveMax = 2;
        }
        if (activScene == "W3")
        {
            moveMax = 3;
        }
        if (activScene == "W4")
        {
            moveMax = 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Flick();
        CameraMove();
    }
    void Flick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }
        if (Input.GetMouseButtonUp(0))
        {
            touchEndPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            GetDir();
        }
    }
    void GetDir()
    {
        float FlickSize = 30;       // フリックを認知させる大きさ

        float dirX = touchEndPos.x - touchStartPos.x;
        if (moveCnt > 0)
        {
            if (FlickSize < dirX)
            {
                //  右向きの時の処理

                moveCnt--;
            }
        }
        if (moveCnt < moveMax)
        {
            if (-FlickSize > dirX)
            {
                //  左向きの時の処理
                moveCnt++;
            }
        }
    }
    void CameraMove()
    {
        this.transform.position = nowPos;

        afterPos.x = moveCnt * movePos;
        if (nowPos.x > afterPos.x)
        {
            nowPos.x--;
        }
        else if (nowPos.x < afterPos.x)
        {
            nowPos.x++;
        }
    }
}
