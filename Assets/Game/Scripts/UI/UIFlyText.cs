using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFlyText : UIElement
{
    public static UIFlyText Instance;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();

        _animator.gameObject.SetActive(false);
    }

    public void Play()
    {
        if(!_animator.gameObject.activeInHierarchy)
        {
            _animator.gameObject.SetActive(true);
        }

        _animator.Play("FlyText", 0, 0.0f);
    }
}
