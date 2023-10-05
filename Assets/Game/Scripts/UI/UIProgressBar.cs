using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIProgressBar : UIElement
{
    [SerializeField] protected GameObject _progressBar;
    [SerializeField] protected Slider _progressImage;
    [SerializeField] private Animator _progressAnimator;

    public Zone Zone { get; set; }


    public override void Show()
    {
        base.Show();
        HideBar();
    }

    public void SetValue(float value)
    {
        //_progressImage.value = value;
        _progressImage.DOValue(value, 0.5f);
    }

    public void ShowProgressBar()
    {
        _progressAnimator.Play("Show", 0, 0);
        _progressBar.SetActive(true);
    }

    public void HideProgressBar()
    {
        _progressAnimator.Play("Hide", 0, 0);
    }

    public void HideBar()
    {
        _progressBar.SetActive(false);
    }
}
