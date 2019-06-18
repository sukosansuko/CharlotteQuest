using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointMove : MonoBehaviour
{
    int moveCnt;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        moveCnt = 0;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        moveCnt++;
        if(moveCnt / 15 % 2 == 0)
        {
            image.transform.position += new Vector3(0, 0.01f, 0);
        }
        else
        {
            image.transform.position += new Vector3(0, -0.01f, 0);
        }
    }
}
