using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageMass : MonoBehaviour
{
    public float posx, posy;
    public SpriteRenderer Mass;
    public static string MassName;
    string stageName;

    // Start is called before the first frame update
    void Start()
    {
        Mass = GetComponent<SpriteRenderer>();
        MassName = " ";
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
        mapChar.SavePos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.4f, 10);
        MassName = transform.name;
        Debug.Log(MassName);
    }
}
