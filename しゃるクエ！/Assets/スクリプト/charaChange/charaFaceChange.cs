using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class charaFaceChange : MonoBehaviour
{
    public Image image;
    public Sprite sp1;
    public Sprite sp2;
    public Sprite sp3;
    public Sprite sp4;
    public Sprite sp5;
    public Sprite sp6;

    private int pID1;
    private int pID2;
    private int pID3;
    private int pID4;
    private int pID5;
    private int pID6;

    public int imageCnt;

    public GameObject CharName;

    public GameObject CharLV;

    private GameObject sceneNavigator;

    private GameObject Manager;

    // Use this for initialization
    void Start()
    {
        sceneNavigator = GameObject.Find("SceneNavigator");
        sceneNavigator.GetComponent<StatusControl>().GetPlayerList(ref pID1, ref pID2, ref pID3, ref pID4, ref pID5, ref pID6);
        GetID();
        SetNameLV();

        Manager = GameObject.Find("Manager");

        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeImage();
        }

        sceneNavigator.GetComponent<StatusControl>().GetPlayerList(ref pID1, ref pID2, ref pID3, ref pID4, ref pID5, ref pID6);
        GetID();
        SetNameLV();

        switch (imageCnt)
        {
            case 1:
                image.sprite = sp1;
                break;
            case 2:
                image.sprite = sp2;
                break;
            case 3:
                image.sprite = sp3;
                break;
            case 4:
                image.sprite = sp4;
                break;
            case 5:
                image.sprite = sp5;
                break;
            case 6:
                image.sprite = sp6;
                break;
        }

        if (imageCnt > 6)
        {
            imageCnt = 1;
        }
    }

    public void ChangeImage()
    {
        imageCnt++;
    }

    private void GetID()
    {
        int id = 1;
        if(gameObject.name.Contains("1"))
        {
            id = pID1;
        }
        else if (gameObject.name.Contains("2"))
        {
            id = pID2;
        }
        else if (gameObject.name.Contains("3"))
        {
            id = pID3;
        }
        else if (gameObject.name.Contains("4"))
        {
            id = pID4;
        }
        else if (gameObject.name.Contains("5"))
        {
            id = pID5;
        }
        else if (gameObject.name.Contains("6"))
        {
            id = pID6;
        }
        else
        {
        }

        switch (id)
        {
            case 1:
                imageCnt = 1;
                break;
            case 2:
                imageCnt = 2;
                break;
            case 3:
                imageCnt = 3;
                break;
            case 4:
                imageCnt = 4;
                break;
            case 5:
                imageCnt = 5;
                break;
            case 6:
                imageCnt = 6;
                break;
            default:
                break;
        }
    }

    public void CharaChange(GameObject obj1,GameObject obj2)
    {

    }

    private void SetNameLV()
    {
        switch(imageCnt)
        {
            case 1:
                CharName.GetComponent<Text>().text = "シャルロット";
                CharLV.GetComponent<Text>().text = "LV:" + Convert.ToString(sceneNavigator.GetComponent<StatusControl>().GetLV(pID1));
                break;
            case 2:
                CharName.GetComponent<Text>().text = "カーラ";
                CharLV.GetComponent<Text>().text = "LV:" + Convert.ToString(sceneNavigator.GetComponent<StatusControl>().GetLV(pID2));
                break;
            case 3:
                CharName.GetComponent<Text>().text = "ディアナ";
                CharLV.GetComponent<Text>().text = "LV:" + Convert.ToString(sceneNavigator.GetComponent<StatusControl>().GetLV(pID3));
                break;
            case 4:
                CharName.GetComponent<Text>().text = "リリー";
                CharLV.GetComponent<Text>().text = "LV:" + Convert.ToString(sceneNavigator.GetComponent<StatusControl>().GetLV(pID4));
                break;
            case 5:
                CharName.GetComponent<Text>().text = "エリザ";
                CharLV.GetComponent<Text>().text = "LV:" + Convert.ToString(sceneNavigator.GetComponent<StatusControl>().GetLV(pID5));
                break;
            case 6:
                CharName.GetComponent<Text>().text = "モルタリア";
                CharLV.GetComponent<Text>().text = "LV:" + Convert.ToString(sceneNavigator.GetComponent<StatusControl>().GetLV(pID6));
                break;
            default:
                break;
        }
    }
}

