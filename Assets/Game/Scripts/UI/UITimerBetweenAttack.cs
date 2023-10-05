using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITimerBetweenAttack : UIElement
{
    public static UITimerBetweenAttack Instance;

    [SerializeField] private Image _fieldImageRed;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private GameObject _viewTimerInfo;
    [SerializeField] private float _maxValue;
    [SerializeField] private Animation _scaleAnimation;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateTimer(float currentValue)
    {
        _fieldImageRed.fillAmount = 1f - currentValue / _maxValue;
        int min = (int)((_maxValue - currentValue)/ 60f);
        int sec = (int)((_maxValue - currentValue) % 60f);
        _timerText.text = min.ToString("00") + ":" + sec.ToString("00");
        if(min == 0 && sec <= 10)
        {
            _scaleAnimation.Play();
        }
        else
        {
            _scaleAnimation.Stop();
        }
    }

    public void ShowTimerInfo(float maxValue)
    {
        _viewTimerInfo.SetActive(true);
        _maxValue = maxValue;
        _timerText.gameObject.SetActive(true);
    }

    public void HideTimerInfo()
    {
        _viewTimerInfo.SetActive(false);
    }

    public void HideTimer()
    {
        _timerText.gameObject.SetActive(false);
    }
}
