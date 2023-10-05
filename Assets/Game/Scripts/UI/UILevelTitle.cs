using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelTitle : UIElement
{
    [SerializeField] private Text titleText;
    [SerializeField] private string prefix;

    public override void Show()
    {
        titleText.text = string.Format($"{prefix} {GameManager.Instance.LevelNum + 1}");

        base.Show();
    }
}
