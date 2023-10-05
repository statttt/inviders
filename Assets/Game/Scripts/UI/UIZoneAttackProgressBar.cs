using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIZoneAttackProgressBar : UIProgressBar
{
    public static UIZoneAttackProgressBar Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

}
