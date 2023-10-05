using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class UIButtonHaptic : UIElement
{
    [SerializeField] private GameObject _hapticOnImage;

    private void Start()
    {
        UpdateHaptic();
    }

    public void OnClickBtn()
    {
        GameManager.Instance.HapticEnabled = !GameManager.Instance.HapticEnabled;

        UpdateHaptic();
    }

    private void UpdateHaptic()
    {
        bool isHapticEnabled = GameManager.Instance.HapticEnabled;

        _hapticOnImage.SetActive(isHapticEnabled);

        if (isHapticEnabled)
        {
            GameManager.Instance.PlayHaptic(HapticTypes.MediumImpact);
        }
    }
}
