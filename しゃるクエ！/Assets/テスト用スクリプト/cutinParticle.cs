using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutinParticle : MonoBehaviour
{
    ParticleSystem cutin;
    // Start is called before the first frame update
    void Start()
    {
        cutin = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetMouseButtonDown(0))
        {
            cutin.Play();
        }
    }
}
