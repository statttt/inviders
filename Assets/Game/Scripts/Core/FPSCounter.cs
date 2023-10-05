using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private Text _fpsText;
    [SerializeField] private float m_refreshTime = 0.5f;

    private int m_frameCounter = 0;
    private float m_timeCounter = 0.0f;


    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            _fpsText.text = $"{(int)(m_frameCounter / m_timeCounter)}";
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
    }
}
