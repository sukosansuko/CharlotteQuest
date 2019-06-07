using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class home : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            click();
        }
    }
    public void click()
    {
        SceneNavigator.Instance.Change("ワールドマップ");
    }
}
