using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject ClickParticle;

    Vector3 mousePos;
    Vector2 screenWorldPos;

    private GameObject m_ClickParticle;
    private ParticleSystem m_ClickParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        m_ClickParticle = Instantiate(ClickParticle);

        m_ClickParticleSystem = m_ClickParticle.GetComponent<ParticleSystem>();
        m_ClickParticleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;

        mousePos.z = 20;
        screenWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("アカン");
            m_ClickParticle.transform.position = screenWorldPos;
            m_ClickParticleSystem.Play();
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_ClickParticleSystem.Stop();
        }
    }
}
