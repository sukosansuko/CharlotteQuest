using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backImage : MonoBehaviour
{
    public SpriteRenderer stageImage;

    public Sprite sp1;
    public Sprite sp2;
    public Sprite sp3;
    public Sprite sp4;
    public Sprite sp5;
    public Sprite sp6;

    private GameObject sceneNavigator;
    private int stageID;

    void Start()
    {
        sceneNavigator = GameObject.Find("SceneNavigator");
        stageID = sceneNavigator.GetComponent<StatusControl>().GetStageID();

        stageImage = GetComponent<SpriteRenderer>();
        ChangeImage();
    }

    void Update()
    {
    }

    private void ChangeImage()
    {
        switch (stageID)
        {
            case 1:
                stageImage.sprite = sp1;
                break;
            case 2:
                stageImage.sprite = sp2;
                break;
            case 3:
                stageImage.sprite = sp3;
                break;
            case 4:
                stageImage.sprite = sp4;
                break;
            case 5:
                stageImage.sprite = sp5;
                break;
            case 6:
                stageImage.sprite = sp6;
                break;
        }
    }
}
