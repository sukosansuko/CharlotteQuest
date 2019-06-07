using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutinAnim : MonoBehaviour
{
    Animator cutin;
    // Start is called before the first frame update
    void Start()
    {
        cutin = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            cutin.Play("");
        }
    }
}
