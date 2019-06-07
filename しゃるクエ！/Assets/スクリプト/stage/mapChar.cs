using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mapChar : MonoBehaviour {

    public static string activScene;

    public SpriteRenderer charImage;
    public static Vector3 InitCharPos;        // ｽﾃｰｼﾞﾏｯﾌﾟ上のｷｬﾗｸﾀｰ初期位置(talk1の場所)
    public static Vector3 SavePos = new Vector3(0, 0, 0);

    public Sprite sp1;
    public Sprite sp2;
    public Sprite sp3;
    public Sprite sp4;
    public Sprite sp5;
    public Sprite sp6;

    private GameObject sceneNavigator;
    private int charID;
    // Use this for initialization
    void Start()
    {
        sceneNavigator = GameObject.Find("SceneNavigator");
        charID = sceneNavigator.GetComponent<StatusControl>().Get1Player();

        charImage = GetComponent<SpriteRenderer>();
        ChangeImage();

        InitCharPos = new Vector3(GameObject.Find("talk1").transform.position.x, GameObject.Find("talk1").transform.position.y + 1.4f, 0);
        charImage.transform.position = InitCharPos;
        activScene = SceneManager.GetActiveScene().name;
    }
    
	// Update is called once per frame
	void Update ()
    {
        if (SavePos == new Vector3(0, 0, 0))
        {
            SavePos = InitCharPos;
        }
        charImage.transform.position = SavePos;
    }

    private void ChangeImage()
    {
        switch (charID)
        {
            case 1:
                charImage.sprite = sp1;
                break;
            case 2:
                charImage.sprite = sp2;
                break;
            case 3:
                charImage.sprite = sp3;
                break;
            case 4:
                charImage.sprite = sp4;
                break;
            case 5:
                charImage.sprite = sp5;
                break;
            case 6:
                charImage.sprite = sp6;
                break;
        }
    }
}
