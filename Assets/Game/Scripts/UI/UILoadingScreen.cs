using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingScreen : UIElement
{
    [SerializeField] private Animator _logoAnimator;

    private void Start()
    {
        _logoAnimator.Play("ShowLogo", 0, 0.0f);
    }
}
