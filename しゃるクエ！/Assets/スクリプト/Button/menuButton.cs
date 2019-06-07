using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class homeButton : MonoBehaviour {

    public bool actFlag;
    public bool flag;
    Image image;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        actFlag = menu.flag;
        if (actFlag)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        image.raycastTarget = flag;
    }
    public void button()
    {

        if(actFlag)
        {
            SceneNavigator.Instance.Change("ホーム");
        }
    }
}
