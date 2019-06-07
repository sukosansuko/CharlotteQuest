using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectControl : MonoBehaviour
{
    Animator anim;

    private string animName;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        animName = "空き";
    }

    void Update()
    {
        if (animName == "空き")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("空き", false);
        }
    }

    public void SetAnim()
    {


        //  単体攻撃
        if (animName == "ひっかく")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("ひっかく", false);
        }
        if (animName == "シャイニングなんちゃら")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("シャイニングなんちゃら", false);
        }
        if (animName == "プロテクト")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("プロテクト", false);
        }
        if (animName == "ホーリー")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("ホーリー", false);
        }
        if (animName == "呪い")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("呪い", false);
        }
        if (animName == "回復")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("回復", false);
        }
        if (animName == "氷1")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("氷1", false);
        }
        if (animName == "氷2")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("氷2", false);
        }
        if (animName == "爆発1")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("爆発1", false);
        }
        if (animName == "爆発2")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("爆発2", false);
        }
        if (animName == "通常攻撃(魔法)")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("通常攻撃(魔法)", false);
        }
        if (animName == "通常攻撃(物理)")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("通常攻撃(物理)", false);
        }
        if (animName == "闇1")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("闇1)", false);
        }
        if (animName == "闇2")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("闇2", false);
        }
        if (animName == "雷1")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("雷1", false);
        }
        if (animName == "雷2")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("雷2", false);
        }



        //  全体攻撃
        if (animName == "ジャッジメント・デイ")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("ジャッジメント・デイ", false);
        }
        if (animName == "地面")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("地面", false);
        }
        if (animName == "煉獄斬")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("煉獄斬", false);
        }
        if (animName == "氷3")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("氷3", false);
        }
        if (animName == "雷3")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("雷3", false);
        }
        if (animName == "爆発3")
        {
            anim.SetBool(animName, true);
        }
        else
        {
            anim.SetBool("爆発3", false);
        }

        animName = "空き";
    }

    public void SetAnimName(string name)
    {
        animName = name;
        SetAnim();
    }
}
