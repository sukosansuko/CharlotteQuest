using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour
{

    public bool actFlag;
    public static bool flag;

    // Use this for initialization
    void Start()
    {
        actFlag = false;
        flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        flag = actFlag;
    }

    public void menuBotton()
    {
        if(actFlag)
        {
            actFlag = false;
        }
        else
        {
            actFlag = true;
        }
    }
}