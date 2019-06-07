using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatusWindow : MonoBehaviour
{
    //  通常時の色
    Color color1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //  薄暗い
    Color color2 = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    public GameObject status;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (status)
        {
            SetHP();
            SetSP();

            if (this.gameObject.name.Contains("playerStatus"))
            {
                if (status.GetComponent<Status>().GetState() == Status.STATE.ST_DEAD)
                {
                    image.color = color2;
                }
            }
        }
        else
        {
            if (this.gameObject.name.Contains("playerStatus"))
            {
                    image.color = color2;
            }

            //if (this.gameObject.name.Contains("charHP"))
            //{
            //    this.gameObject.GetComponent<Text>().text = "HP:" + Convert.ToString(status.GetComponent<Status>().GetHP() + "/" + Convert.ToString(status.GetComponent<Status>().GetMAXHP()));
            //}
        }
    }

    public void SetHP()
    {
        if (this.gameObject.name.Contains("charHP"))
        {
            this.gameObject.GetComponent<Text>().text = "HP:" + Convert.ToString(status.GetComponent<Status>().GetHP() + "/" + Convert.ToString(status.GetComponent<Status>().GetMAXHP()));
        }
    }

    public void SetSP()
    {
        if (this.gameObject.name.Contains("charSP"))
        {
            this.gameObject.GetComponent<Text>().text = "SP:" + Convert.ToString(status.GetComponent<Status>().GetSP() + "/" + Convert.ToString(status.GetComponent<Status>().GetMAXSP()));
        }
    }

    public void SetName()
    {
        if (this.gameObject.name.Contains("charName"))
        {
            this.gameObject.GetComponent<Text>().text = status.GetComponent<Status>().GetCharName();
        }
    }
}
