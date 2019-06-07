using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMass : MonoBehaviour
{
    // Start is called before the first frame update
    public float posx, posy;
    public SpriteRenderer Mass;
    public static string stageName;
    // Use this for initialization
    void Start()
    {
        Mass = GetComponent<SpriteRenderer>();
        posx = Mass.transform.position.x;
        posy = Mass.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Push()
    {
        stageName = this.gameObject.transform.name;
        WorldmapChar.SavePos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.4f, 0);
        SceneNavigator.Instance.Change(stageName);
        Debug.Log(stageName);
    }
}
