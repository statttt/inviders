using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyProgressBar : UIProgressBar
{
    public static UIEnemyProgressBar Instance { get; private set;}

    private void Awake()
    {
        Instance = this;
    }


}
