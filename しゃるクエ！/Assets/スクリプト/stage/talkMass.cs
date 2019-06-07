using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Novel;

public class talkMass : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (mapChar.activScene == "W1")
        {
            if (stageMass.MassName == "talk1")      // 取得したﾏｽ名ごとに呼び出すｼﾅﾘｵを変える
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene1", "");
            }
            if (stageMass.MassName == "talk2")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene2", "");
            }
            if (stageMass.MassName == "talk3")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene3", "");
            }
        }
        if (mapChar.activScene == "W2")
        {
            if (stageMass.MassName == "talk1")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene4", "");
            }
            if (stageMass.MassName == "talk2")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene5", "");
            }
            if (stageMass.MassName == "talk3")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene6", "");
            }
        }
        if (mapChar.activScene == "W3")
        {
            if (stageMass.MassName == "talk1")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene7", "");
            }
            if (stageMass.MassName == "talk2")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene8", "");
            }
            if (stageMass.MassName == "talk3")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene9", "");
            }
        }
        if (mapChar.activScene == "W4")
        {
            if (stageMass.MassName == "talk1")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene10", "");
            }
            if (stageMass.MassName == "talk2")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene11", "");
            }
            if (stageMass.MassName == "talk3")
            {
                NovelSingleton.StatusManager.callJoker("wide/Scene12", "");
            }
        }
    }
}
