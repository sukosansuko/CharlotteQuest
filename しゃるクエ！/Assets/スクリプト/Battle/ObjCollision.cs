using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjCollision : MonoBehaviour
{
    private string EnterName;

    private string ExitName;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetEnterName()
    {
        return EnterName;
    }

    public string GetExitName()
    {
        return ExitName;
    }

    public void ResetEnter()
    {
        EnterName = "null";
    }

    public void ResetExit()
    {
        ExitName = "null";
    }

    //  触れたオブジェクトの名前取得(触れた瞬間のみ)
    void OnCollisionEnter2D(Collision2D collision)
    {
        EnterName = collision.gameObject.name;
    }

    //  離れたオブジェクトの名前取得(離れた瞬間のみ)
    void OnCollisionExit2D(Collision2D collision)
    {
        ExitName = collision.gameObject.name;
        Debug.Log(ExitName);
    }
}
