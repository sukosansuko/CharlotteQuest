using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class formationManager : MonoBehaviour
{
    public GameObject changeObj1;
    public GameObject changeObj2;

    //  通常時の色
    Color color1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //  薄暗い
    Color color2 = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    private GameObject sceneNavigator;

    // Start is called before the first frame update
    void Start()
    {
        sceneNavigator = GameObject.Find("SceneNavigator");
    }

    // Update is called once per frame
    void Update()
    {
        if (changeObj1 && changeObj2)
        {
            StartChange();
        }
    }

    public void SetChangeObj1(GameObject obj)
    {
        if (!changeObj1)
        {
            changeObj1 = obj;
            obj.GetComponent<Image>().color = color2;
        }
    }

    public void SetChangeObj2(GameObject obj)
    {
        if(obj != changeObj1)
        {
            changeObj2 = obj;
        }
    }

    public void StartChange()
    {
        int obj1Number = int.Parse(Regex.Replace(changeObj1.name, @"[^0-9]", ""));
        int obj2Number = int.Parse(Regex.Replace(changeObj2.name, @"[^0-9]", ""));

        sceneNavigator.GetComponent<StatusControl>().ChangePlayerList(obj1Number,obj2Number);

        changeObj1.GetComponent<Image>().color = color1;

        changeObj1 = null;
        changeObj2 = null;
    }
}
