using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backButton : MonoBehaviour
{
    string activScene;
    // Start is called before the first frame update
    void Start()
    {
        activScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Push()
    {
        if(activScene.StartsWith("W"))
        {
            SceneNavigator.Instance.Change("ワールドマップ");
        }
    }
}
