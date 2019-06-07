using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchTest : MonoBehaviour
{
    int touchCnt;
    // Start is called before the first frame update
    void Start()
    {
        touchCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            touchCnt = 0;
        }
        if(Input.GetMouseButton(0))
        {
            touchCnt++;
        }
        if (touchCnt >= 60)
        {
            this.gameObject.transform.position = new Vector3(5, 0, 0);
        }
        else
        {
            this.gameObject.transform.position = new Vector3(-5, 0, 0);
        }
    }
}
