using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notDiscard : MonoBehaviour
{
    // アタッチしたオブジェクトを破棄しないためのスクリプト
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
