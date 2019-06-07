using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class titleLogo : MonoBehaviour {

    Image logo;
    int moveCnt;
	// Use this for initialization
	void Start () {
        moveCnt = 0;
        logo = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        moveCnt++;
        
        if(moveCnt / 80 % 2 == 0)
        {
            logo.transform.position += new Vector3(0.0f, 0.008f, 0.0f);
        }
        else
        {
            logo.transform.position -= new Vector3(0.0f, 0.008f, 0.0f);
        }
	}
}
