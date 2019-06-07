using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HPText : MonoBehaviour
{
    public GameObject status;

    void Start()
    {

    }

    void Update()
    {
        SetHP();
    }

    public void SetHP()
    {
        if (status)
        {
            this.gameObject.GetComponent<Text>().text = "HP:" + Convert.ToString(status.GetComponent<Status>().GetHP());
        }
    }
}
