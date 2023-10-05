using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgress : UIElement
{
    public static UIProgress Instance;

    [SerializeField] private Slider _slider;

    private float _needVal;

    private void Awake()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();

        _slider.gameObject.SetActive(true);
        _slider.value = 0;
    }

    public void UpdateProg(float val)
    {
        _needVal = val;
    }

    private void LateUpdate()
    {
        if(_needVal - _slider.value > 0.5f)
        {
            _slider.value = Mathf.Lerp(_slider.value, _needVal, 0.15f);
        }
        else
        {
            _slider.value = _needVal;
        }
    }
}
