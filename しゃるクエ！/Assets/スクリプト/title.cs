using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class title : MonoBehaviour {

    Image image;
    int FlashingCnt;

    Color color1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color color2 = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    // Use this for initialization
    void Start () {
        WorldmapChar.SavePos = WorldmapChar.InitCharPos;
        FlashingCnt = 0;
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        FlashingCnt++;

        if (FlashingCnt / 30 % 2 == 0)
        {
            image.color = color1;
        }
        else
        {
            image.color = color2;
        }
		if(Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlaySE("タイトルから進む時");

            SceneNavigator.Instance.Change("ホーム");
            //  デバッグ用
            //SceneNavigator.Instance.Change("BattleScene");
        }
	}
}
