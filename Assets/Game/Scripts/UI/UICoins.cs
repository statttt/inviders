using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoins : UIElement
{
    public static UICoins Instance;

    [SerializeField] private Text _textCoinsCnt;
    [SerializeField] private RectTransform _coins;

    public event Action OnUpdate;

    private void Awake()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();

        UpdateText();
    }

    public void UpdateText()
    {
        _textCoinsCnt.text = GameManager.Instance.Coins.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_coins);
        if (UIUpgrade.Instance != null)
        {
            UIUpgrade.Instance.UpdateButtons();
        }
    }
}
