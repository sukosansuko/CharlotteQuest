using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animTest : MonoBehaviour
{
    Animator anim;
    CharacterController con;

    int num;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        con = GetComponent<CharacterController>();
        num = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //switch(num)
        //{
        //    case 1:
        //        anim.SetBool("isAttack", true);
        //        break;
        //    case 2:
        //        anim.SetBool("isDefence", true);
        //        break;
        //    case 3:
        //        anim.SetBool("isDeath", true);
        //        break;
        //    case 4:
        //        anim.SetBool("isDamage", true);
        //        break;
        //    default:
        //        anim.SetBool("isIdol", true);
        //        break;
        //}
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("isAttack", true);
            //num = 1;
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("isDefence", true);
            //num = 2;
        }
        else
        {
            anim.SetBool("isDefence", false);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetBool("isDeath", true);
            //num = 3;
        }
        else
        {
            anim.SetBool("isDeath", false);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetBool("isDamage", true);
            //num = 4;
        }
        else
        {
            anim.SetBool("isDamage", false);
        }
        if (Input.GetKey(KeyCode.None))
        {
            anim.SetBool("isIdol", true);
            //num = 0;
        }
        else
        {
            anim.SetBool("isIdol", false);
        }
    }
}
