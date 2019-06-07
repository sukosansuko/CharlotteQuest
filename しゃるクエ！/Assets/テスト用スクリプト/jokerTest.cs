using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Novel;

public class jokerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            NovelSingleton.StatusManager.callJoker("wide/scene10", "");
        }
    }
}
