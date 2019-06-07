using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("コレジャナイ感");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            click();
        }

    }
    void click()
    {
        SceneNavigator.Instance.Change("タイトル");
    }

}
