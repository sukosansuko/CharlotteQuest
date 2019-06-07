using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invisible : MonoBehaviour {
    public bool actFlag;
    public Image image;
    Color color1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color color2 = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
         actFlag = menu.flag;
       if (actFlag)
        {
            image.color = color1;
        }
        else
        {
            image.color = color2;
        }
    }
}
