using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTarget : MonoBehaviour
{
    private GameObject battleManager;

    void Start()
    {
        battleManager = GameObject.Find("BattleManager");
    }

    void Update()
    {
        
    }

    public void SetName()
    {
        battleManager.GetComponent<command>().InitTarget();
        battleManager.GetComponent<command>().SetSkillName(gameObject.name);
    }

    public void SetTargetReceive()
    {
        battleManager.GetComponent<BattleScene>().ActionSelectEnd(this.gameObject);
    }
}
