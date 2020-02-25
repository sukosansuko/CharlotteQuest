using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DamageText : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Text>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //  ダメージ時
    public void SetDamage(int damage)
    {
        this.gameObject.GetComponent<Text>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        this.gameObject.GetComponent<Text>().text = Convert.ToString(damage);
        this.gameObject.GetComponent<Text>().enabled = true;
    }

    //  回復時
    public void SetCure(int damage)
    {
        this.gameObject.GetComponent<Text>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        this.gameObject.GetComponent<Text>().text = Convert.ToString(damage);
        this.gameObject.GetComponent<Text>().enabled = true;
    }

    public void SetFalse()
    {
        this.gameObject.GetComponent<Text>().enabled = false;
    }
}
