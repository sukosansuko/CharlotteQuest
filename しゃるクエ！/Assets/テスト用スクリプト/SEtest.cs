using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEtest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("高槻ワンサイドラバー");
    }

    // Update is called once per frame
    void Update () {
    }
}
