using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

[RequireComponent(typeof(Animator))]
public class UIButtonSkin : UIElement
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void OnBtnClick()
    {
        m_animator.Play("SkinBtnShow", 0, 0.0f);

        GameManager.Instance.PlayHaptic(HapticTypes.MediumImpact);
    }
}
